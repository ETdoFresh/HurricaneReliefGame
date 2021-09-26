using CodeExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using static CodeExtensions.ObjectExtension;
using Vector3 = UnityEngine.Vector3;

namespace UnrealBlankTemplate
{
    [RequireComponent(typeof(CharacterController))]
    public class Character : Pawn
    {
        [SerializeField] protected InputActionAsset inputActionAsset;
        [SerializeField] protected float baseTurnRate = 45;
        [SerializeField] protected float baseLookUpRate = 45;
        [SerializeField] protected bool addDefaultMovementBindings = true;

        [SerializeField] private float gravityScale = 1;
        [SerializeField] private float maxAcceleration = 20.48f;
        [SerializeField] private float brakingFrictionFactor = 2;
        [SerializeField] private float crouchedHalfHeight = .40f;
        [SerializeField] private float mass = 1;
        [SerializeField] private float maxWalkSpeed = 6;
        [SerializeField] private float maxCrouchSpeed = 3;
        [SerializeField] private float brakingDecelerationWalking = 20.48f;
        [SerializeField] private float jumpVelocity = 3;

        [SerializeField] private Vector3 rotationRate = new Vector3(0, 540, 0);
        [SerializeField] private bool useControllerDesiredRotation;
        [SerializeField] private bool orientRotationToMovement;

        [SerializeField] private Vector3 _movementInput;
        [SerializeField] private Vector3 _acceleration;
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private bool _jumpPressed;
        private CharacterController _characterController;

        private InputAction _moveForwardAction;
        private InputAction _moveRightAction;
        private InputAction _jumpAction;
        private InputAction _turnAction;
        private InputAction _lookUpAction;
        private InputAction _turnRateAction;
        private InputAction _lookUpRateAction;
        private Quaternion _targetRotation;

        public bool IsFalling => !_characterController.isGrounded;
        public Vector3 Velocity => _velocity;

        private void OnValidate()
        {
            inputActionAsset = FindIfNull(inputActionAsset, "ThirdPersonInputActions");
        }

        protected virtual void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (_moveForwardAction != null) InputAxisMoveForward(_moveForwardAction.ReadValue<float>());
            if (_moveRightAction != null) InputAxisMoveRight(_moveRightAction.ReadValue<float>());
            if (_turnAction != null) InputAxisTurn(_turnAction.ReadValue<float>());
            if (_lookUpAction != null) InputAxisLookUp(_lookUpAction.ReadValue<float>());
            if (_turnRateAction != null) InputAxisTurnRate(_turnRateAction.ReadValue<float>());
            if (_lookUpRateAction != null) InputAxisLookUpRate(_lookUpRateAction.ReadValue<float>());

            OrientRotationToMovement();
            
            if (_movementInput.sqrMagnitude > 1)
                _movementInput = _movementInput.normalized;

            if (_movementInput != Vector3.zero)
            {
                _acceleration = _movementInput * maxAcceleration;
                _velocity += _acceleration * Time.deltaTime;
            }
            else if (_velocity.x != 0 || _velocity.z != 0)
            {
                _acceleration = -_velocity.GetX0Z();
                var brakingDeltaVelocity = brakingDecelerationWalking * Time.deltaTime;
                if (_acceleration.magnitude > brakingDeltaVelocity)
                    _acceleration = _acceleration.normalized * brakingDeltaVelocity;

                _velocity += _acceleration;
            }
            else
                _acceleration = Vector3.zero;

            _velocity = _velocity.ClampXZMagnitude(maxWalkSpeed);

            if (!_characterController.isGrounded)
                _velocity.y += Physics.gravity.y * Time.deltaTime * gravityScale;
            else
                _velocity.y = -_characterController.stepOffset * gravityScale;

            if (_jumpPressed)
                _velocity.y = jumpVelocity;

            _characterController.Move(maxWalkSpeed * Time.deltaTime * _velocity);
            _movementInput = Vector3.zero;
            _jumpPressed = false;
        }

        private void OrientRotationToMovement()
        {
            if (!orientRotationToMovement) return;
            _targetRotation = _movementInput.sqrMagnitude > 0 ? Quaternion.LookRotation(_movementInput) : transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationRate.y * Time.deltaTime);
        }

        private void OnEnable()
        {
            AssignInputActions();
            if (_jumpAction != null) _jumpAction.performed += OnJumpAction;
        }

        private void OnDisable()
        {
            if (_jumpAction != null) _jumpAction.performed -= OnJumpAction;
        }

        protected void AddMovementInput(Vector3 worldDirection, float scaleValue, bool force = false)
        {
            _movementInput += worldDirection * scaleValue;
        }

        protected void Jump()
        {
            _jumpPressed = true;
        }

        protected void StopJumping()
        {
            _jumpPressed = false;
        }
        
        private void AssignInputActions()
        {
            if (!inputActionAsset) return;
            _moveForwardAction ??= inputActionAsset.FindAction("Gameplay/MoveForward");
            _moveRightAction ??= inputActionAsset.FindAction("Gameplay/MoveRight");
            _turnAction ??= inputActionAsset.FindAction("Gameplay/Turn");
            _lookUpAction ??= inputActionAsset.FindAction("Gameplay/LookUp");
            _jumpAction ??= inputActionAsset.FindAction("Gameplay/Jump");
            _turnRateAction ??= inputActionAsset.FindAction("Gameplay/TurnRate");
            _lookUpRateAction ??= inputActionAsset.FindAction("Gameplay/LookUpRate");
        }

        private void OnJumpAction(InputAction.CallbackContext obj)
        {
            if (obj.phase == InputActionPhase.Performed)
                InputActionJumpPressed();
            else
                InputActionJumpReleased();
        }

        protected virtual void InputAxisMoveForward(float axisValue)
        {
        }

        protected virtual void InputAxisMoveRight(float axisValue)
        {
        }

        protected virtual void InputAxisTurn(float axisValue)
        {
        }

        protected virtual void InputAxisLookUp(float axisValue)
        {
        }

        protected virtual void InputActionJumpPressed()
        {
        }

        protected virtual void InputActionJumpReleased()
        {
        }

        protected virtual void InputAxisTurnRate(float axisValue)
        {
        }

        protected virtual void InputAxisLookUpRate(float axisValue)
        {
        }
    }
}