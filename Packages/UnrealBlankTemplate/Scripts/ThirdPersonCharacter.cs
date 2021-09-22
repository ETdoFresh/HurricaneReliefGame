using UnityEngine;

namespace UnrealBlankTemplate
{
    public class ThirdPersonCharacter : Character
    {
        private void InputAxisMoveForward()
        {
            var controlRotation = _controller.transform.eulerAngles;
            var quaternion = Quaternion.Euler(0, controlRotation.y, 0);
            var forwardVector = quaternion * Vector3.forward;
            _characterController.Move(forwardVector);
        }
        
        private void InputAxisMoveRight()
        {
            var controlRotation = _controller.transform.eulerAngles;
            var quaternion = Quaternion.Euler(0, controlRotation.y, 0);
            var rightVector = quaternion * Vector3.right;
            _characterController.Move(rightVector);
        }

        private void InputAxisTurn(float axisValue)
        {
            AddControllerYawInput(axisValue);
        }

        private void InputAxisLookUp(float axisValue)
        {
            AddControllerPitchInput(axisValue);
        }

        private void InputActionJump()
        {
            // If Press Jump
            // If Release StopJumping
        }

        private void InputAxisTurnRate(float axisValue)
        {
            var val = axisValue * baseTurnRate * Time.deltaTime;
            AddControllerYawInput(val);
        }
        
        private void InputAxisLookUpRate(float axisValue)
        {
            var val = axisValue * baseLookUpRate * Time.deltaTime;
            AddControllerPitchInput(val);
        }
    }
}