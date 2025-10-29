using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraVolumeTweener : MonoBehaviour
{
    static CameraVolumeTweener _instance;
    public static CameraVolumeTweener Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("CameraVolumeTweener");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<CameraVolumeTweener>();
            }
            return _instance;
        }
    }

    [Header("Opcjonalne rêczne referencje (jeœli chcesz nadpisaæ)")]
    public Camera defaultCamera;
    public Volume defaultVolume;

    // przechowuje oryginalne wartoœci (tylko pierwszy zapis)
    Dictionary<string, float> originalValues = new Dictionary<string, float>();
    // aktywne korutyny dla danego klucza (¿eby móc je zatrzymaæ)
    Dictionary<string, Coroutine> activeTweens = new Dictionary<string, Coroutine>();
    // oryginalne overrideState dla VolumeParameter
    Dictionary<string, bool> originalOverrideStates = new Dictionary<string, bool>();

    // domyœlny easing (³atwo dynamiczne przejœcie)
    AnimationCurve defaultCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    void Awake()
    {
        if (defaultCamera == null)
            defaultCamera = Camera.main;

        if (defaultVolume == null)
        {
            defaultVolume = FindObjectOfType<Volume>();
            if (defaultVolume == null)
                Debug.LogWarning("CameraVolumeTweener: nie znaleziono ¿adnego Volume w scenie. Dodaj Volume albo przypisz rêcznie defaultVolume.");
        }
    }

    #region Public static API (wywo³uj z ka¿dego miejsca)

    // Camera FOV
    public static void TweenCameraFOV(float targetFOV, float duration = 0.2f, float holdTime = 0f, AnimationCurve curve = null, Camera cam = null)
    {
        cam = cam ?? Instance.defaultCamera ?? Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("CameraVolumeTweener: brak kamery (podaj Camera jako parametr albo ustaw defaultCamera).");
            return;
        }

        string key = $"camera:{cam.GetInstanceID()}:FOV";
        Instance.StartNamedTween(key, Instance.TemporaryFloatCoroutine(
            key,
            () => cam.fieldOfView,
            v => cam.fieldOfView = v,
            targetFOV, duration, holdTime, curve));
    }

    // Bloom: threshold
    public static void TweenBloomThreshold(float target, float duration = 0.2f, float holdTime = 0f, AnimationCurve curve = null)
    {
        var volume = Instance.defaultVolume;
        if (!TryGetBloom(volume, out var bloom)) return;
        string key = VolumeKey(volume, "Bloom.threshold");
        Instance.StartNamedTween(key, Instance.TemporaryVolumeParamCoroutine(
            key,
            () => bloom.threshold.value,
            v => bloom.threshold.value = v,
            target, duration, holdTime, curve,
            () => bloom.threshold));
    }

    // Bloom: intensity
    public static void TweenBloomIntensity(float target, float duration = 0.2f, float holdTime = 0f, AnimationCurve curve = null)
    {
        var volume = Instance.defaultVolume;
        if (!TryGetBloom(volume, out var bloom)) return;
        string key = VolumeKey(volume, "Bloom.intensity");
        Instance.StartNamedTween(key, Instance.TemporaryVolumeParamCoroutine(
            key,
            () => bloom.intensity.value,
            v => bloom.intensity.value = v,
            target, duration, holdTime, curve,
            () => bloom.intensity));
    }

    // Bloom: scatter
    public static void TweenBloomScatter(float target, float duration = 0.2f, float holdTime = 0f, AnimationCurve curve = null)
    {
        var volume = Instance.defaultVolume;
        if (!TryGetBloom(volume, out var bloom)) return;
        string key = VolumeKey(volume, "Bloom.scatter");
        Instance.StartNamedTween(key, Instance.TemporaryVolumeParamCoroutine(
            key,
            () => bloom.scatter.value,
            v => bloom.scatter.value = v,
            target, duration, holdTime, curve,
            () => bloom.scatter));
    }

    // Saturation (Color Adjustments)
    public static void TweenSaturation(float target, float duration = 0.2f, float holdTime = 0f, AnimationCurve curve = null)
    {
        var volume = Instance.defaultVolume;
        if (!TryGetColorAdjustments(volume, out var color)) return;
        string key = VolumeKey(volume, "ColorAdjustments.saturation");
        Instance.StartNamedTween(key, Instance.TemporaryVolumeParamCoroutine(
            key,
            () => color.saturation.value,
            v => color.saturation.value = v,
            target, duration, holdTime, curve,
            () => color.saturation));
    }

    #endregion

    #region Korutyny i pomocnicze

    // Startuje i zapisuje korutynê pod kluczem (jeœli istnieje dotychczasowa, zatrzymuje j¹)
    void StartNamedTween(string key, IEnumerator routine)
    {
        if (activeTweens.TryGetValue(key, out var prev) && prev != null)
        {
            StopCoroutine(prev); // zatrzymaj poprzedni; oryginalna wartoœæ zosta³a zapisana przy pierwszym wywo³aniu
            activeTweens.Remove(key);
        }
        var c = StartCoroutine(routine);
        activeTweens[key] = c;
    }

    IEnumerator TemporaryFloatCoroutine(string key, Func<float> getter, Action<float> setter, float target, float duration, float holdTime, AnimationCurve curve)
    {
        // zapisz oryginaln¹ wartoœæ tylko jeœli jej jeszcze nie ma
        if (!originalValues.ContainsKey(key))
        {
            originalValues[key] = getter();
        }

        var usedCurve = curve ?? defaultCurve;
        float original = originalValues[key];

        // do
        yield return StartCoroutine(DoLerp(getter, setter, original, target, duration, usedCurve));
        // hold
        if (holdTime > 0f) yield return new WaitForSeconds(holdTime);
        // powrót
        yield return StartCoroutine(DoLerp(getter, setter, target, original, duration, usedCurve));

        // upewnij siê, ¿e wartoœæ dok³adnie przywrócona
        setter(original);

        // cleanup
        originalValues.Remove(key);
        activeTweens.Remove(key);
    }

    IEnumerator TemporaryVolumeParamCoroutine(string key, Func<float> getter, Action<float> setter, float target, float duration, float holdTime, AnimationCurve curve, Func<VolumeParameter<float>> paramProvider)
    {
        // zapisz oryginaln¹ wartoœæ tylko jeœli jej jeszcze nie ma
        if (!originalValues.ContainsKey(key))
        {
            originalValues[key] = getter();

            // jeœli parametr istnieje, zapisz jego overrideState
            var p = paramProvider?.Invoke();
            if (p != null)
                originalOverrideStates[key] = p.overrideState;
        }

        // wymuœ overrideState = true na czas tweena
        var param = paramProvider?.Invoke();
        if (param != null)
            param.overrideState = true;

        var usedCurve = curve ?? defaultCurve;
        float original = originalValues[key];

        // do
        yield return StartCoroutine(DoLerp(getter, setter, original, target, duration, usedCurve));
        // hold
        if (holdTime > 0f) yield return new WaitForSeconds(holdTime);
        // powrót
        yield return StartCoroutine(DoLerp(getter, setter, target, original, duration, usedCurve));

        // restore dok³adnie
        setter(original);

        // przywróæ overrideState jeœli mieliœmy oryginalny
        if (param != null && originalOverrideStates.TryGetValue(key, out var hadOverride))
            param.overrideState = hadOverride;

        // cleanup
        originalValues.Remove(key);
        originalOverrideStates.Remove(key);
        activeTweens.Remove(key);
    }

    IEnumerator DoLerp(Func<float> getter, Action<float> setter, float from, float to, float duration, AnimationCurve curve)
    {
        if (duration <= 0f)
        {
            setter(to);
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            float ct = curve.Evaluate(t);
            setter(Mathf.Lerp(from, to, ct));
            elapsed += Time.deltaTime;
            yield return null;
        }
        setter(to);
    }

    #endregion

    #region Volume helpers

    static string VolumeKey(Volume v, string paramName)
    {
        int id = v != null ? v.GetInstanceID() : 0;
        return $"volume:{id}:{paramName}";
    }

    static bool TryGetBloom(Volume volume, out Bloom bloom)
    {
        bloom = null;
        if (volume == null)
        {
            volume = Instance.defaultVolume;
            if (volume == null)
            {
                Debug.LogWarning("CameraVolumeTweener: brak Volume (ustaw defaultVolume albo dodaj Volume w scenie).");
                return false;
            }
        }
        if (volume.profile == null)
        {
            Debug.LogWarning("CameraVolumeTweener: Volume nie ma przypisanego VolumeProfile.");
            return false;
        }
        if (!volume.profile.TryGet<Bloom>(out bloom))
        {
            Debug.LogWarning("CameraVolumeTweener: VolumeProfile nie zawiera Bloom override.");
            return false;
        }
        return true;
    }

    static bool TryGetColorAdjustments(Volume volume, out ColorAdjustments color)
    {
        color = null;
        if (volume == null)
        {
            volume = Instance.defaultVolume;
            if (volume == null)
            {
                Debug.LogWarning("CameraVolumeTweener: brak Volume (ustaw defaultVolume albo dodaj Volume w scenie).");
                return false;
            }
        }
        if (volume.profile == null)
        {
            Debug.LogWarning("CameraVolumeTweener: Volume nie ma przypisanego VolumeProfile.");
            return false;
        }
        if (!volume.profile.TryGet<ColorAdjustments>(out color))
        {
            Debug.LogWarning("CameraVolumeTweener: VolumeProfile nie zawiera Color Adjustments override.");
            return false;
        }
        return true;
    }

    #endregion
}
