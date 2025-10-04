using System;
using _Project.Scripts.Character;
using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.Events;


public class StatisticsHolder : MonoBehaviour
{
    public UnityEvent<DamageData> OnDamage;
    public UnityEvent<DamageData> OnDeath;
    public EShieldState EShieldState { get; private set; }
    public float Damage => damage;

    public bool IsDead => currentHealth <= 0;

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

    internal bool ChangeAmountMana(float v)
    {
        if (currentMana < v)
            return false;
        currentMana += v;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        return true;
    }

    internal void ChangeAmountHealth(float v)
    {
        currentHealth += v;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    public void TakeDamage(DamageData damageData)
    {
        if (IsDead)
            return;
        damageData = DamageCalculator.CalculateDamage(damageData);
        ChangeAmountHealth(-damageData.Damage);
        OnDamage.Invoke(damageData);
        if (currentHealth <= 0)
        {
            OnDeath.Invoke(damageData);
        }
        if(hitVFX!=null && damageData.Particles > 0)
            hitVFX.Play(damageData.Particles);
    }
}