using UnityEngine;

public abstract class PlayerSkill : ScriptableObject
{
    [field: SerializeField] public float Cost { private set; get; }
    [field: SerializeField] public int Level { private set; get; }

    public virtual void Execute()
    {

    }
}
