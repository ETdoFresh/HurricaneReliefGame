using Cinemachine;
using UnityEngine;

namespace UnrealBlankTemplate
{
    public class CinemachineSameAsLookAtTarget : CinemachineComponentBase
    {
        [Tooltip("How much time it takes for the aim to catch up to the target's rotation")]
        [SerializeField] private float damping;
        
        private Quaternion _previousReferenceOrientation = Quaternion.identity;
        
        public override bool IsValid => enabled && LookAtTarget != null;
        public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Aim;

        public override void MutateCameraState(ref CameraState curState, float deltaTime)
        {
            if (!IsValid) return;
            Quaternion dampedOrientation = LookAtTargetRotation;
            if (deltaTime >= 0)
            {
                var t = VirtualCamera.DetachedFollowTargetDamp(1, damping, deltaTime);
                dampedOrientation = Quaternion.Slerp(
                    _previousReferenceOrientation, LookAtTargetRotation, t);
            }
            _previousReferenceOrientation = dampedOrientation;
            curState.RawOrientation = dampedOrientation;
        }
    }
}