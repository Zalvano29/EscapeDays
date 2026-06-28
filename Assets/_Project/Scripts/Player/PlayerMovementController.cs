using UnityEngine;
using UnityEngine.InputSystem; // WAJIB ditambahkan untuk sistem baru

namespace EscapeDays.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Pengaturan Pergerakan")]
        [SerializeField] private float _moveSpeed = 5f;

        [Header("Referensi Sistem")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;

        private Vector2 _movementInput;

        private void Awake()
        {
            if (_rb == null) _rb = GetComponent<Rigidbody2D>();
            if (_animator == null) _animator = GetComponentInChildren<Animator>(); 
        }

        private void Update()
        {
            // 1. Membaca input menggunakan Input System Baru
            _movementInput = Vector2.zero;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) _movementInput.y += 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) _movementInput.y -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) _movementInput.x += 1f;
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) _movementInput.x -= 1f;
            }

            // 2. KIRIM DATA KE ANIMATOR
            if (_animator != null)
            {
                _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
            }
        }

        private void FixedUpdate()
        {
            // 3. Eksekusi pergerakan fisik karakter
            _rb.linearVelocity = _movementInput.normalized * _moveSpeed;
        }
    }
}