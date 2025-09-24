using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] PlayerShield leftShield;
    [SerializeField] PlayerShield rightShield;
    [SerializeField] SimpleVFX hitVFX;
    [SerializeField] PlayerIKController IKController;

    public float MaxMana { private set; get; } = 10;
    public float MaxHealth { private set; get; } = 10;

    public float CurrentMana { private set; get; } = 0;
    public float CurrentHealth { private set; get; } = 0;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentMana = MaxMana;
    }
    internal void TakeHit(BasicEnemy basicEnemy)
    {
        Vector3 DirToEnemy = basicEnemy.transform.position - this.transform.position;
        float angle = Vector3.SignedAngle(this.transform.forward, DirToEnemy, Vector3.up);
        if (Mathf.Abs(angle) > 90)
        {
            TakeDamage(basicEnemy.damage, 20);
            return;
        }

        bool isRightSide = angle < 0;
        PlayerShield hitShield = isRightSide ? leftShield : rightShield;

        switch (hitShield.shieldState)
        {
            case PlayerShield.EShieldState.PerfectGuard:
                RestoreMana(1);
                hitShield.OnHitBlock(true);
                IKController.SetIKWeight(isRightSide ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand, 1);
                break;
            case PlayerShield.EShieldState.Guard:
                hitShield.OnHitBlock(false);
                RestoreMana(1);
                TakeDamage(basicEnemy.damage / 3, 3);
                IKController.SetIKWeight(isRightSide ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand, 1);
                break;
            default:
                TakeDamage(basicEnemy.damage, 20);
                break;
        }
    }

    private void RestoreMana(float v)
    {
        CurrentMana += v;
        CurrentMana = Mathf.Clamp(CurrentMana, 0, MaxMana);
    }

    private void TakeDamage(float v, int particles)
    {
        CurrentHealth -= v;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        hitVFX.Play(particles);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("StartGuard");
            GuardUp();
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            anim.SetTrigger("EndGuard");
            GuardDown();
        }
    }

    private void GuardUp()
    {
        if (leftShield.shieldState != PlayerShield.EShieldState.Thrown) leftShield.GuardUp();
        if (rightShield.shieldState != PlayerShield.EShieldState.Thrown) rightShield.GuardUp();
    }
    private void GuardDown()
    {
        if (leftShield.shieldState != PlayerShield.EShieldState.Thrown) leftShield.GuardDown();
        if (rightShield.shieldState != PlayerShield.EShieldState.Thrown) rightShield.GuardDown();
    }
}
