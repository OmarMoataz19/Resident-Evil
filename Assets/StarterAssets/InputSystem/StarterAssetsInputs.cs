using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool shoot;
		public bool reload;
		public bool canMove = true;
        public bool canLook = true;
        public bool canJump = true;
        public bool canSprint = true;
        public bool canAim = true;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
public void OnMove(InputValue value)
        {
            if(canMove){
                MoveInput(value.Get<Vector2>());
            }
            else{
                MoveInput(new Vector2(0f,0f));
            }
        }

        public void OnLook(InputValue value)
        {
            if(cursorInputForLook && canLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            if(canJump){
                JumpInput(value.isPressed);
            }
        }

        public void OnSprint(InputValue value)
        {
            if(canSprint){
                SprintInput(value.isPressed);
            }
        }

        public void OnAim(InputValue value)
        {
            if(canAim){
                AimInput(value.isPressed);
            }
        }
		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}
		public void OnReload(InputValue value)
		{
			ReloadInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
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
		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}
		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}
		public void ReloadInput(bool newReloadState)
		{
			reload = newReloadState;
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