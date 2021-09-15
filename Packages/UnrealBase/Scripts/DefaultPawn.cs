using UnityEngine;
using UnityEngine.InputSystem;

namespace UnrealBase
{
    [RequireComponent(typeof(MovementComponent))]
    public class DefaultPawn : Pawn
    {
        [SerializeField] private float baseTurnRate = 45;
        [SerializeField] private float baseLookUpRate = 45;
        [SerializeField] private bool addDefaultMovementBindings = true;

        private MovementComponent _movementComponent;
        private InputAction _movementXZInputAction;
        private InputAction _movementYInputAction;
        [SerializeField] private InputAction _turnInputAction;
        private InputAction _lookInputAction;

        private void Awake()
        {
            _movementComponent = GetComponent<MovementComponent>();
            _movementXZInputAction = new InputAction("Movement XZ Input Action");
            _movementYInputAction = new InputAction("Movement Y Input Action");
            _turnInputAction = new InputAction("Turn Input Action");
            _lookInputAction = new InputAction("Look Input Action");
            AssignDefaultBindingsToInputAction();
        }

        private void OnEnable()
        {
            if (!addDefaultMovementBindings) return;
            _movementXZInputAction.Enable();
            _movementYInputAction.Enable();
            _turnInputAction.Enable();
            _lookInputAction.Enable();
        }

        private void OnDisable()
        {
            _movementXZInputAction.Disable();
            _movementYInputAction.Disable();
            _turnInputAction.Disable();
            _lookInputAction.Disable();
        }

        private void Update()
        {
            var inputXZ = _movementXZInputAction.ReadValue<Vector2>();
            var rotatedInput = RotateInputTowardsController(inputXZ);
            _movementComponent.Move(rotatedInput);
            var inputY = _movementYInputAction.ReadValue<float>();
            _movementComponent.MoveAddY(inputY);
            var turn = _turnInputAction.ReadValue<float>(); ;
            AddControllerYawInput(turn * baseTurnRate * Time.deltaTime);
            var look = _lookInputAction.ReadValue<float>();
            AddControllerPitchInput(-look * baseLookUpRate * Time.deltaTime);
        }

        private void AssignDefaultBindingsToInputAction()
        {
            _movementXZInputAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Down", "<Keyboard>/s")
                .With("Left", "<Keyboard>/a")
                .With("Right", "<Keyboard>/d");
            _movementYInputAction.AddCompositeBinding("Axis")
                .With("Positive", "<Keyboard>/e")
                .With("Negative", "<Keyboard>/q");
            _movementYInputAction.AddCompositeBinding("Axis")
                .With("Positive", "<Keyboard>/space");
            _turnInputAction.AddBinding("<Mouse>/delta/x");
            _lookInputAction.AddBinding("<Mouse>/delta/y");
        }
    }
}