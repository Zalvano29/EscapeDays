using System.Collections;
using UnityEngine;

// Namespace ini harus cocok dengan yang dipanggil oleh peluru sebelumnya
namespace EscapeDays.Enemy
{
    public class EnemyVitals : MonoBehaviour
    {
        [Header("Atribut Nyawa")]
        [SerializeField] private float _maxHealth = 30f;
        private float _currentHealth;

        [Header("Efek Visual")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _hitColor = Color.red;
        private Color _originalColor;

        private void Start()
        {
            // Saat lahir, nyawa musuh penuh
            _currentHealth = _maxHealth;

            // Simpan warna asli musuh (biasanya putih) agar bisa dikembalikan setelah berkedip
            if (_spriteRenderer != null)
            {
                _originalColor = _spriteRenderer.color;
            }
        }

        // Fungsi ini dipanggil oleh skrip Bullet.cs saat peluru menabrak
        public void TakeDamage(float damageAmount)
        {
            _currentHealth -= damageAmount;

            // Memicu efek kedip merah saat terkena *damage*
            if (_spriteRenderer != null)
            {
                StartCoroutine(FlashHitEffect());
            }

            // Cek apakah musuh sudah mati
            if (_currentHealth <= 0)
            {
                Die();
            }
        }

        // Coroutine untuk mengatur waktu kedipan visual
        private IEnumerator FlashHitEffect()
        {
            _spriteRenderer.color = _hitColor; // Ubah ke warna merah
            yield return new WaitForSeconds(0.1f); // Tunggu 0.1 detik
            _spriteRenderer.color = _originalColor; // Kembalikan ke warna asli
        }

        private void Die()
        {
            // Untuk saat ini, musuh langsung kita hancurkan dari arena.
            // Di masa depan, Anda bisa menambahkan efek ledakan, animasi mati, atau menjatuhkan item di sini.
            Destroy(gameObject);
        }
    }
}