using UnityEngine;
using UnityEngine.InputSystem;

namespace SimpleRigidbodyMovement
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyMovement : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputActionAsset;
        [SerializeField] private float movementForce = 20;
        [SerializeField] private float jumpForce = 8;
        [SerializeField] private bool enableMovement = true;
        [SerializeField] private bool enableJump = true;
        private Rigidbody _rigidbody;
        private bool _jumpQueued;
        private void OnValidate()
        {
            inputActionAsset = CodeExtensions.ScriptableObject.FindIfNull(inputActionAsset, "SimpleRigidbodyControls");
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            if (!inputActionAsset) return;
            inputActionAsset.FindActionMap("Gameplay").FindAction("Jump").performed += OnJump;
            inputActionAsset.Enable();
        }

        private void OnDisable()
        {
            if (!inputActionAsset) return;
            inputActionAsset.FindActionMap("Gameplay").FindAction("Jump").performed -= OnJump;
            inputActionAsset.Disable();
        }

        private void FixedUpdate()
        {
            if (!inputActionAsset) return;
            ApplyMovement();
            ApplyJump();
        }

        private void ApplyMovement()
        {
            if (!enableMovement) return;
            var input = inputActionAsset.FindActionMap("Gameplay").FindAction("Movement").ReadValue<Vector2>();
            var movementDirection = new Vector3(input.x, 0, input.y);
            _rigidbody.AddForce(movementForce * movementDirection);
        }

        private void ApplyJump()
        {
            if (!enableJump) return;
            if (!_jumpQueued) return;
            _jumpQueued = false;
            _rigidbody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
        }

        private void OnJump(InputAction.CallbackContext obj)
        {
            if (Time.timeScale > 0)
                _jumpQueued = true;
        }
    }
}
