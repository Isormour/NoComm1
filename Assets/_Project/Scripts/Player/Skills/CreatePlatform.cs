using UnityEngine;

namespace _Project.Scripts.Player.Skills
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Player/Skill/Platform", order = 1)]
    public class CreatePlatform : Skill
    {
        [SerializeField] private Platform platform;

        private Platform spawnedPlatform;

        public override void Execute()
        {
            throw new System.NotImplementedException();
        }

        public override void StartCharge()
        {
            spawnedPlatform = Instantiate(platform);
            spawnedPlatform.transform.position = CalculatedPoint();
            spawnedPlatform.SetSelected(true);
        }

        public override void UpdateCharge()
        {
            spawnedPlatform.transform.position = CalculatedPoint();
        }

        public override void ReleaseCharge()
        {
            spawnedPlatform.SetSelected(false);
            spawnedPlatform = null;
            SkillData.StatisticsHolder.ChangeAmountMana(-Cost);
        }


        private Vector3 CalculatedPoint()
        {
            var character = SkillData.Owner.transform;
            var position = character.position + character.forward * 2;
            return position;
        }
    }
}
