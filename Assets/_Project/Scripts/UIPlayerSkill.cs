using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerSkill : MonoBehaviour
{
    [SerializeField] Image icon;
    [field: SerializeField] public KeyCode KeyCode { private set; get; }
    [field: SerializeField] public int MouseButton { private set; get; } = -1;

    [SerializeField] private Image timeSprite;
    [SerializeField] private TextMeshProUGUI timeText;
    public SkillSLot skillSlot { private set; get; }

    private void Update()
    {
        if (skillSlot == null)
            return;
        timeSprite.fillAmount = skillSlot.CooldownPrecent;
        var cooldownTime = skillSlot.Cooldown;
        timeText.text = cooldownTime > 0 ? skillSlot.Cooldown.ToString("0.0") : "";
    }

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
