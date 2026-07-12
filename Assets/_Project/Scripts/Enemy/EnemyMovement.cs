using UnityEngine;
using Pathfinding; // Wajib dipanggil untuk mengakses komponen A* Pathfinding

namespace EscapeDays.Enemy
{
    [RequireComponent(typeof(AIPath))]
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Referensi Target")]
        [SerializeField] private Transform _playerTransform;

        [Header("Pengaturan Jarak Deteksi")]
        [Tooltip("Jarak maksimal musuh bisa melihat pemain untuk mulai mengejar")]
        [SerializeField] private float _detectionRange = 7f;
        
        [Tooltip("Jarak di mana musuh kehilangan jejak pemain (sebaiknya lebih besar dari jarak deteksi)")]
        [SerializeField] private float _loseRange = 10f;

        private IAstarAI _ai;
        private bool _isChasing = false;

        private void Awake()
        {
            // IAstarAI adalah interface universal untuk AIPath / RichAI
            _ai = GetComponent<IAstarAI>(); 

            // Pengaman otomatis jika lupa memasukkan target Player di Inspector
            if (_playerTransform == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null) _playerTransform = playerObj.transform;
            }
        }

        private void Update()
        {
            if (_playerTransform == null || _ai == null) return;

            // 1. Hitung jarak antara posisi musuh dan posisi pemain
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);

            // 2. Logika Status Pengejaran
            if (!_isChasing && distanceToPlayer <= _detectionRange)
            {
                // Pemain masuk radius -> Mulai mengejar
                _isChasing = true;
            }
            else if (_isChasing && distanceToPlayer > _loseRange)
            {
                // Pemain terlalu jauh keluar radius -> Berhenti mengejar
                _isChasing = false;
            }

            // 3. Eksekusi Pergerakan ke A* Pathfinding
            if (_isChasing)
            {
                // Kirim koordinat pemain secara real-time ke sistem A*
                _ai.destination = _playerTransform.position;
                _ai.canMove = true;
            }
            else
            {
                // Jika kehilangan jejak, suruh AI diam di posisi dirinya sendiri saat ini
                _ai.destination = transform.position;
                
                // Opsional: Anda bisa mematikan canMove, namun menyamakan destination ke diri sendiri 
                // lebih aman agar mayat/musuh tidak mengalami glitch geser
            }
        }

        // FITUR BONUS UNTUK TIM DESIGNER: Menggambar lingkaran radius di jendela Scene Unity
        private void OnDrawGizmosSelected()
        {
            // Lingkaran Merah = Jarak pandang musuh mulai mengejar
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);

            // Lingkaran Kuning = Batas pemain bisa kabur meloloskan diri
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _loseRange);
        }

        public void OnProvoked()
        {
            _isChasing = true;
            
            if (_ai != null) 
            {
                _ai.isStopped = false; // Lepas rem
            }

            // TRIK PENTING: Perluas batas jarak menyerah (_loseRange)
            // Jika pemain menembak dari jarak 15 meter (di luar loseRange 10 meter),
            // musuh akan langsung berhenti lagi di detik berikutnya jika ini tidak diubah.
            // Dengan mengubahnya menjadi 30 meter, musuh akan gigih mengejar!
            if (_loseRange < 30f)
            {
                _loseRange = 30f; 
            }
        }
    }
}