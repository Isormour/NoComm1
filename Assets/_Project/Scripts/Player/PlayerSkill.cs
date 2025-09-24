using UnityEngine;

public abstract class PlayerSkill : ScriptableObject
{
    public enum ESkillType
    {
        Instant,
        Charge,
    }
    [field: SerializeField] public float Cost { private set; get; }
    [field: SerializeField] public int Level { private set; get; }
    [field: SerializeField] public ESkillType skilltype { private set; get; }

    [field: SerializeField] public Sprite Icon { private set; get; }
    [field: SerializeField] public int id { private set; get; }

    public virtual void Execute()
    {

    }
    public virtual void StartCharge()
    {

    }
    public virtual void UpdateCharge()
    {

    }
    public virtual void ReleaseCharge()
    {

    }
}
