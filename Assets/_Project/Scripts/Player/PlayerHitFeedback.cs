using System.Collections;
using UnityEngine;
using EscapeDays.Core; // WAJIB DITAMBAHKAN: Untuk mengakses AudioManager

namespace EscapeDays.Player
{
    public class PlayerHitFeedback : MonoBehaviour
    {
        [Header("Pengaturan Efek Visual")]
        [SerializeField] private Color _flashColor = Color.red;
        [SerializeField] private float _flashDuration = 0.1f;
        
        [Header("Referensi Visual")] 
        [Tooltip("Tarik objek VisualContainer ke sini")]
        [SerializeField] private SpriteRenderer _sprite; 

        [Header("Pengaturan Fisika (Knockback)")]
        [SerializeField] private float _knockbackImpulse = 20f;
        [SerializeField] private LayerMask _enemyLayer;
        [SerializeField] private MonoBehaviour _playerMovementScript;

        [Header("Pengaturan Audio")] // TAMBAHAN BARU
        [Tooltip("Masukkan file punch-sfx.mp3 ke sini")]
        [SerializeField] private AudioClip _hitSFX;

        private PlayerVitals _vitals;
        private Rigidbody2D _rb;
        private Color _originalColor;
        private float _lastHealth = 100f;

        private void Awake()
        {
            _vitals = GetComponent<PlayerVitals>();
            _rb = GetComponent<Rigidbody2D>();
            
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
            // Jika HP saat ini LEBIH KECIL dari HP sebelumnya, berarti pemain terkena damage!
            if (currentHealth < _lastHealth)
            {
                // 1. MAINKAN SUARA TERPUKUL
                if (AudioManager.Instance != null && _hitSFX != null)
                {
                    AudioManager.Instance.PlaySFX(_hitSFX);
                }

                // 2. MUNCULKAN KEDIPAN MERAH
                if (_sprite != null) StartCoroutine(FlashEffect());

                // 3. PENTALKAN PEMAIN
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

            yield return new WaitForSeconds(0.2f);

            if (currentHealth > 0)
            {
                if (_playerMovementScript != null) _playerMovementScript.enabled = true;
            }
            else
            {
                _rb.linearVelocity = Vector2.zero;
                _rb.angularVelocity = 0f;
            }
        }
    }
}