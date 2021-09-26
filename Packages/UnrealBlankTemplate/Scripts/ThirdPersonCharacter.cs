using UnityEngine;

namespace UnrealBlankTemplate
{
    public class ThirdPersonCharacter : Character
    {
        protected override void InputAxisMoveForward(float axisValue)
        {
            var controlRotation = _controller.transform.eulerAngles;
            var quaternion = Quaternion.Euler(0, controlRotation.y, 0);
            var forwardVector = quaternion * Vector3.forward;
            AddMovementInput(forwardVector, axisValue);
        }

        protected override void InputAxisMoveRight(float axisValue)
        {
            var controlRotation = _controller.transform.eulerAngles;
            var quaternion = Quaternion.Euler(0, controlRotation.y, 0);
            var rightVector = quaternion * Vector3.right;
            AddMovementInput(rightVector, axisValue);
        }

        protected override void InputAxisTurn(float axisValue)
        {
            AddControllerYawInput(axisValue);
        }

        protected override void InputAxisLookUp(float axisValue)
        {
            AddControllerPitchInput(axisValue);
        }

        protected override void InputActionJumpPressed()
        {
            Jump();
        }

        protected override void InputActionJumpReleased()
        {
            StopJumping();
        }

        protected override void InputAxisTurnRate(float axisValue)
        {
            var val = axisValue * baseTurnRate * Time.deltaTime;
            AddControllerYawInput(val);
        }

        protected override void InputAxisLookUpRate(float axisValue)
        {
            var val = axisValue * baseLookUpRate * Time.deltaTime;
            AddControllerPitchInput(val);
        }
    }
}