using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Player
{
    public class PlayerMeleeAttack : MonoBehaviour
    {
        [Header("Pengaturan Jangkauan")]
        [Tooltip("Tarik objek kosong yang menjadi titik pusat pukulan di depan pemain")]
        [SerializeField] private Transform _meleePoint;
        [SerializeField] private float _attackRadius = 1.5f;
        [SerializeField] private LayerMask _enemyLayer;

        [Header("Statistik Pukulan")]
        [SerializeField] private float _meleeDamage = 15f;

        [Tooltip("Ini adalah nilai TENAGA PENTALAN (Reward untuk pemain)")]
        [SerializeField] private float _knockbackForce = 15f;
        [SerializeField] private float _attackCooldown = 0.5f;

        private float _nextAttackTime = 0f;
        private PlayerVitals _vitals;
        private bool _isDead = false;

        private void Awake()
        {
            _vitals = GetComponent<PlayerVitals>();
            if (_vitals != null) _vitals.OnHealthChanged += CheckDeath;
        }

        private void OnDestroy()
        {
            if (_vitals != null) _vitals.OnHealthChanged -= CheckDeath;
        }

        private void CheckDeath(float currentHealth, float maxHealth)
        {
            if (currentHealth <= 0) _isDead = true;
        }

        private void Update()
        {
            if (_isDead) return;

            // Membaca klik KANAN mouse untuk memukul
            if (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            {
                if (Time.time >= _nextAttackTime)
                {
                    PerformMelee();
                    _nextAttackTime = Time.time + _attackCooldown;
                }
            }
        }

        private void PerformMelee()
        {
            // 1. Deteksi semua objek ber-layer 'Enemy' di dalam lingkaran area pukul
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_meleePoint.position, _attackRadius, _enemyLayer);

            // 2. Beri efek pada masing-masing musuh yang terkena
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.TryGetComponent(out Enemy.EnemyVitals enemyVitals))
                {
                    // Kirim TIGA data: Jumlah Damage, Tenaga Pentalan, dan Posisi Pemain (sebagai pusat pentalan)
                    enemyVitals.TakeDamage(_meleeDamage, _knockbackForce, transform);
                }
            }
        }

        // Fitur bantuan untuk melihat seberapa besar jangkauan pukul di dalam Unity Editor
        private void OnDrawGizmosSelected()
        {
            if (_meleePoint != null)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(_meleePoint.position, _attackRadius);
            }
        }
    }
}