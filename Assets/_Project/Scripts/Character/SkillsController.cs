using System;
using StarterAssets;
using UnityEngine;

namespace _Project.Scripts.Character
{
    public class SkillsController : MonoBehaviour
    {
        [SerializeField] private SkillSlot attack1;
        [SerializeField] private SkillSlot attack2;
        [SerializeField] private SkillSlot[] skillSlots;

        private MoveInputReceiver inputReceiver;
        private StatisticsHolder statisticsHolder;

        private void Awake()
        {
            statisticsHolder = GetComponent<StatisticsHolder>();
            inputReceiver =  GetComponent<MoveInputReceiver>();
        }

        private void Update()
        {
            SetAttack1(inputReceiver.isPressedAttack1);
            SetAttack2(inputReceiver.isPressedAttack2);
            SetSkillInput(inputReceiver.isPressedSkill1, 0);
            SetSkillInput(inputReceiver.isPressedSkill2, 1);
            SetSkillInput(inputReceiver.isPressedSkill3, 2);
            SetSkillInput(inputReceiver.isPressedSkill4, 3);
        }

        private void SetAttack1(bool value)
        {
            attack1.UpdateInputState(value);
        }

        private void SetAttack2(bool value)
        {
            attack2.UpdateInputState(value);
        }
        private void SetSkillInput(bool value, int skillId)
        {
            if (skillId >= skillSlots.Length)
                return;
            var skill = skillSlots[skillId];
            skill.UpdateInputState(value);   
        }

        private void CheckSkillsInput()
        {
            attack1.CheckSkillInput();
            attack2.CheckSkillInput();
            for (int i = 0; i < skillSlots.Length; i++)
            {
                skillSlots[i].CheckSkillInput();
            }
        }
    }
}
