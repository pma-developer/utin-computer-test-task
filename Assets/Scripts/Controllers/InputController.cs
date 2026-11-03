using UnityEngine;
using UnityEngine.InputSystem;

namespace Controllers
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _shootActionReference;
        
        private InputAction _shootAction;
        public event System.Action ShootStarted;
        public event System.Action ShootCanceled;

        private void OnEnable()
        {
            _shootAction = _shootActionReference.action;
            _shootAction.Enable();
            
            _shootAction.started += OnShootStarted;
            _shootAction.canceled += OnShootCanceled;
        }

        private void OnDisable()
        {
            if (_shootAction != null)
            {
                _shootAction.started -= OnShootStarted;
                _shootAction.canceled -= OnShootCanceled;
                _shootAction.Disable();
            }
        }

        private void OnShootStarted(InputAction.CallbackContext context)
        {
            ShootStarted?.Invoke();
        }

        private void OnShootCanceled(InputAction.CallbackContext context)
        {
            ShootCanceled?.Invoke();
        }
    }
}
