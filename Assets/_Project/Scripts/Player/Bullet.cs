using UnityEngine;
using System.Collections; // Wajib untuk menggunakan Coroutine (timer)

namespace EscapeDays.Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _lifetime = 3f;

        private Rigidbody2D _rb;
        private int _enemyLayerIndex;

        private void Awake()
        {
            // Awake HANYA dipanggil 1 kali seumur hidup peluru saat pertama kali diciptakan
            _rb = GetComponent<Rigidbody2D>();
            _enemyLayerIndex = LayerMask.NameToLayer("Enemy");
        }

        // OnEnable dipanggil SETIAP KALI peluru diaktifkan (dibangunkan dari tidur)
        private void OnEnable()
        {
            // 1. Beri gaya dorong (Reset kecepatan)
            _rb.linearVelocity = transform.right * _speed;

            // 2. Mulai timer untuk menidurkan peluru jika tidak menabrak apa-apa
            StartCoroutine(SleepAfterLifetime());
        }

        private IEnumerator SleepAfterLifetime()
        {
            yield return new WaitForSeconds(_lifetime);

            // JANGAN gunakan Destroy. Matikan saja agar bisa didaur ulang nanti.
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == _enemyLayerIndex)
            {
                Enemy.EnemyVitals enemyVitals = collision.collider.GetComponentInParent<Enemy.EnemyVitals>();
                if (enemyVitals != null)
                {
                    enemyVitals.TakeDamage(_damage);
                }
            }

            // Peluru tertidur saat menabrak apa pun (fisik)
            gameObject.SetActive(false);
        }

        // TAMBAHAN: Tangani juga jika musuh menggunakan Collider "Is Trigger"
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == _enemyLayerIndex)
            {
                Enemy.EnemyVitals enemyVitals = collision.GetComponentInParent<Enemy.EnemyVitals>();
                if (enemyVitals != null)
                {
                    enemyVitals.TakeDamage(_damage);
                }
                
                // Peluru tertidur saat menabrak musuh (trigger)
                gameObject.SetActive(false);
            }
            // Catatan: Jika menabrak trigger SELAIN musuh (misal zona), peluru dibiarkan menembus
        }
    }
}