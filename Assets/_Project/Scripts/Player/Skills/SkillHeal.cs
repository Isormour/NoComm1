using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/Heal", order = 1)]
public class SkillHeal : Skill
{
    public override void Execute()
    {
        
    }

    public override void StartCharge()
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateCharge()
    {
        throw new System.NotImplementedException();
    }

    public override void ReleaseCharge()
    {
        throw new System.NotImplementedException();
    }
}
