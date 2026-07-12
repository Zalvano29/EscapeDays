using UnityEngine;
using EscapeDays.Enemy; // Pastikan memanggil namespace musuh Anda

namespace EscapeDays.Weapon
{
    public class BulletProjectile : MonoBehaviour
    {
        [Header("Pengaturan Tabrakan")]
        [Tooltip("Layer tembok atau rintangan yang bisa menghancurkan peluru")]
        [SerializeField] private LayerMask _obstacleLayer;

        private float _bulletDamage;
        private bool _hasHit = false; // GEMBOK: Mencegah peluru memberi damage 2x

        // Fungsi ini dipanggil oleh pistol saat peluru baru keluar dari moncong
        public void SetupBullet(float damageValue)
        {
            _bulletDamage = damageValue;
            _hasHit = false; // Buka gembok kembali setiap kali peluru diambil dari Pool
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 1. Jika peluru ini sudah mengenai sesuatu di frame sebelumnya, abaikan!
            if (_hasHit) return;

            // 2. Mencegah peluru menabrak area "pandangan" musuh (Trigger Collider)
            if (collision.isTrigger) return; 

            // 3. Cek apakah yang ditabrak adalah musuh (Cari di objek itu atau induknya)
            // Menggunakan GetComponentInParent sangat ampuh jika Collider ada di anak objek (Visual)
            // tapi skrip Vitals ada di induk objek.
            EnemyVitals enemy = collision.GetComponentInParent<EnemyVitals>();

            if (enemy != null)
            {
                _hasHit = true; // Kunci peluru!
                
                // Berikan damage
                enemy.TakeDamage(_bulletDamage, 0f, transform);
                
                // Kembalikan peluru ke dalam Pool (hilangkan dari layar)
                gameObject.SetActive(false); 
                return;
            }

            // 4. Jika tidak kena musuh, cek apakah kena tembok/rintangan
            // (Menggunakan perbandingan LayerMask)
            if (((1 << collision.gameObject.layer) & _obstacleLayer) != 0)
            {
                _hasHit = true;
                gameObject.SetActive(false);
            }
        }
    }
}