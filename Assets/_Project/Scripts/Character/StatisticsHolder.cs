using System;
using UnityEngine;
using UnityEngine.Events;


public class StatisticsHolder : MonoBehaviour
{
    public UnityEvent<DamageData> OnDamage;
    public UnityEvent<DamageData> OnDeath;
    public EShieldState EShieldState { get; private set; }
    public float Damage => damage;

    public DamageCalculator DamageCalculator { get; set; } = new DamageCalculator();
    [SerializeField] SimpleVFX hitVFX;

    [SerializeField] private float maxMana;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damage;
    public float currentMana { get; private set; }
    public float currentHealth { get; private set; }
    
    public float currentPercentHealth => currentHealth / maxHealth;
    public float currentPercentMana => currentMana / maxMana;

    private void Awake()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
    }

    public void SetBlockState(EShieldState shieldState)
    {
        EShieldState = shieldState;
    }

    internal void ChangeAmountMana(float v)
    {
        currentMana += v;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
    }

    internal void ChangeAmountHealth(float v)
    {
        currentHealth += v;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void TakeDamage(DamageData damageData)
    {
        damageData = DamageCalculator.CalculateDamage(damageData);
        ChangeAmountHealth(-damageData.Damage);
        if(hitVFX!=null && damageData.Particles > 0)
            hitVFX.Play(damageData.Particles);
    }
}