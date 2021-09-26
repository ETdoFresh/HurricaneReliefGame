using Cinemachine;
using CodeExtensions;
using UnityEngine;

namespace UnrealBlankTemplate
{
    public class PlayerController : Controller
    {
        [SerializeField] private bool autoManageActiveCameraTarget = true;
        [SerializeField] private float inputYawScale = 2.5f;
        [SerializeField] private float inputPitchScale = -2.5f;
        [SerializeField] private float inputRollScale = 1f;
        
        private CinemachineVirtualCamera _playerCameraManager;

        private int _controllerId;
        private float _pitch;
        private float _yaw;

        public int MyPlayerIndex => _controllerId;

        protected override void Awake()
        {
            base.Awake();
            SetInputModeGameOnly();
            _pitch = transform.eulerAngles.x;
            _yaw = transform.eulerAngles.y;
        }

        public override void Possess(Pawn newPawn)
        {
            base.Possess(newPawn);
            var pawnCamera = (Component)pawn.GetComponentInChildren<CinemachineVirtualCamera>();
            if (!pawnCamera) pawnCamera = pawn.GetComponentInChildren<Camera>();
            var pawnCameraTransform = pawnCamera ? pawnCamera.transform : null;
            _playerCameraManager.Follow = pawnCamera ? pawnCameraTransform : pawn.transform;
            _playerCameraManager.LookAt = pawnCamera ? pawnCameraTransform : transform;
            transform.rotation = pawn.transform.rotation;
            _pitch = transform.eulerAngles.x;
            _yaw = transform.eulerAngles.y;
        }

        public void AddPitchInput(float val)
        {
            SetPitch(_pitch + val / inputPitchScale);
            var myTransform = transform;
            myTransform.eulerAngles = myTransform.eulerAngles.SetX(_pitch);
        }

        public void AddRollInput(float val)
        {
            var myTransform = transform;
            myTransform.eulerAngles = myTransform.eulerAngles.SetZ(val);
        }

        public void AddYawInput(float val)
        {
            SetYaw(_yaw + val / inputYawScale);
            var myTransform = transform;
            myTransform.eulerAngles = myTransform.eulerAngles.SetY(_yaw);
        }

        public void AssignPlayerCameraManager(CinemachineVirtualCamera playerCameraManager)
        {
            _playerCameraManager = playerCameraManager;
        }

        private void SetPitch(float val)
        {
            val = Mathf.Clamp(val, -90f, 90f);
            _pitch = val;
        }

        private void SetYaw(float val)
        {
            while (val < 0f) val += 360f;
            while (val >= 360f) val -= 360f;
            _yaw = val;
        }

        public void SetInputModeGameOnly()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void SetInputModeUIOnly()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        public void SetPlayerControllerID(int controllerId)
        {
            _controllerId = controllerId;
        }
    }
}
