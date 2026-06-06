using UnityEngine;
using System;

namespace EscapeDays.Player
{
    /// <summary>
    /// Mengatur status hidup pemain (Darah dan Lapar).
    /// Berjalan independen dari sistem pergerakan.
    /// </summary>
    public class PlayerVitals : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _currentHealth;

        [Header("Hunger Settings")]
        [SerializeField] private float _maxHunger = 100f;
        [Tooltip("Berapa banyak rasa lapar berkurang setiap detiknya")]
        [SerializeField] private float _hungerDepletionRate = 2f;
        [SerializeField] private float _currentHunger;

        // Event (Action) agar UI bisa bereaksi tanpa terikat langsung (Decoupling)
        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnHungerChanged;

        private void Awake()
        {
            // Memastikan darah dan rasa lapar penuh saat game dimulai
            _currentHealth = _maxHealth;
            _currentHunger = _maxHunger;
        }

        private void Update()
        {
            HandleHunger();
        }

        private void HandleHunger()
        {
            // Jika pemain belum kelaparan total
            if (_currentHunger > 0)
            {
                // Mengurangi rasa lapar berdasarkan waktu dunia nyata (bukan frame)
                _currentHunger -= _hungerDepletionRate * Time.deltaTime;

                // Mencegah nilai turun di bawah 0
                _currentHunger = Mathf.Max(_currentHunger, 0f);

                // Memicu event untuk memberi tahu UI (jika UI sudah ada)
                OnHungerChanged?.Invoke(_currentHunger, _maxHunger);
            }
            else
            {
                // Implementasi masa depan: Jika lapar = 0, darah mulai berkurang perlahan
            }
        }

        // Metode publik yang bisa dipanggil oleh musuh atau jebakan nanti
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
            Debug.Log("[PlayerVitals] Pemain telah mati!");
            // Logika Game Over akan disambungkan ke sini nanti
        }
    }
}