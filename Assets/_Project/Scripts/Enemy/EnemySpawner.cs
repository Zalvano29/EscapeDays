using UnityEngine;
using System.Collections;
using Pathfinding; // Untuk integrasi Grid A*

namespace EscapeDays.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Referensi Objek")]
        [Tooltip("Prefab musuh yang ingin di-spawn")]
        [SerializeField] private GameObject _enemyPrefab;
        [Tooltip("Referensi transform pemain")]
        [SerializeField] private Transform _playerTransform;

        [Header("Pengaturan Tempo")]
        [SerializeField] private float _spawnInterval = 3f;
        [SerializeField] private int _maxEnemiesInScene = 15;
        
        [Header("Area Peta (Kotak Spawn)")]
        [Tooltip("Titik tengah dari peta permainan Anda (Biasanya 0,0)")]
        [SerializeField] private Vector2 _mapCenter = Vector2.zero;
        [Tooltip("Lebar (X) dan Tinggi (Y) keseluruhan peta Anda")]
        [SerializeField] private Vector2 _mapSize = new Vector2(40f, 40f);

        [Header("Area Aman Pemain")]
        [Tooltip("Musuh tidak boleh spawn di dalam jarak ini dari pemain")]
        [SerializeField] private float _safeZoneRadius = 8f;

        [Header("Sistem Keamanan Anti-Tembok")]
        [Tooltip("Layer yang digunakan untuk lantai padat / SolidWall Anda")]
        [SerializeField] private LayerMask _solidWallLayer;
        [Tooltip("Radius lingkaran fisik badan musuh untuk cek tabrakan dengan tembok")]
        [SerializeField] private float _bodyCheckRadius = 0.4f;
        [Tooltip("Berapa kali sistem mencoba mencari titik kosong di frame tersebut")]
        [SerializeField] private int _maxAttempts = 15;

        private void Start()
        {
            if (_playerTransform == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) _playerTransform = player.transform;
            }

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

                if (currentEnemyCount < _maxEnemiesInScene && _playerTransform != null)
                {
                    TrySpawnEnemy();
                }
            }
        }

        private void TrySpawnEnemy()
        {
            for (int i = 0; i < _maxAttempts; i++)
            {
                // 1. Tentukan titik acak di dalam kotak batas peta
                float randomX = Random.Range(_mapCenter.x - (_mapSize.x / 2f), _mapCenter.x + (_mapSize.x / 2f));
                float randomY = Random.Range(_mapCenter.y - (_mapSize.y / 2f), _mapCenter.y + (_mapSize.y / 2f));
                Vector2 potentialSpawnPos = new Vector2(randomX, randomY);

                // 2. CEK AREA AMAN: Jika titik ini terlalu dekat dengan pemain, batalkan dan cari titik lain
                if (Vector2.Distance(potentialSpawnPos, _playerTransform.position) < _safeZoneRadius)
                {
                    continue; 
                }

                // 3. CEK TEMBOK: Pastikan badan musuh tidak menyangkut di dalam SolidWall
                Collider2D hitWall = Physics2D.OverlapCircle(potentialSpawnPos, _bodyCheckRadius, _solidWallLayer);

                // 4. CEK A* PATHFINDING: Pastikan titik tersebut bisa dipijak (Walkable)
                bool isAStarWalkable = true;
                if (AstarPath.active != null)
                {
                    var node = AstarPath.active.GetNearest(potentialSpawnPos).node;
                    if (node != null && !node.Walkable)
                    {
                        isAStarWalkable = false; 
                    }
                }

                // JIKA SEMUA UJIAN LULUS: Lahirkan musuh!
                if (hitWall == null && isAStarWalkable)
                {
                    Instantiate(_enemyPrefab, potentialSpawnPos, Quaternion.identity);
                    return; 
                }
            }
        }

        // Fitur untuk menggambar kotak kuning (Batas Map) dan lingkaran hijau (Area Aman) di Editor
        private void OnDrawGizmosSelected()
        {
            // Gambar batas kotak spawn map
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_mapCenter, _mapSize);

            // Gambar zona aman pemain (jika game sedang dimainkan)
            if (_playerTransform != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_playerTransform.position, _safeZoneRadius);
            }
        }
    }
}