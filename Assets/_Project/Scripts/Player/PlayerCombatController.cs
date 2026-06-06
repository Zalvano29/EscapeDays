using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Player
{
    public class PlayerCombatController : MonoBehaviour
    {
        [Header("Combat Settings")]
        [Tooltip("Tarik objek AttackPoint ke sini")]
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private float _attackRange = 0.5f;
        [SerializeField] private float _attackDamage = 20f;

        [Tooltip("Pilih layer 'Enemy' agar serangan tidak mengenai tembok/diri sendiri")]
        [SerializeField] private LayerMask _enemyLayer;

        private void Update()
        {
            // 2. Fix: Membaca input menggunakan New Input System (sementara kita hardcode ke tombol Spasi)
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Attack();
            }
        }

        private void Attack()
        {
            Debug.Log("[PlayerCombat] Mengayunkan senjata!");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                // Mencari tahu apakah objek yang ditabrak memiliki komponen EnemyVitals
                Enemy.EnemyVitals enemyVitals = enemy.GetComponent<Enemy.EnemyVitals>();

                // Jika punya, eksekusi metode TakeDamage dan kurangi darahnya
                if (enemyVitals != null)
                {
                    enemyVitals.TakeDamage(_attackDamage);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (_attackPoint == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
        }
    }
}