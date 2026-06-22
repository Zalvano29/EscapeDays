using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Wajib ditambahkan untuk sistem baru

namespace EscapeDays.Player
{
    public class BulletPool : MonoBehaviour
    {
        [Header("Pengaturan Gudang")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _poolSize = 20;
        [SerializeField] private Transform _firePoint;

        private Queue<GameObject> _bulletPool = new Queue<GameObject>();

        private void Start()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Enqueue(bullet);
            }
        }

        private void Update()
        {
            // Menggunakan sintaks modern: Mengecek apakah mouse ada, lalu mendeteksi klik kiri
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            GameObject bulletToShoot = _bulletPool.Dequeue();

            bulletToShoot.transform.position = _firePoint.position;
            bulletToShoot.transform.rotation = _firePoint.rotation;
            bulletToShoot.SetActive(true);

            _bulletPool.Enqueue(bulletToShoot);
        }
    }
}