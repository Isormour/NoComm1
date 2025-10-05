using UnityEngine;

namespace _Project.Scripts.Player.Skills
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/Platform", order = 1)]
    public class CreatePlatform : Skill
    {
        [SerializeField] private Platform platform;

        private Platform spawnedPlatform;

        public override bool Execute()
        {
            throw new System.NotImplementedException();
        }

        public override bool StartCharge()
        {
            spawnedPlatform = Instantiate(platform);
            spawnedPlatform.transform.position = CalculatedPoint();
            //spawnedPlatform.SetSelected(true);
            return true;
        }

        public override bool UpdateCharge()
        {
            spawnedPlatform.transform.position = CalculatedPoint();
            return true;
        }

        public override bool ReleaseCharge()
        {
            spawnedPlatform.InitPLatform();
            //spawnedPlatform.SetSelected(false);
            spawnedPlatform = null;
            SkillData.StatisticsHolder.ChangeAmountMana(-Cost);
            return true;
        }


        private Vector3 CalculatedPoint()
        {
            var character = SkillData.Owner.transform;
            var position = character.position + character.forward * 2;
            return position;
        }
    }
}
