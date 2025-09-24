using System;
using UnityEngine;
using ESkillType = PlayerSkill.ESkillType;
[System.Serializable]

public class SkillSLot
{
    public KeyCode pressKeyCode;
    public PlayerSkill skillToExecute { private set; get; }
    public int mouseButton = -1;

    bool charging = false;
    public Action<SkillSLot> OnSkillChanged;
    public void SetSkill(PlayerSkill skill)
    {
        this.skillToExecute = skill;
        OnSkillChanged?.Invoke(this);
    }
    public void CheckSkillInput()
    {
        if (skillToExecute == null) return;

        if (mouseButton < 0)
        {
            if (Input.GetKeyDown(pressKeyCode))
            {
                OnKeyDown();
            }
            if (Input.GetKeyUp(pressKeyCode))
            {
                OnKeyUp();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(mouseButton))
            {
                OnKeyDown();
            }
            if (Input.GetMouseButtonUp(mouseButton))
            {
                OnKeyUp();
            }
        }

        if (skillToExecute.skilltype == ESkillType.Charge && charging)
        {
            skillToExecute.UpdateCharge();
        }
    }

    private void OnKeyUp()
    {
        if (skillToExecute.skilltype != ESkillType.Charge)
            return;
        skillToExecute.ReleaseCharge();
        charging = false;
    }

    private void OnKeyDown()
    {
        if (skillToExecute.skilltype == ESkillType.Instant)
        {
            skillToExecute.Execute();
            return;
        }
        charging = true;
        skillToExecute.StartCharge();
    }
}