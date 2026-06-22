using System.Collections;
using UnityEngine;

namespace EscapeDays.Enemy
{
    public class EnemyVitals : MonoBehaviour
    {
        [Header("Atribut Nyawa")]
        [SerializeField] private float _maxHealth = 30f;
        private float _currentHealth;

        [Header("Efek Visual & Fisika")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _hitColor = Color.red;

        [Tooltip("Wajib: Rigidbody2D musuh (Pastikan Dynamic & punya Drag)")]
        [SerializeField] private Rigidbody2D _rb;

        [Tooltip("Wajib: Skrip pengejaran musuh (misal AIPath atau EnemyMovement) agar dimatikan saat terpental")]
        [SerializeField] private MonoBehaviour _movementScript;

        private Color _originalColor;

        private void Awake()
        {
            _currentHealth = _maxHealth;
            if (_spriteRenderer != null) _originalColor = _spriteRenderer.color;
            if (_rb == null) _rb = GetComponent<Rigidbody2D>();
        }

        // Fungsi ini kita "Upgrade" agar bisa menerima parameter knockback opsional
        public void TakeDamage(float damageAmount, float knockbackForce = 0f, Transform attackerTransform = null)
        {
            _currentHealth -= damageAmount;

            if (_spriteRenderer != null) StartCoroutine(FlashHitEffect());

            // Jika serangan ini memiliki tenaga knockback (Melee), jalankan efek pentalan
            if (knockbackForce > 0f && attackerTransform != null && _currentHealth > 0)
            {
                StartCoroutine(KnockbackRoutine(knockbackForce, attackerTransform));
            }

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        private IEnumerator FlashHitEffect()
        {
            _spriteRenderer.color = _hitColor;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = _originalColor;
        }

        private IEnumerator KnockbackRoutine(float force, Transform attacker)
        {
            // 1. Matikan skrip jalan musuh (mencegah musuh melawan efek pentalan)
            if (_movementScript != null) _movementScript.enabled = false;

            // 2. Hitung arah pentalan (Menjauh dari pemain)
            Vector2 knockbackDirection = (transform.position - attacker.position).normalized;

            // 3. Reset kecepatan saat ini lalu tembakkan daya pental
            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(knockbackDirection * force, ForceMode2D.Impulse);

            // 4. Tunggu musuh mengudara selama 0.2 detik
            yield return new WaitForSeconds(0.2f);

            // 5. Kembalikan kendali jalan musuh
            if (_movementScript != null && _currentHealth > 0)
            {
                _movementScript.enabled = true;
            }
            else
            {
                _rb.linearVelocity = Vector2.zero;
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}