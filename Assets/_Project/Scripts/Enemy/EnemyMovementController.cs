using UnityEngine;

namespace EscapeDays.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovementController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 3f;

        private Transform _playerTarget;
        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            // AI akan otomatis mencari objek yang memiliki Tag "Player" di arena
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                _playerTarget = player.transform;
            }
            else
            {
                Debug.LogWarning("[EnemyAI] Pemain tidak ditemukan di arena!");
            }
        }

        private void FixedUpdate()
        {
            ChasePlayer();
        }

        private void ChasePlayer()
        {
            // Jika pemain tidak ada atau sudah mati hancur, berhenti bergerak
            if (_playerTarget == null)
            {
                _rb.linearVelocity = Vector2.zero;
                return;
            }

            // Menghitung arah dari musuh ke pemain
            Vector2 direction = (_playerTarget.position - transform.position).normalized;

            // Mengaplikasikan kecepatan ke arah pemain
            _rb.linearVelocity = direction * _moveSpeed;
        }
    }
}