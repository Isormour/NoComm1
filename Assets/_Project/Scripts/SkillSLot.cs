using System;
using UnityEngine;
using ESkillType = Skill.ESkillType;
[System.Serializable]

public class SkillSLot
{
    public KeyCode pressKeyCode;
    public Skill skillToExecute { private set; get; }
    public int mouseButton = -1;

    public float CooldownPrecent { get; private set; }
    public float Cooldown { get; private set; }

    bool charging = false;
    public Action<SkillSLot> OnSkillChanged;
    public void SetSkill(Skill skill)
    {
        this.skillToExecute = skill;
        OnSkillChanged?.Invoke(this);
    }
    public void CheckSkillInput()
    {
        if (skillToExecute == null) return;

        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
            CooldownPrecent = Cooldown / skillToExecute.CooldownTime;
            return;
        }

        Cooldown = 0;
        CooldownPrecent = 0;

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
        Cooldown = skillToExecute.CooldownTime;
        skillToExecute.ReleaseCharge();
        charging = false;
    }

    private void OnKeyDown()
    {
        if (skillToExecute.skilltype == ESkillType.Instant)
        {
            Cooldown = skillToExecute.CooldownTime;
            skillToExecute.Execute();
            return;
        }
        charging = true;
        skillToExecute.StartCharge();
    }
}