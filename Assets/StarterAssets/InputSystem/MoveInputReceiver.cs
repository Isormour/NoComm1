using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class MoveInputReceiver : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public bool isPressedAttack1 = false;
		public bool isPressedAttack2 = false;
		public bool isPressedSkill1 = false;
		public bool isPressedSkill2 = false;
		public bool isPressedSkill3 = false;
		public bool isPressedSkill4 = false;
		public bool isPressedInterract = false;
		public bool isPressedShield = false;

		public void ResetAllStates()
		{
			move = Vector2.zero;
			look = Vector2.zero;
			jump = false;
			sprint = false;
			analogMovement = false;
			cursorLocked = true;
			cursorInputForLook = true;
			isPressedAttack1  = false;
			isPressedAttack2  = false;
			isPressedSkill1 = false;
			isPressedSkill2 = false;
			isPressedSkill3 = false;
			isPressedSkill4 = false;
		}
		
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			if(cursorInputForLook)
				look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void SetPressedAttack1(bool value)
		{
			isPressedAttack1  = value;
		}

		public void SetPressedAttack2(bool value)
		{
			isPressedAttack2 = value;
		}

		public void SetPressedSkill1(bool value)
		{
			isPressedSkill1 = value;
		}

		public void SetPressedSkill2(bool value)
		{
			isPressedSkill2 = value;
		}

		public void SetPressedSkill3(bool value)
		{
			isPressedSkill3 = value;
		}

		public void SetPressedSkill4(bool value)
		{
			isPressedSkill4 = value;
		}

		public void SetPressedInterract(bool value)
		{
			isPressedInterract = value;
		}

		public void SetPressedShield(bool value)
		{
			isPressedShield = value;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}