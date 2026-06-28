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

        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnHungerChanged;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _currentHunger = _maxHunger;
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
            _currentHealth -= amount;
            _currentHealth = Mathf.Max(_currentHealth, 0f);

            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // 1. Matikan kaki (Berjalan)
            if (_movementScript != null) _movementScript.enabled = false;

            // 2. Matikan leher/mata (Rotasi)
            if (_rotationScript != null) _rotationScript.enabled = false;

            // 3. Matikan jari (Menembak)
            if (_shootingScript != null) _shootingScript.enabled = false;

            // 4. Rem darurat fisika
            if (TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }
}