using System.Collections;
using UnityEngine;

namespace EscapeDays.Enemy
{
    public class EnemyVitals : MonoBehaviour
    {
        [Header("Atribut Nyawa")]
        [SerializeField] private float _maxHealth = 30f;
        private float _currentHealth;
        private bool _isDead = false; // GEMBOK: Mencegah efek terpanggil berkali-kali
        
        // TAMBAHAN: Gembok kekebalan sementara untuk mencegah double-damage
        private bool _isInvulnerable = false; 

        [Header("Pengaturan Kematian")]
        [Tooltip("Waktu tunggu setelah animasi mati sebelum musuh lenyap")]
        [SerializeField] private float _destroyDelay = 1.5f;

        [Header("Efek Visual & Fisika")]
        [Tooltip("Tarik objek VisualContainer ke sini")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [Tooltip("Tarik objek VisualContainer ke sini")]
        [SerializeField] private Animator _animator; // TAMBAHAN: Referensi Animator
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
            
            // Cari otomatis jika lupa ditarik di Inspector
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
        }

        public void TakeDamage(float damageAmount, float knockbackForce = 0f, Transform attackerTransform = null)
        {
            // GEMBOK GANDA: Abaikan jika sudah mati ATAU sedang dalam masa kebal sepersekian detik
            if (_isDead || _isInvulnerable) return; 

            // Langsung aktifkan masa kebal begitu peluru pertama menyentuh
            StartCoroutine(InvulnerabilityRoutine());

            _currentHealth -= damageAmount;

            if (TryGetComponent(out EnemyMovement movementBrain))
            {
                movementBrain.OnProvoked();
            }
            
            // Log opsional agar Anda bisa melihat damage yang masuk di Console Unity dengan jelas
            // Debug.Log($"HP Musuh berkurang {damageAmount}. Sisa: {_currentHealth}");

            if (_spriteRenderer != null) StartCoroutine(FlashHitEffect());

            if (knockbackForce > 0f && attackerTransform != null && _currentHealth > 0)
            {
                StartCoroutine(KnockbackRoutine(knockbackForce, attackerTransform));
            }

            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        // FUNGSI BARU: Mengatur jeda kekebalan dari double-hit
        private IEnumerator InvulnerabilityRoutine()
        {
            _isInvulnerable = true;
            
            // Beri waktu kebal 0.1 detik. 
            // Angka ini sangat pas untuk membiarkan peluru yang sama lewat sepenuhnya
            // tanpa membuat musuh terasa kebal dari tembakan peluru berikutnya.
            yield return new WaitForSeconds(0.1f); 
            
            _isInvulnerable = false;
        }

        private IEnumerator FlashHitEffect()
        {
            _spriteRenderer.color = _hitColor;
            yield return new WaitForSeconds(0.1f);
            
            // Kembalikan warna hanya jika musuh belum dihancurkan
            if (_spriteRenderer != null) _spriteRenderer.color = _originalColor;
        }

        private IEnumerator KnockbackRoutine(float force, Transform attacker)
        {
            if (_movementScript != null) _movementScript.enabled = false;

            Vector2 knockbackDirection = (transform.position - attacker.position).normalized;

            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(knockbackDirection * force, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.2f);

            // Cek _isDead juga di sini agar mayat tidak tiba-tiba mengejar pemain
            if (_movementScript != null && _currentHealth > 0 && !_isDead)
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
            _isDead = true;

            // 1. Matikan skrip jalan musuh
            if (_movementScript != null) _movementScript.enabled = false;

            // 2. Rem total fisik musuh dan tembus pandangkan badannya
            if (_rb != null)
            {
                _rb.linearVelocity = Vector2.zero;
                _rb.angularVelocity = 0f;
                _rb.simulated = false; // Peluru dan pemain sekarang bisa melewati jasadnya!
            }

            // 3. Mainkan animasi mati
            if (_animator != null)
            {
                _animator.SetTrigger("Die");
            }

            // 4. Lenyapkan objek setelah jeda waktu habis
            Destroy(gameObject, _destroyDelay);
        }
    }
}