using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Player
{
    [RequireComponent(typeof(BulletPool))]
    public class PlayerShootingController : MonoBehaviour
    {
        [Header("Gun Settings")]
        [Tooltip("Tarik titik kumpul (ujung laras senjata) ke sini")]
        [SerializeField] private Transform _firePoint;

        [Header("Shooting Logic")]
        [SerializeField] private float _fireRate = 0.2f;
        private float _nextFireTime = 0f;

        [Header("Ammo Settings")]
        [SerializeField] private int _maxAmmo = 30;
        [Tooltip("Waktu yang dibutuhkan untuk reload (dalam detik)")]
        [SerializeField] private float _reloadTime = 1.5f;
        
        private int _currentAmmo;
        private bool _isReloading = false;

        // Referensi internal
        private BulletPool _bulletPool;
        private PlayerVitals _vitals;
        private bool _isDead = false;

        // Event ini sangat berguna agar skrip UI bisa tahu kapan harus mengupdate teks peluru di layar
        public event Action<int, int> OnAmmoChanged;

        public event Action OnReloadStart;

        private void Awake()
        {
            _bulletPool = GetComponent<BulletPool>();

            _vitals = GetComponentInParent<PlayerVitals>();
            if (_vitals != null)
            {
                _vitals.OnHealthChanged += CheckDeath;
            }
        }

        private void Start()
        {
            // 1. Isi penuh peluru saat awal game
            _currentAmmo = _maxAmmo;

            // 2. TAMBAHKAN BARIS INI: Pemicu agar UI langsung sinkron di detik pertama game dimulai
            OnAmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
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
            // ========================================================
            // 1. TAMBAHAN BARU: Gembok mutlak saat game sedang di-pause
            // ========================================================
            if (Time.timeScale == 0f) return; 

            // 2. Gembok jika sudah mati ATAU sedang reload
            if (_isDead || _isReloading) return;

            // Logika Manual Reload (Tombol R)
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                if (_currentAmmo < _maxAmmo) 
                {
                    StartCoroutine(ReloadRoutine());
                    return; 
                }
            }

            // Logika Menembak
            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                if (Time.time >= _nextFireTime)
                {
                    if (_currentAmmo > 0)
                    {
                        Shoot();
                        _nextFireTime = Time.time + _fireRate;
                    }
                    else
                    {
                        StartCoroutine(ReloadRoutine());
                    }
                }
            }
        }

        private void Shoot()
        {
            // 1. Kurangi jumlah peluru
            _currentAmmo--;

            // 2. Kirim sinyal ke UI (jika ada skrip UI yang mendengarkan)
            OnAmmoChanged?.Invoke(_currentAmmo, _maxAmmo);

            // 3. Tembakkan peluru
            if (_bulletPool != null)
            {
                _bulletPool.GetBullet(_firePoint.position, _firePoint.rotation);
            }
        }

        private IEnumerator ReloadRoutine()
        {
            _isReloading = true;
            
            // 2. TEMBAKKAN SINYAL RELOAD KE UI SEBELUM JEDA WAKTU DIMULAI
            OnReloadStart?.Invoke(); 
            Debug.Log("Reloading..."); 

            yield return new WaitForSeconds(_reloadTime);

            _currentAmmo = _maxAmmo;
            _isReloading = false;

            OnAmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
            Debug.Log("Reload Complete!");
        }
    }
}