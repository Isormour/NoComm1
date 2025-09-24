using UnityEngine;

public class UIPlayerPanelSkills : UIPlayerControl
{
    [SerializeField] UIPlayerSkill[] skills;
    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);
        SetSkillsInSlots(controller);
    }

    private void SetSkillsInSlots(PlayerController controller)
    {
        foreach (var item in skills)
        {
            for (int i = 0; i < controller.skillSlots.Length; i++)
            {
                SkillSLot currentSlot = controller.skillSlots[i];
                bool isMouseKey = item.KeyCode == KeyCode.None;
                bool sameKey = item.KeyCode == currentSlot.pressKeyCode;

                if (sameKey && !isMouseKey)
                {
                    item.SetSlot(currentSlot);
                    break;
                }
                sameKey = item.MouseButton == currentSlot.mouseButton;
                if (sameKey && isMouseKey && item.MouseButton > -1)
                {
                    item.SetSlot(currentSlot);
                }
            }
        }
    }
}
