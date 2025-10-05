using _Project.Scripts.Camera;
using StarterAssets;
using UnityEngine;

namespace _Project.Scripts.Player.Skills
{
    [CreateAssetMenu(fileName = "MovePlatform", menuName = "Player/Skill/Move Platform")]
    public class MovePlatform : Skill
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float range;
        private Platform platform;

        public override bool Execute()
        {
            var owner = SkillData.Owner;
            var platforms = Physics.OverlapSphere(owner.transform.position, range, layerMask);
            if (platforms.Length == 0)
                return false;

            platform = platforms[0].GetComponent<Platform>();
            var ownerInputReceiver = SkillData.Owner.GetComponent<MoveInputReceiver>();

            platform.TakeControl(ownerInputReceiver);
            
            // show UI 
            return true;
        }

        public override bool StartCharge()
        {
            return false;
        }

        public override bool UpdateCharge()
        {
            return true;
        }

        public override bool ReleaseCharge()
        {
            return false;
        }
    }
}