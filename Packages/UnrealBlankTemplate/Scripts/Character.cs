using UnityEngine;

namespace UnrealBlankTemplate
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : Pawn
    {
        [SerializeField] protected float baseTurnRate = 45;
        [SerializeField] protected float baseLookUpRate = 45;
        [SerializeField] protected bool addDefaultMovementBindings = true;
        
        [SerializeField] private float gravityScale = 1;
        [SerializeField] private float maxAcceleration = 2048;
        [SerializeField] private float brakingFrictionFactor = 2;
        [SerializeField] private float crouchedHalfHeight = .40f;
        [SerializeField] private float mass = 1;
        [SerializeField] private float maxWalkSpeed = 6;
        [SerializeField] private float maxCrouchSpeed = 3;
        [SerializeField] private float brakingDecelerationWalking = 2.048f;

        protected CharacterController _characterController;

        protected virtual void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }
    }
}