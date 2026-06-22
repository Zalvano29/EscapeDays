using System.Collections.Generic;
using UnityEngine;

namespace EscapeDays.Player
{
    public class BulletPool : MonoBehaviour
    {
        [Header("Pengaturan Gudang")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _poolSize = 20;

        private Queue<GameObject> _bulletPool = new Queue<GameObject>();

        private void Start()
        {
            // Membangun antrean peluru saat game dimulai (Pre-allocation)
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Enqueue(bullet);
            }
        }

        // Fungsi publik ini yang akan dipanggil oleh senjata saat mau menembak
        public GameObject GetBullet(Vector3 position, Quaternion rotation)
        {
            // Ambil peluru dari antrean terdepan
            GameObject bulletToShoot = _bulletPool.Dequeue();

            // Posisikan dan aktifkan
            bulletToShoot.transform.position = position;
            bulletToShoot.transform.rotation = rotation;
            bulletToShoot.SetActive(true);

            // Masukkan kembali peluru tersebut ke antrean paling belakang (Circular Queue)
            _bulletPool.Enqueue(bulletToShoot);

            return bulletToShoot;
        }
    }
}