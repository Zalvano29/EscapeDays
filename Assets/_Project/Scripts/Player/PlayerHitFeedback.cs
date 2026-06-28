using System.Collections;
using UnityEngine;

namespace EscapeDays.Player
{
    public class PlayerHitFeedback : MonoBehaviour
    {
        [Header("Pengaturan Efek Visual")]
        [SerializeField] private Color _flashColor = Color.red;
        [SerializeField] private float _flashDuration = 0.1f;
        
        [Header("Referensi Visual")] // TAMBAHKAN INI
        [Tooltip("Tarik objek VisualContainer ke sini")]
        [SerializeField] private SpriteRenderer _sprite; // SEKARANG BISA DIISI DI INSPECTOR

        [Header("Pengaturan Fisika (Knockback)")]
        [SerializeField] private float _knockbackImpulse = 20f;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private MonoBehaviour _playerMovementScript;

        private PlayerVitals _vitals;
        private Rigidbody2D _rb;
        private Color _originalColor;
        private float _lastHealth = 100f;

        private void Awake()
        {
            _vitals = GetComponent<PlayerVitals>();
            _rb = GetComponent<Rigidbody2D>();
            
            // HANYA cari otomatis jika di Inspector masih kosong
            if (_sprite == null) _sprite = GetComponentInChildren<SpriteRenderer>();

            if (_sprite != null)
            {
                _originalColor = _sprite.color;
            }
        }

        private void OnEnable()
        {
            if (_vitals != null)
            {
                _vitals.OnHealthChanged += HandleHitEffect;
            }
        }

        private void OnDisable()
        {
            if (_vitals != null)
            {
                _vitals.OnHealthChanged -= HandleHitEffect;
            }
        }

        private void HandleHitEffect(float currentHealth, float maxHealth)
        {
            if (currentHealth < _lastHealth)
            {
                if (_sprite != null) StartCoroutine(FlashEffect());

                // KITA KIRIM DATA HP SAAT INI KE COROUTINE KNOCKBACK
                if (_rb != null) StartCoroutine(ApplySmartKnockbackRoutine(currentHealth));
            }

            _lastHealth = currentHealth;
        }

        private IEnumerator FlashEffect()
        {
            _sprite.color = _flashColor;
            yield return new WaitForSeconds(_flashDuration);
            _sprite.color = _originalColor;
        }

        // COROUTINE SEKARANG MENERIMA DATA HP
        private IEnumerator ApplySmartKnockbackRoutine(float currentHealth)
        {
            if (_playerMovementScript != null) _playerMovementScript.enabled = false;

            Collider2D[] enemiesNearby = Physics2D.OverlapCircleAll(transform.position, 2f, _enemyLayer);
            Vector2 knockbackDirection = Vector2.up;

            if (enemiesNearby.Length > 0)
            {
                Transform nearestEnemy = enemiesNearby[0].transform;
                knockbackDirection = (transform.position - nearestEnemy.position).normalized;
            }

            _rb.linearVelocity = Vector2.zero;
            _rb.AddForce(knockbackDirection * _knockbackImpulse, ForceMode2D.Impulse);

            // Tunggu pemain selesai terpental di udara
            yield return new WaitForSeconds(0.2f);

            // CEK STATUS KEMATIAN:
            // Hanya kembalikan kendali JIKA pemain masih hidup
            if (currentHealth > 0)
            {
                if (_playerMovementScript != null) _playerMovementScript.enabled = true;
            }
            else
            {
                // Jika ini adalah hit yang membunuh pemain, rem total fisika setelah pentalan selesai
                // agar jasadnya tidak meluncur terus jika Anda menekan tombol arah.
                _rb.linearVelocity = Vector2.zero;
                _rb.angularVelocity = 0f;
            }
        }
    }
}