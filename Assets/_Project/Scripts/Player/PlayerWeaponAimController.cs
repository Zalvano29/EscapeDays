using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Player
{
    public class PlayerWeaponAimController : MonoBehaviour
    {
        [Header("Referensi Transform")]
        [Tooltip("Tarik objek PlayerAimAxis ke sini")]
        [SerializeField] private Transform _aimAxis;
        
        [Header("Referensi Visual Senjata")]
        [Tooltip("Tarik objek WeaponVisual ke sini")]
        [SerializeField] private SpriteRenderer _weaponRenderer;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            
            if (_aimAxis == null) _aimAxis = transform.Find("PlayerAimAxis");
        }

        private void Update()
        {
            HandleAiming();
        }

        private void HandleAiming()
        {
            if (Mouse.current == null || _mainCamera == null || _aimAxis == null) return;

            // 1. Hitung posisi dunia dari mouse
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);

            // 2. Arah dan Rotasi Poros
            Vector2 direction = (mouseWorldPosition - _aimAxis.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _aimAxis.rotation = Quaternion.Euler(0, 0, angle);

            // 3. Mencegah Senjata Terbalik (Anti-Upside Down)
            if (_weaponRenderer != null)
            {
                // Jika mouse berada di sebelah kiri badan pemain (sumbu X)
                if (mouseWorldPosition.x < transform.position.x)
                {
                    _weaponRenderer.flipY = true;  // Balik gambar senjata secara vertikal
                }
                else
                {
                    _weaponRenderer.flipY = false; // Kembalikan ke posisi normal
                }
            }
        }
    }
}