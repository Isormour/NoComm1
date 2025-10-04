using System;
using StarterAssets;
using UnityEditor.Embree;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveInputEvents : MonoBehaviour
{
    public static MoveInputEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void OnAttack(InputValue value)
    {
        receiver.SetPressedAttack1(value.isPressed);
    }

    public void OnAttack2(InputValue value)
    {
        receiver.SetPressedAttack2(value.isPressed);
    }

    public void OnSkill1(InputValue value)
    {
        receiver.SetPressedSkill1(value.isPressed);
    }

    public void OnSkill2(InputValue value)
    {
        receiver.SetPressedSkill2(value.isPressed);
    }

    public void OnSkill3(InputValue value)
    {
        receiver.SetPressedSkill3(value.isPressed);
    }

    public void OnSkill4(InputValue value)
    {
        receiver.SetPressedSkill4(value.isPressed);
    }

    public void OnShield(InputValue value)
    {
        Debug.Log(value.isPressed);
        receiver.SetPressedShield(value.isPressed);
    }

    public void OnInterract(InputValue value)
    {
        receiver.SetPressedInterract(value.isPressed);
    }
#endif

    public void SetMoveInputReceiver(MoveInputReceiver mir)
    {
        receiver = mir;
    }
}