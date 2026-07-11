using UnityEngine;
using Pathfinding; // Tambahkan namespace A* Pathfinding

namespace EscapeDays.Enemy
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [Header("Referensi Komponen")]
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private IAstarAI _ai;

        private void Awake()
        {
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            // Ambil referensi komponen AI secara paksa di seluruh bagian musuh
            _ai = transform.root.GetComponentInChildren<IAstarAI>();

            transform.rotation = Quaternion.identity; 
        }

        private void Update()
        {
            HandleEnemyAnimation();
        }

        private void HandleEnemyAnimation()
        {
            if (_animator == null) return;

            Vector3 currentVelocity = Vector3.zero;
            Vector3 desiredVelocity = Vector3.zero;

            // Gunakan velocity dari AI jika tersedia
            if (_ai != null)
            {
                currentVelocity = _ai.velocity;
                // desiredVelocity adalah arah yang DIINGINKAN AI, jauh lebih mulus untuk animasi
                desiredVelocity = _ai.desiredVelocity; 
            }

            float currentSpeed = currentVelocity.sqrMagnitude;
            _animator.SetFloat("Speed", currentSpeed);

            // Gunakan desiredVelocity untuk menentukan arah hadap (MoveX, MoveY)
            // Ini mencegah animasi kaku/bergetar saat AI sedikit tersangkut
            if (desiredVelocity.sqrMagnitude > 0.01f)
            {
                Vector2 moveDirection = desiredVelocity.normalized;

                _animator.SetFloat("MoveX", Mathf.Abs(moveDirection.x));
                _animator.SetFloat("MoveY", moveDirection.y);

                if (_spriteRenderer != null)
                {
                    if (moveDirection.x < -0.01f)
                    {
                        _spriteRenderer.flipX = true;
                    }
                    else if (moveDirection.x > 0.01f)
                    {
                        _spriteRenderer.flipX = false;
                    }
                }
            }
        }
    }
}