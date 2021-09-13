using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleKartMovement
{
    public class KartController : MonoBehaviour
    {
        private const float k_DeadZone = 0.01f;

        [SerializeField] private WheelCollider frontLeftWheel;
        [SerializeField] private WheelCollider frontRightWheel;
        [SerializeField] private WheelCollider rearLeftWheel;
        [SerializeField] private WheelCollider rearRightWheel;
        [SerializeField] private Transform centerOfMass;
        [SerializeField] private float maxForwardSpeed = 15;
        [SerializeField] private float maxBackwardSpeed = 10;
        [SerializeField] private float forwardAccelerationPower = 7;
        [SerializeField] private float backwardAccelerationPower = 3;
        [SerializeField] private float decelerationPower = 16;
        [SerializeField] private float accelerationCurve = 0.5f;
        [SerializeField] private float turnPower = 4;
        [SerializeField] private float driftTurnPower = 0.97f;
        [SerializeField] private float coastingDrag = 5;
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private float airborneReorientationCoefficient = 1;
        [SerializeField] private float airborneGravityMultiplier = 1;
        [SerializeField] private float hopForce = 5f;
        private float _groundedWheelsPercent;
        private float _airWheelsPercent;
        private Rigidbody _rigidbody;
        private bool _canMove;
        private bool _isDrifting;
        private bool _isInAir;
        private bool _hasCollision;
        private Vector3 _lastCollisionNormal;
        private Vector3 _verticalReference;
        private bool _jumpRequested;
        [SerializeField] private InputActionAsset inputActionAsset;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _canMove = true;
        }

        private void OnEnable()
        {
            if (!inputActionAsset) return;
            inputActionAsset.FindAction("Gameplay/Jump").performed += Jump;
            inputActionAsset.Enable();
        }

        private void OnDisable()
        {
            if (!inputActionAsset) return;
            inputActionAsset.FindAction("Gameplay/Jump").performed -= Jump;
            inputActionAsset.Disable();
        }

        private void FixedUpdate()
        {
            ApplyPowerUps();
            ApplyCenterOfMass();
            ApplyGroundCheck();
            ApplyMovement();
            ApplyExtraGravityToAirborne();
            ApplyHop();
        }

        private void OnCollisionEnter(Collision other)
        {
            _hasCollision = true;
        }

        private void OnCollisionExit(Collision other)
        {
            _hasCollision = false;
        }

        private void OnCollisionStay(Collision other)
        {
            _hasCollision = true;
            _lastCollisionNormal = Vector3.zero;
            const float dot = -1f;
            foreach (var contact in other.contacts)
                if (Vector3.Dot(contact.normal, Vector3.up) > dot)
                    _lastCollisionNormal = contact.normal;
        }

        private void Jump(InputAction.CallbackContext obj)
        {
            _jumpRequested = true;
        }

        private void ApplyPowerUps()
        {
        }

        private void ApplyCenterOfMass()
        {
            _rigidbody.centerOfMass = transform.InverseTransformPoint(centerOfMass.position);
        }

        private void ApplyGroundCheck()
        {
            var groundedWheelsCount = 0;
            if (frontLeftWheel.isGrounded && frontLeftWheel.GetGroundHit(out _))
                groundedWheelsCount++;
            if (frontRightWheel.isGrounded && frontRightWheel.GetGroundHit(out _))
                groundedWheelsCount++;
            if (rearLeftWheel.isGrounded && rearLeftWheel.GetGroundHit(out _))
                groundedWheelsCount++;
            if (rearRightWheel.isGrounded && rearRightWheel.GetGroundHit(out _))
                groundedWheelsCount++;

            _groundedWheelsPercent = groundedWheelsCount / 4f;
            _airWheelsPercent = 1 - _groundedWheelsPercent;
        }

        private void ApplyMovement()
        {
            if (!_canMove) return;

            var movementInput = inputActionAsset.FindAction("Gameplay/Movement").ReadValue<Vector2>();
            var accelerate = movementInput.y > 0f;
            var brake = movementInput.y < 0f;
            var turn = movementInput.x;
            //var drift = Input.GetButton("Jump");

            var accelerationInput = (accelerate ? 1f : 0f) - (brake ? 1f : 0f);
            var localVelocity = transform.InverseTransformVector(_rigidbody.velocity);
            var isAccelerationForward = accelerationInput >= 0;
            var isLocalVelocityForward = localVelocity.z >= 0;
            var maxSpeed = isLocalVelocityForward ? maxForwardSpeed : maxBackwardSpeed;
            var accelerationPower = isAccelerationForward ? forwardAccelerationPower : backwardAccelerationPower;
            var currentSpeed = _rigidbody.velocity.magnitude;
            var accelerationRampT = currentSpeed / maxSpeed;
            var scaledAccelerationCurve = 5 * accelerationCurve;
            var accelerationRamp = Mathf.Lerp(scaledAccelerationCurve, 1, accelerationRampT * accelerationRampT);
            var isBraking = (isLocalVelocityForward && brake) || (!isLocalVelocityForward && accelerate);
            accelerationPower = isBraking ? decelerationPower : accelerationPower;
            var acceleration = accelerationPower * accelerationRamp;
            var turningPower = _isDrifting ? driftTurnPower : turn * turnPower;
            var turnAngle = Quaternion.AngleAxis(turningPower, transform.up);
            var forward = turnAngle * transform.forward;
            var isGrounded = _groundedWheelsPercent > 0f;
            var groundFactor = isGrounded ? 1f : 0f;
            var movement = accelerationInput * acceleration * groundFactor * forward;
            var wasOverMaxSpeed = currentSpeed >= maxSpeed;

            if (wasOverMaxSpeed && !isBraking)
                movement *= 0f;

            var newVelocity = _rigidbody.velocity + movement * Time.fixedDeltaTime;
            newVelocity.y = _rigidbody.velocity.y;

            if (isGrounded && !wasOverMaxSpeed)
                newVelocity = Vector3.ClampMagnitude(newVelocity, maxSpeed);

            if (isGrounded && Mathf.Abs(accelerationInput) < k_DeadZone)
                newVelocity = Vector3.MoveTowards(newVelocity, new Vector3(0, _rigidbody.velocity.y, 0),
                    Time.fixedDeltaTime * coastingDrag);

            _rigidbody.velocity = newVelocity;

            if (isGrounded)
            {
                if (_isInAir)
                {
                    _isInAir = false;
                    // Spawn Jump Effects
                }

                var angularVelocitySteering = 0.4f;
                var angularVelocitySmoothSpeed = 20f;
                var reverseTurning = !isLocalVelocityForward && !isAccelerationForward;
                if (reverseTurning)
                    angularVelocitySteering *= -1f;
                var angularVelocity = _rigidbody.angularVelocity;
                angularVelocity.y = Mathf.MoveTowards(angularVelocity.y, turningPower * angularVelocitySteering,
                    Time.fixedDeltaTime * angularVelocitySmoothSpeed);
                _rigidbody.angularVelocity = angularVelocity;
                var velocitySteering = 25f;
            }

            var validPosition = false;
            if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out var hit, 3f,
                groundLayers))
            {
                var lerpVector = (_hasCollision && _lastCollisionNormal.y > hit.normal.y)
                    ? _lastCollisionNormal
                    : hit.normal;
                _verticalReference = Vector3.Slerp(_verticalReference, lerpVector,
                    Mathf.Clamp01(airborneReorientationCoefficient * Time.fixedDeltaTime *
                                  (_groundedWheelsPercent > 0.0f ? 10.0f : 1.0f)));
            }
            else
            {
                var lerpVector = (_hasCollision && _lastCollisionNormal.y > 0f) ? _lastCollisionNormal : Vector3.up;
                _verticalReference = Vector3.Slerp(_verticalReference, lerpVector,
                    Mathf.Clamp01(airborneReorientationCoefficient * Time.fixedDeltaTime));
            }

            validPosition = _groundedWheelsPercent > 0.7f && !_hasCollision &&
                            Vector3.Dot(_verticalReference, Vector3.up) > 0.9f;

            if (_groundedWheelsPercent < 0.7f)
            {
                _rigidbody.angularVelocity = new Vector3(0f, _rigidbody.angularVelocity.y * 0.98f, 0);
                var finalOrientationDirection = Vector3.ProjectOnPlane(transform.forward, _verticalReference);
                if (finalOrientationDirection.sqrMagnitude > 0f)
                    _rigidbody.MoveRotation(Quaternion.Lerp(_rigidbody.rotation,
                        Quaternion.LookRotation(finalOrientationDirection, _verticalReference),
                        Mathf.Clamp01(airborneReorientationCoefficient * Time.fixedDeltaTime)));
            }
            else if (validPosition)
            {
                // _lastValidPosition = transform.position;
                // _lastValidRotation.eulerAngles = new Vector3(0f, transform.rotation.y, 0f);
            }
        }

        private void ApplyExtraGravityToAirborne()
        {
            if (_airWheelsPercent >= 1)
                _rigidbody.velocity += Time.fixedDeltaTime * airborneGravityMultiplier * Physics.gravity;
        }

        private void ApplyHop()
        {
            if (!_jumpRequested) return;
            _rigidbody.velocity += hopForce * transform.up; 
            _jumpRequested = false;
        }
    }
}