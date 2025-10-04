using System;
using _Project.Scripts.Character;
using UnityEngine;
using ESkillType = Skill.ESkillType;
using Object = UnityEngine.Object;

[System.Serializable]

public class SkillSlot
{
    [field: SerializeField] public Skill SkillToExecute { private set; get; }
    public float CooldownPrecent { get; private set; }
    public float Cooldown { get; private set; }
    
    private bool charging = false;
    public Action<SkillSlot> OnSkillChanged;

    private bool previousInputState = false;
    private bool inputState = false;
    private SkillsController skillsController;

    public void UpdateInputState(bool inputState)
    {
        this.inputState = inputState;
    }

    public void InitSkillsController(SkillsController skillsController)
    {
        this.skillsController = skillsController;
        if (SkillToExecute == null)
            return;
        SetSkill(SkillToExecute);
    }
    
    public void SetSkill(Skill skill)
    {
        SkillToExecute = Object.Instantiate(skill);
        SkillToExecute.InitSkillData(new SkillData()
        {
            Owner = skillsController,
        });
        OnSkillChanged?.Invoke(this);
    }
    public void CheckSkillInput()
    {
        if (SkillToExecute == null) return;

        if (Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
            CooldownPrecent = Cooldown / SkillToExecute.CooldownTime;
            return;
        }

        Cooldown = 0;
        CooldownPrecent = 0;

        if (previousInputState != inputState)
        {
            previousInputState = inputState;
            if (inputState)
            {
                OnKeyDown();
            }
            else
            {
                OnKeyUp();
            }
        }

        if (SkillToExecute.skilltype == ESkillType.Charge && charging)
        {
            SkillToExecute.UpdateCharge();
        }
    }

    private void OnKeyUp()
    {
        if (SkillToExecute.skilltype != ESkillType.Charge)
            return;
        Cooldown = SkillToExecute.CooldownTime;
        SkillToExecute.ReleaseCharge();
        charging = false;
    }

    private void OnKeyDown()
    {
        if (SkillToExecute.skilltype == ESkillType.Instant)
        {
            Cooldown = SkillToExecute.CooldownTime;
            SkillToExecute.Execute();
            return;
        }
        charging = true;
        SkillToExecute.StartCharge();
    }
}