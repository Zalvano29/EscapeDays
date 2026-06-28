using UnityEngine;
using System;

namespace EscapeDays.Player
{
    public class PlayerVitals : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _currentHealth;

        [Header("Hunger Settings")]
        [SerializeField] private float _maxHunger = 100f;
        [SerializeField] private float _hungerDepletionRate = 2f;
        [SerializeField] private float _currentHunger;

        [Header("Referensi Sistem Pengendali")]
        [Tooltip("Skrip untuk berjalan")]
        [SerializeField] private PlayerMovementController _movementScript;

        [Tooltip("Skrip untuk memutar tubuh/mouse")]
        [SerializeField] private PlayerRotationController _rotationScript;

        [Tooltip("Skrip untuk menembak")]
        [SerializeField] private PlayerShootingController _shootingScript;

        [Header("Referensi Visual")]
        [Tooltip("Tarik objek VisualContainer ke sini")]
        [SerializeField] private Animator _animator;

        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnHungerChanged;
        private bool _isDead = false;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _currentHunger = _maxHunger;

            // Auto-cari Animator di anak objek (VisualContainer) jika lupa ditarik
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            HandleHunger();
        }

        private void HandleHunger()
        {
            if (_currentHunger > 0)
            {
                _currentHunger -= _hungerDepletionRate * Time.deltaTime;
                _currentHunger = Mathf.Max(_currentHunger, 0f);
                OnHungerChanged?.Invoke(_currentHunger, _maxHunger);
            }
        }

        public void TakeDamage(float amount)
        {
            // GEMBOK PENGAMAN: Jika sudah mati, abaikan semua damage/serangan masuk!
            if (_isDead) return; 

            _currentHealth -= amount;
            _currentHealth = Mathf.Max(_currentHealth, 0f);

            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                _isDead = true; // Tandai bahwa pemain sudah mati
                Die();
            }
        }

        private void Die()
        {
            // 1. Matikan komponen agar pemain tidak bisa bergerak/menembak
            if (_movementScript != null) _movementScript.enabled = false;
            if (_rotationScript != null) _rotationScript.enabled = false;
            if (_shootingScript != null) _shootingScript.enabled = false;

            // 2. Rem darurat fisika
            if (TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f; 
            }

            // 3. MAINKAN ANIMASI KEMATIAN
            if (_animator != null)
            {
                _animator.SetTrigger("Die");
            }
        }
    }
}