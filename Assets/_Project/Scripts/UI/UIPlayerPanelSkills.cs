using _Project.Scripts.Character;
using UnityEngine;

public class UIPlayerPanelSkills : UIPlayerControl
{
    [SerializeField] private UIPlayerSkill leftMouseSkill;
    [SerializeField] private UIPlayerSkill rightMouseSkill;
    [SerializeField] private UIPlayerSkill[] skills;

    private SkillsController skillsController;
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        skillsController = controller.GetComponent<SkillsController>();
        SetSkillsInSlots();
    }

    private void SetSkillsInSlots()
    {
        leftMouseSkill.SetSlot(skillsController.Attack1);
        rightMouseSkill.SetSlot(skillsController.Attack2);
        for (int i = 0; i < skills.Length; i++)
        {
            if (skillsController.SkillSlots.Length <= i)
                return;
            skills[i].SetSlot(skillsController.SkillSlots[i]);
        }
    }
}
