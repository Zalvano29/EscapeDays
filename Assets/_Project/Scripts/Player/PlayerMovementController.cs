using UnityEngine;
using EscapeDays.Core;

namespace EscapeDays.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;

        [Tooltip("Wajib ditarik via Inspector, dilarang menggunakan GetComponent di Update")]
        [SerializeField] private Rigidbody2D _rb;

        private Vector2 _moveInput;

        private void Update()
        {
            // 1. Fase Membaca Data: Berjalan setiap frame.
            // Kita mengambil nilai dari InputManager (Decoupling)
            if (InputManager.HasInstance)
            {
                _moveInput = InputManager.Instance.MoveInput;
            }
        }

        private void FixedUpdate()
        {
            // 2. Fase Eksekusi Fisika: Berjalan pada fixed timestep yang stabil.
            // Memanipulasi kecepatan secara langsung untuk pergerakan top-down yang responsif
            _rb.linearVelocity = _moveInput * _moveSpeed;
        }
    }
}