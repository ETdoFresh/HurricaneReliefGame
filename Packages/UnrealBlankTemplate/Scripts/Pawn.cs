using UnityEngine;

namespace UnrealBlankTemplate
{
    public class Pawn : MonoBehaviour
    {
        public AutoPossessPlayerEnum autoPossessPlayer = AutoPossessPlayerEnum.Disabled;

        protected Controller _controller;

        public void AddControllerPitchInput(float val)
        {
            if (!_controller) return;
            if (_controller is PlayerController playerController)
                playerController.AddPitchInput(val);
        }

        public void AddControllerRollInput(float val)
        {
            if (!_controller) return;
            if (_controller is PlayerController playerController)
                playerController.AddRollInput(val);
        }

        public void AddControllerYawInput(float val)
        {
            if (!_controller) return;
            if (_controller is PlayerController playerController)
                playerController.AddYawInput(val);
        }

        internal void SetController(Controller controller)
        {
            _controller = controller;
        }

        protected Vector3 RotateInputTowardsController(Vector2 inputXZ)
        {
            if (!_controller) return inputXZ;
            var forward = _controller.transform.forward;
            var right = _controller.transform.right;
            forward.Normalize();
            right.Normalize();
            return forward * inputXZ.y + right * inputXZ.x;
        }

        public Controller GetController()
        {
            return _controller;
        }
    }
}