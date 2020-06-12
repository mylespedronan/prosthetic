using System.Collections;
using System.Collections.Generic;
using Prosthetic.Scripts;
using Prosthetic.Scripts.MonoBehaviours.Commands;
using UnityEngine;
using UnityEngine.Experimental.Input;

namespace Prosthetic.Entity.Scripts.Monobehaviours
{
    public class CharacterInput : MonoBehaviour, IInteractInput, IRotationInput, IMoveInput
    {
        public Command interactInput;
        public Command movementInput;
        public Command rotationInput;
        
        private Movement _movement;

        public bool IsPressingInteract { get; private set; }
        public Vector3 RotationDirection { get; set; }
        public Vector3 MoveDirection { get; private set; }

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            _movement = new Movement();
        }

        // This function is called when the object becomes enabled and active
        private void OnEnable()
        {
            _movement.Enable();

            if (movementInput)
                _movement.Player.Movement.performed += OnMoveInput;

            _movement.Player.Interact.performed += OnInteractButton;
            
            if (rotationInput)
                _movement.Player.Movement.performed += OnRotationInput;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            MoveDirection = new Vector3(value.x, 0, value.y);

            if (movementInput != null)
                movementInput.Execute();
        }

        private void OnRotationInput(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            RotationDirection = new Vector3(value.x, 0, value.y);
            if (rotationInput != null)
                rotationInput.Execute();
        }

        private void OnInteractButton(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            IsPressingInteract = value >= 0.15;
            if(interactInput != null && IsPressingInteract)
                interactInput.Execute();
        }

        // This function is called when the behaviour becomes disabled or inactive
        private void OnDisable()
        {
            _movement.Player.Interact.performed -= OnInteractButton;
            _movement.Disable();
        }
        
    }
}
