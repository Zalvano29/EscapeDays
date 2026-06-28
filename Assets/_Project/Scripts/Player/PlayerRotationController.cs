using UnityEngine;
using UnityEngine.InputSystem; // Wajib untuk New Input System

namespace EscapeDays.Player
{
    [RequireComponent(typeof(PlayerInput))] // Memastikan objek punya komponen input
    public class PlayerRotationController : MonoBehaviour
    {
        [Header("Aiming Settings")]
        [Tooltip("Wajib ditarik via Inspector")]
        [SerializeField] private Camera _mainCamera;

        [Tooltip("Container visual karakter (agar visual bisa diputar tanpa memutar Collider utama)")]
        [SerializeField] private Transform _visualContainer;

        private Vector2 _mouseScreenPosition;

        private void Awake()
        {
            // Memastikan referensi kamera terisi jika lupa menariknya di Inspector
            if (_mainCamera == null) _mainCamera = Camera.main;
        }

        private void Update()
        {
            ReadMouseInput();
        }

        private void FixedUpdate()
        {
            HandleRotation();
        }

        private void ReadMouseInput()
        {
            // Membaca posisi mouse di layar (New Input System)
            if (Mouse.current != null)
            {
                _mouseScreenPosition = Mouse.current.position.ReadValue();
            }
        }

        private void HandleRotation()
        {
            if (_mainCamera == null || _visualContainer == null) return;

            // 1. Mengubah posisi mouse (Layar) menjadi koordinat dunia (World)
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(_mouseScreenPosition.x, _mouseScreenPosition.y, 0f));
            mouseWorldPosition.z = 0f; // Memastikan kita tetap di bidang 2D

            // 2. Menghitung arah vector dari karakter ke mouse
            Vector3 lookDirection = mouseWorldPosition - transform.position;

            // 3. Menghitung sudut (dalam Radian, lalu ke Derajat) menggunakan Atan2
            // Atan2 mengembalikan sudut antara sumbu X dan vector arah.
            // Di Unity Top-Down, visual default biasanya menghadap ke Kanan (X) atau Atas (Y).
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

            // 4. Menerapkan rotasi ke Visual Container pada sumbu Z
            // Opsional: Anda mungkin butuh offset (misalnya angle - 90f) tergantung orientasi sprite asli.
            _visualContainer.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}