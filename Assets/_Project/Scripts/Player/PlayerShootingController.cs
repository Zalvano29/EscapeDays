using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Player
{
    public class PlayerShootingController : MonoBehaviour
    {
        [Header("Gun Settings")]
        [Tooltip("Tarik titik kumpul (ujung laras senjata) ke sini")]
        [SerializeField] private Transform _firePoint;
        [Tooltip("Tarik prefab Bullet dari folder Project ke sini")]
        [SerializeField] private GameObject _bulletPrefab;

        [Header("Shooting Logic")]
        [SerializeField] private float _fireRate = 0.2f;
        private float _nextFireTime = 0f;

        private void Update()
        {
            // Menggunakan klik kiri mouse (atau tombol lain) untuk menembak
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                if (Time.time >= _nextFireTime)
                {
                    Shoot();
                    _nextFireTime = Time.time + _fireRate;
                }
            }
        }

        private void Shoot()
        {
            // Menciptakan kloningan peluru di posisi dan rotasi ujung laras senjata
            Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
        }
    }
}