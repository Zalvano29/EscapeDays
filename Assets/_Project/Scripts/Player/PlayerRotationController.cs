using UnityEngine;
using UnityEngine.InputSystem;

namespace EscapeDays.Player
{
    public class PlayerRotationController : MonoBehaviour
    {
        [Header("Referensi Visual")]
        [Tooltip("Tarik komponen Animator pemain ke sini")]
        [SerializeField] private Animator _animator;
        [Tooltip("Tarik komponen SpriteRenderer pemain ke sini")]
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            
            // Auto-wiring jika Anda lupa memasukkan di Inspector
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            // FASE 1: Kunci mati rotasi fisik pemain agar tidak pernah berputar lagi
            transform.rotation = Quaternion.identity; 
        }

        private void Update()
        {
            HandleVisualDirection();
        }

        private void HandleVisualDirection()
        {
            if (Mouse.current == null || _mainCamera == null) return;

            // 1. Ubah koordinat layar mouse menjadi koordinat dunia game
            Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mouseScreenPosition);

            // 2. Dapatkan vektor arah (dari pemain menuju mouse)
            Vector2 direction = (mouseWorldPosition - transform.position).normalized;

            // FASE 2: Trik Mirroring (FlipX)
            // Jika arah mouse berada di sebelah kiri (X negatif), balik gambarnya ke kiri
            if (direction.x < -0.01f)
            {
                _spriteRenderer.flipX = true;
            }
            // Jika arah mouse berada di sebelah kanan (X positif), kembalikan gambar ke kanan
            else if (direction.x > 0.01f)
            {
                _spriteRenderer.flipX = false;
            }

            // 3. Kirim data ke Animator (Fase 3)
            if (_animator != null)
            {
                // Kita menggunakan Mathf.Abs (Nilai Mutlak) pada sumbu X.
                // Kenapa? Karena animasi Kiri dan Kanan memakai animasi yang sama (Idle_Side).
                // Animator hanya perlu tahu "seberapa kuat tarikan ke sumbu X horizontal", 
                // urusan menghadap Kiri/Kanan sudah diselesaikan oleh FlipX di atas.
                _animator.SetFloat("MouseX", Mathf.Abs(direction.x));
                _animator.SetFloat("MouseY", direction.y);
            }
        }
    }
}