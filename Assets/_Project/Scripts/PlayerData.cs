using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float maxMana { get; private set; }
    public float maxHealth { get; private set; }
    public float currentMana { get; private set; }
    public float currentHealth { get; private set; }

    //TODO: add skill IDs to profile
    public PlayerData(float MaxHealth, float MaxMana)
    {
        this.maxHealth = MaxHealth;
        this.maxMana = MaxMana;
        this.currentMana = maxMana;
        this.currentHealth = maxHealth;
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
}