using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public enum EShieldState
    {
        None = 0,
        PerfectGuard,
        Guard,
        Thrown
    }
    public EShieldState shieldState { private set; get; } = EShieldState.None;

    [SerializeField]
    float perfectGuardTime = 0.5f;
    float guardTime = 0;

    [SerializeField] SimpleVFX perfectHitVFX;
    [SerializeField] SimpleVFX HitVFX;
    public void GuardUp()
    {
        shieldState = EShieldState.PerfectGuard;
        guardTime = perfectGuardTime;
    }
    private void Update()
    {
        if (shieldState == EShieldState.PerfectGuard)
        {
            UpdatePerfectGuard();
        }
    }

    private void UpdatePerfectGuard()
    {
        guardTime -= Time.deltaTime;
        if (guardTime < 0) shieldState = EShieldState.Guard;
    }

    internal void OnHitBlock(bool isPerfect)
    {
        if (isPerfect)
        {
            perfectHitVFX.Play();
        }
        else
        {
            HitVFX.Play();
        }
    }

    internal void GuardDown()
    {
        shieldState = EShieldState.None;
    }
}
