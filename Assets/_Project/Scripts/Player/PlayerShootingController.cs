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
            // Isi penuh peluru saat awal game
            _currentAmmo = _maxAmmo;
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
            // Jika sudah mati ATAU sedang reload, hentikan proses tembak
            if (_isDead || _isReloading) return;

            // Logika Manual Reload (Tombol R)
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                if (_currentAmmo < _maxAmmo) // Hanya reload jika peluru belum penuh
                {
                    StartCoroutine(ReloadRoutine());
                    return; // Keluar dari Update agar tidak bisa menembak di frame ini
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
                        // Auto Reload jika mencoba menembak tapi peluru habis
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
            
            // Opsional: Panggil animasi atau suara reload di sini
            Debug.Log("Reloading..."); 

            // Jeda selama waktu reload
            yield return new WaitForSeconds(_reloadTime);

            // Isi kembali peluru
            _currentAmmo = _maxAmmo;
            _isReloading = false;

            // Perbarui UI setelah reload selesai
            OnAmmoChanged?.Invoke(_currentAmmo, _maxAmmo);
            Debug.Log("Reload Complete!");
        }
    }
}