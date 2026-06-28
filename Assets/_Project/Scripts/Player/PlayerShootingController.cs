using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Player
{
    // Memaksa Unity agar skrip BulletPool selalu ada di objek yang sama dengan skrip ini
    [RequireComponent(typeof(BulletPool))]
    public class PlayerShootingController : MonoBehaviour
    {
        [Header("Gun Settings")]
        [Tooltip("Tarik titik kumpul (ujung laras senjata) ke sini")]
        [SerializeField] private Transform _firePoint;

        [Header("Shooting Logic")]
        [SerializeField] private float _fireRate = 0.2f;
        private float _nextFireTime = 0f;

        // Referensi internal
        private BulletPool _bulletPool;
        private PlayerVitals _vitals;
        private bool _isDead = false;

        private void Awake()
        {
            // Karena kita pakai RequireComponent, BulletPool pasti ditemukan
            _bulletPool = GetComponent<BulletPool>();

            // Sabuk pengaman kematian
            _vitals = GetComponentInParent<PlayerVitals>();
            if (_vitals != null)
            {
                _vitals.OnHealthChanged += CheckDeath;
            }
        }

        private void OnDestroy()
        {
            if (_vitals != null)
            {
                _vitals.OnHealthChanged -= CheckDeath;
            }
        }

        private void CheckDeath(float currentHealth, float maxHealth)
        {
            if (currentHealth <= 0)
            {
                _isDead = true;
                this.enabled = false;
            }
        }

        private void Update()
        {
            if (_isDead) return;

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
            // ALIH-ALIH MENGGUNAKAN INSTANTIATE, KITA MINTA DARI GUDANG
            if (_bulletPool != null)
            {
                _bulletPool.GetBullet(_firePoint.position, _firePoint.rotation);
            }
        }
    }
}