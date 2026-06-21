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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"[Bullet] Menabrak objek: {collision.gameObject.name}");

            // Mengecek apakah yang ditabrak berada di layer "Enemy"
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Enemy.EnemyVitals enemyVitals = collision.GetComponent<Enemy.EnemyVitals>();
                if (enemyVitals != null)
                {
                    enemyVitals.TakeDamage(_damage);
                }
            }

            // Peluru hancur setelah mengenai apa pun (musuh atau tembok)
            Destroy(gameObject);
        }
    }
}