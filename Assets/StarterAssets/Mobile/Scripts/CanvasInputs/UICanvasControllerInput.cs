using UnityEngine;
using UnityEngine.Serialization;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {

        [FormerlySerializedAs("starterAssetsInputs")] [Header("Output")]
        public MoveInputReceiver moveInputReceiver;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            moveInputReceiver.MoveInput(virtualMoveDirection);
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            moveInputReceiver.LookInput(virtualLookDirection);
        }

        public void VirtualJumpInput(bool virtualJumpState)
        {
            moveInputReceiver.JumpInput(virtualJumpState);
        }

        public void VirtualSprintInput(bool virtualSprintState)
        {
            moveInputReceiver.SprintInput(virtualSprintState);
        }
        
    }

}
