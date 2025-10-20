using UnityEngine;
using System.Collections;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance { get; private set; }

    [Header("Krzywa manipulacji czasem")]
    [Tooltip("X = czas (sekundy), Y = mno¿nik Time.timeScale")]
    public AnimationCurve timeCurve = AnimationCurve.Linear(0, 1, 1, 1);

    [Header("Ustawienia")]
    [Tooltip("Globalny mno¿nik na wartoœæ krzywej (np. 1 = normalnie, 0.5 = wolniej)")]
    public float globalMultiplier = 1f;

    [Tooltip("Czy automatycznie przywraca TimeScale po zakoñczeniu krzywej?")]
    public bool resetTimeAfter = true;

    private bool isManipulating = false;
    private Coroutine manipulationRoutine;
    private float defaultFixedDeltaTime;

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        defaultFixedDeltaTime = Time.fixedDeltaTime;
    }

    /// <summary>
    /// Rozpoczyna manipulacjê czasem wg podanej krzywej (jeœli nie trwa ju¿ inna manipulacja).
    /// </summary>
    public void ManipulateTime(AnimationCurve curve, float multiplier = 1f)
    {
        if (isManipulating)
            return;

        timeCurve = curve;
        globalMultiplier = multiplier;

        if (manipulationRoutine != null)
            StopCoroutine(manipulationRoutine);

        manipulationRoutine = StartCoroutine(ManipulateTimeRoutine());
    }

    private IEnumerator ManipulateTimeRoutine()
    {
        isManipulating = true;
        float duration = timeCurve.keys[timeCurve.length - 1].time;
        float startTime = Time.unscaledTime;

        while (true)
        {
            float elapsed = Time.unscaledTime - startTime;

            // Koniec manipulacji
            if (elapsed > duration)
                break;

            float curveValue = timeCurve.Evaluate(elapsed);
            float newTimeScale = Mathf.Max(0f, curveValue * globalMultiplier);

            Time.timeScale = newTimeScale;
            Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;

            yield return null;
        }

        // Przywracanie normalnego czasu
        if (resetTimeAfter)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = defaultFixedDeltaTime;
        }

        isManipulating = false;
        manipulationRoutine = null;
    }

    /// <summary>
    /// Zwraca informacjê, czy aktualnie trwa manipulacja czasem.
    /// </summary>
    public bool IsManipulating() => isManipulating;

    /// <summary>
    /// Natychmiast zatrzymuje manipulacjê i przywraca normalny czas.
    /// </summary>
    public void ResetTime()
    {
        if (manipulationRoutine != null)
            StopCoroutine(manipulationRoutine);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
        isManipulating = false;
        manipulationRoutine = null;
    }
}
