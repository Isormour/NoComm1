using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public enum ESkillType
    {
        Instant,
        Charge,
    }
    [field: SerializeField] public float Cost { private set; get; }
    [field: SerializeField] public int Level { private set; get; }
    [field: SerializeField] public float CooldownTime { private set; get; }
    [field: SerializeField] public ESkillType skilltype { private set; get; }

    [field: SerializeField] public Sprite Icon { private set; get; }
    [field: SerializeField] public int id { private set; get; }

    public SkillData SkillData { get; private set; }
    public virtual void InitSkillData(SkillData skillData)
    {
        SkillData = skillData;
    }

    public abstract void Execute();

    public abstract void StartCharge();
    public abstract void UpdateCharge();
    public abstract void ReleaseCharge();
}
