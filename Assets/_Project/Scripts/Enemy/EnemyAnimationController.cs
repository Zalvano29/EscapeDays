using UnityEngine;

namespace EscapeDays.Enemy
{
    public class EnemyAnimationController : MonoBehaviour
    {
        [Header("Referensi Komponen")]
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Vector3 _lastPosition;

        private void Awake()
        {
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            transform.rotation = Quaternion.identity; 
        }

        private void OnEnable()
        {
            _lastPosition = transform.position;
        }

        // PERUBAHAN UTAMA: Gunakan FixedUpdate agar seirama dengan pergerakan AI/Fisika
        private void FixedUpdate()
        {
            HandleEnemyAnimation();
        }

        private void HandleEnemyAnimation()
        {
            if (_animator == null) return;

            Vector3 positionDelta = transform.position - _lastPosition;
            _lastPosition = transform.position;

            // PERUBAHAN KEDUA: Gunakan fixedDeltaTime
            Vector3 currentVelocity = positionDelta / Time.fixedDeltaTime;
            float currentSpeed = currentVelocity.sqrMagnitude;

            _animator.SetFloat("Speed", currentSpeed);

            // PERUBAHAN KETIGA: Naikkan sedikit batas toleransinya (threshold) menjadi 0.1f
            // Untuk mengabaikan getaran mikroskopis saat musuh sedang bertabrakan/berdempetan
            if (currentSpeed > 0.1f)
            {
                Vector2 moveDirection = currentVelocity.normalized;

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