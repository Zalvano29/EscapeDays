using UnityEngine;

namespace EscapeDays.Enemy
{
    public class EnemyVitals : MonoBehaviour
    {
        [Header("Health Settings")]
        [SerializeField] private float _maxHealth = 50f;
        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        // Metode publik agar pedang pemain bisa melukai musuh ini
        public void TakeDamage(float damageAmount)
        {
            _currentHealth -= damageAmount;

            // Cetak ke Console agar kita tahu darahnya berkurang
            Debug.Log($"[EnemyVitals] {gameObject.name} terkena {damageAmount} damage! Sisa darah: {_currentHealth}");

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log($"[EnemyVitals] {gameObject.name} hancur!");
            // Menghancurkan objek dummy dari arena game
            Destroy(gameObject);
        }
    }
}
