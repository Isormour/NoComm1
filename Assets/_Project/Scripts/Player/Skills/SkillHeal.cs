using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/Heal", order = 1)]
public class SkillHeal : Skill
{
    public override bool Execute()
    {
        return false;
    }

    public override bool StartCharge()
    {
        throw new System.NotImplementedException();
    }

    public override bool UpdateCharge()
    {
        throw new System.NotImplementedException();
    }

    public override bool ReleaseCharge()
    {
        throw new System.NotImplementedException();
    }
}
