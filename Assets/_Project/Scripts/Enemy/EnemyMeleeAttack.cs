using UnityEngine;
using System.Collections;

namespace EscapeDays.Enemy
{
    // Skrip ini dipasang pada objek 'MeleePoint' (anak dari musuh)
    // yang memiliki Collider dengan IsTrigger aktif.
    public class EnemyMeleeAttack : MonoBehaviour
    {
        [Header("Statistik Serangan Melee")]
        [SerializeField] private float _meleeDamage = 15f;

        // Jeda waktu antar gigitan (Cooldown)
        [SerializeField] private float _attackCooldown = 1.2f;

        private float _lastAttackTime;

        // Menggunakan OnTrigger karena MeleePoint adalah Trigger
        private void OnTriggerStay2D(Collider2D other)
        {
            // 1. Cek Cooldown
            if (Time.time - _lastAttackTime < _attackCooldown) return;

            // 2. Cek apakah yang masuk zona Trigger memiliki PlayerVitals
            if (other.TryGetComponent(out Player.PlayerVitals playerVitals))
            {
                // 3. Eksekusi Damage
                playerVitals.TakeDamage(_meleeDamage);

                // 4. Catat waktu agar cooldown berjalan
                _lastAttackTime = Time.time;
            }
        }
    }
}