using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Core
{
    /// <summary>
    /// Single entry point for all player inputs.
    /// Reads from the generated PlayerInputActions class.
    /// </summary>
    public class InputManager : SingletonMonoBehaviour<InputManager>
    {
        private PlayerInputActions _actions;

        // Properti publik (Read-Only) yang akan diakses oleh sistem lain
        public Vector2 MoveInput { get; private set; }
        public bool IsSprintHeld { get; private set; }
        public bool IsCrouchHeld { get; private set; }

        protected override void Awake()
        {
            // Wajib memanggil base.Awake() agar logika Singleton dari parent dieksekusi
            base.Awake();

            _actions = new PlayerInputActions();
            _actions.Enable();
        }

        private void Update()
        {
            // Membaca nilai input setiap frame dan menyimpannya di properti publik
            MoveInput = _actions.Player.Move.ReadValue<Vector2>();
            IsSprintHeld = _actions.Player.Sprint.IsPressed();
            IsCrouchHeld = _actions.Player.Crouch.IsPressed();
        }

        private void OnDestroy()
        {
            // Mencegah memory leak saat objek hancur atau pindah scene
            _actions?.Disable();
        }
    }
}