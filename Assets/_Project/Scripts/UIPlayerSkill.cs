using UnityEngine;
using UnityEngine.UI;

public class UIPlayerSkill : MonoBehaviour
{
    [SerializeField] Image icon;
    [field: SerializeField] public KeyCode KeyCode { private set; get; }
    [field: SerializeField] public int MouseButton { private set; get; } = -1;
    public SkillSLot skillSlot { private set; get; }
    public void SetSlot(SkillSLot slot)
    {
        skillSlot = slot;
        skillSlot.OnSkillChanged = OnSkillChanged;
        UpdateIcon(slot);

    }

    void OnSkillChanged(SkillSLot slot)
    {
        UpdateIcon(slot);

    }
    private void UpdateIcon(SkillSLot slot)
    {
        if (slot.skillToExecute == null) return;
        icon.sprite = slot.skillToExecute.Icon;
    }
}
