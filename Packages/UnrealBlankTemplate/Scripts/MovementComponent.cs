using CodeExtensions;
using UnityEngine;

namespace UnrealBlankTemplate
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementComponent : MonoBehaviour
    {
        [Header("Floating Pawn Movement")]
        [SerializeField] private float maxSpeed = 12;

        [SerializeField] private float acceleration = 40;
        [SerializeField] private float deceleration = 80;
        [SerializeField] private float turningBoost = 0;

        [Header("Velocity")]
        [SerializeField] private Vector3 velocity;

        private Vector3 _input;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            velocity = _rigidbody.velocity;
            if (IsNoInputAndWithinStoppingDistance())
                SetVelocityToZero();
            else if (IsNoInputAndMoving())
                DecelerateToZero();
            else if (IsInputAndVelocityInSameDirection())
                AccelerateTowardsInput();
            else
                DecelerateTowardsInput();

            velocity = velocity.ClampMagnitude(maxSpeed);
            _rigidbody.velocity = velocity;
        }

        private bool IsNoInputAndWithinStoppingDistance()
        {
            return _input == Vector3.zero && velocity.magnitude < deceleration * Time.deltaTime;
        }

        private void SetVelocityToZero()
        {
            velocity = Vector3.zero;
        }

        private bool IsNoInputAndMoving()
        {
            return _input == Vector3.zero && velocity != Vector3.zero;
        }

        private void DecelerateToZero()
        {
            velocity = deceleration * Time.deltaTime * -velocity.normalized;
        }

        private bool IsInputAndVelocityInSameDirection()
        {
            return Vector3.Dot(velocity, _input) > 0;
        }

        private void AccelerateTowardsInput()
        {
            velocity += acceleration * Time.deltaTime * _input;
        }

        private void DecelerateTowardsInput()
        {
            velocity += deceleration * Time.deltaTime * _input;
        }

        public void Move(Vector3 input)
        {
            _input = input;
        }

        public void MoveAddY(float input)
        {
            _input = _input.AddY(input);
            _input = _input.normalized;
        }

        private bool SameDirection(Vector2 a, Vector2 b) // Within +/- 90 degrees
        {
            return Vector2.Dot(a, b) > 0;
        }

        private bool OppositeDirection(Vector2 a, Vector2 b) // Beyond +/- 90 degrees
        {
            return Vector2.Dot(a, b) < 0;
        }

        private bool Perpendicular(Vector2 a, Vector2 b) // At 90 degrees
        {
            return Vector2.Dot(a, b) == 0;
        }
    }
}