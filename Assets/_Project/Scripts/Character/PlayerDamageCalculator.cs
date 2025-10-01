using UnityEngine;

public class PlayerDamageCalculator : DamageCalculator
{
    private PlayerShield leftShield;
    private PlayerShield rightShield;
    
    public PlayerDamageCalculator(PlayerShield leftShield, PlayerShield rightShield)
    {
        this.leftShield = leftShield;
        this.rightShield = rightShield;
    }
    
    public override DamageData CalculateDamage(DamageData damageData)
    {
        float angle = damageData.AngleToEnemy;
        if (Mathf.Abs(angle) > 90)
        {
            return damageData;
        }

        bool isRightSide = angle < 0;
        PlayerShield hitShield = isRightSide ? leftShield : rightShield;


        switch (hitShield.shieldState)
        {
            case EShieldState.PerfectGuard:
                hitShield.OnHitBlock(true);
                damageData.Damage = 0;
                damageData.Particles = 0;
                break;

            case EShieldState.Guard:
                hitShield.OnHitBlock(false);
                damageData.Damage /= 3;
                damageData.Particles = 3;
                break;
        }

        return damageData;
    }
}
