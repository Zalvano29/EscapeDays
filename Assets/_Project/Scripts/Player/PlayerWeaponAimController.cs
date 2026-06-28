using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Player
{
    public class PlayerWeaponAimController : MonoBehaviour
    {
        [Header("Referensi Transform")]
        [Tooltip("Tarik objek Pivot/Axis yang akan diputar mengikuti mouse ke sini")]
        [SerializeField] private Transform _aimAxis;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            
            // Auto-wiring jika Anda lupa memasukkan di Inspector
            if (_aimAxis == null) _aimAxis = transform.Find("PlayerAimAxis");
        }

        private void Update()
        {
            HandleAiming();
        }

        private void HandleAiming()
        {
            if (Mouse.current == null || _mainCamera == null || _aimAxis == null) return;

            // 1. Dapatkan posisi mouse di dunia game
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);

            // 2. Hitung arah dari Pivot ke Mouse
            Vector2 direction = (mouseWorldPosition - _aimAxis.position).normalized;

            // 3. Hitung sudut putarannya (menggunakan Atan2)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // 4. Terpakan rotasi tersebut ke Pivot Axis (hanya di sumbu Z)
            _aimAxis.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}