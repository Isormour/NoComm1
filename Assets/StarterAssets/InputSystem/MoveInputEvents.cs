using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveInputEvents : MonoBehaviour
{
    [SerializeField] private MoveInputReceiver receiver;

#if ENABLE_INPUT_SYSTEM
    public void OnMove(InputValue value)
    {
        receiver.MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        receiver.LookInput(value.Get<Vector2>());
    }

    public void OnJump(InputValue value)
    {
        receiver.JumpInput(value.isPressed);
    }

    public void OnSprint(InputValue value)
    {
        receiver.SprintInput(value.isPressed);
    }
#endif

    public void SetMoveInputReceiver(MoveInputReceiver mir)
    {
        receiver = mir;
    }
}