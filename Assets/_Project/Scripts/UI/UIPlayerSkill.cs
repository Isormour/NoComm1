using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerSkill : MonoBehaviour
{
    [SerializeField] Image icon;

    [SerializeField] private Image timeSprite;
    [SerializeField] private TextMeshProUGUI timeText;
    public SkillSlot skillSlot { private set; get; }

    private void Update()
    {
        if (skillSlot == null || skillSlot.SkillToExecute == null)
        {
            timeSprite.fillAmount = 1;
            timeText.text = "N/A";
            return;
        }

        timeSprite.fillAmount = skillSlot.CooldownPrecent;
        var cooldownTime = skillSlot.Cooldown;
        timeText.text = cooldownTime > 0 ? skillSlot.Cooldown.ToString("0.0") : "";
        icon.sprite = skillSlot.SkillToExecute.Icon;
    }

    public void SetSlot(SkillSlot slot)
    {
        skillSlot = slot;

    }
}
