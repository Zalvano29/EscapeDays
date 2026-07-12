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

        [Header("Thirst Settings")] // TAMBAHAN THIRST
        [SerializeField] private float _maxThirst = 100f;
        [Tooltip("Laju pengurangan rasa haus per detik")]
        [SerializeField] private float _thirstDepletionRate = 3f; // Biasanya haus berkurang sedikit lebih cepat
        [SerializeField] private float _currentThirst;

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

        [Header("Pengaturan Kematian")]
        [Tooltip("Berapa detik jasad dibiarkan sebelum objeknya lenyap")]
        [SerializeField] private float _destroyDelay = 2f;

        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnHungerChanged;
        public event Action<float, float> OnThirstChanged;
        public event Action OnPlayerDeath;
        
        private bool _isDead = false;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            _currentHunger = _maxHunger;
            _currentThirst = _maxThirst; // TAMBAHAN THIRST

            if (_animator == null) _animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            // Memicu nilai UI penuh di awal game
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
            OnHungerChanged?.Invoke(_currentHunger, _maxHunger);
            OnThirstChanged?.Invoke(_currentThirst, _maxThirst); // TAMBAHAN THIRST
        }

        private void Update()
        {
            if (_isDead) return; // Stop pengurangan vitals jika pemain sudah mati

            HandleHunger();
            HandleThirst(); // TAMBAHAN THIRST
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

        private void HandleThirst() // TAMBAHAN THIRST
        {
            if (_currentThirst > 0)
            {
                _currentThirst -= _thirstDepletionRate * Time.deltaTime;
                _currentThirst = Mathf.Max(_currentThirst, 0f);
                OnThirstChanged?.Invoke(_currentThirst, _maxThirst);
            }
        }

        public void TakeDamage(float amount)
        {
            if (_isDead) return; 

            _currentHealth -= amount;
            _currentHealth = Mathf.Max(_currentHealth, 0f);

            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                _isDead = true; 
                Die();
            }
        }

        private void Die()
        {
            if (_movementScript != null) _movementScript.enabled = false;
            if (_rotationScript != null) _rotationScript.enabled = false;
            if (_shootingScript != null) _shootingScript.enabled = false;

            if (TryGetComponent(out Rigidbody2D rb))
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f; 
                rb.simulated = false; 
            }

            if (_animator != null)
            {
                _animator.SetTrigger("Die");
            }

            OnPlayerDeath?.Invoke();

            Destroy(gameObject, _destroyDelay);
        }
    }
}