using UnityEngine;

namespace EscapeDays.Player
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _lifetime = 3f; // Peluru hancur otomatis setelah 3 detik

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            // Hancurkan peluru ini setelah beberapa detik agar tidak memenuhi memori
            Destroy(gameObject, _lifetime);
        }

        private void Start()
        {
            _rb.linearVelocity = transform.right * _speed;

            // -----------------------
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Catatan: variabel 'collision' di sini bertipe Collision2D (bukan Collider2D)
            Debug.Log($"[Bullet] Menabrak benda padat: {collision.gameObject.name}");

            // Mengecek layer objek yang ditabrak
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Enemy.EnemyVitals enemyVitals = collision.gameObject.GetComponent<Enemy.EnemyVitals>();
                if (enemyVitals != null)
                {
                    enemyVitals.TakeDamage(_damage);
                }
            }

            // Peluru langsung hancur saat berbenturan fisik dengan apa pun (tembok/musuh)
            Destroy(gameObject);
        }
    }
}