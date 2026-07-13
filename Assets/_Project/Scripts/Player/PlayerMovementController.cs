using UnityEngine;
using UnityEngine.InputSystem; // WAJIB ditambahkan untuk sistem baru
using EscapeDays.Core;

namespace EscapeDays.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Pengaturan Pergerakan")]
        [SerializeField] private float _moveSpeed = 5f;

        [Header("Referensi Sistem")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Animator _animator;

        [Header("Audio Settings")]
        [SerializeField] private AudioClip _walkSFX;
        private AudioSource _walkAudioSource;

        private Vector2 _movementInput;

        private void Awake()
        {
            if (_rb == null) _rb = GetComponent<Rigidbody2D>();
            if (_animator == null) _animator = GetComponentInChildren<Animator>(); 
            
            // Buat AudioSource khusus untuk langkah kaki agar bisa di-Stop kapan saja
            _walkAudioSource = gameObject.AddComponent<AudioSource>();
            _walkAudioSource.playOnAwake = false;
            _walkAudioSource.loop = true; // Otomatis mengulang (loop)
        }

        private void Start()
        {
            if (_walkSFX != null)
            {
                _walkAudioSource.clip = _walkSFX;
            }
        }

        private void Update()
        {
            // 1. Membaca input menggunakan Input System Baru
            _movementInput = Vector2.zero;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) _movementInput.y += 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) _movementInput.y -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) _movementInput.x += 1f;
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) _movementInput.x -= 1f;
            }

            // 2. KIRIM DATA KE ANIMATOR
            if (_animator != null)
            {
                _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
            }
        }

        private void FixedUpdate()
        {
            // 3. Eksekusi pergerakan fisik karakter
            _rb.linearVelocity = _movementInput.normalized * _moveSpeed;

            // 4. Mainkan/Hentikan suara langkah kaki
            if (_movementInput.sqrMagnitude > 0.01f)
            {
                // Jika sedang bergerak dan suara belum menyala, nyalakan
                if (!_walkAudioSource.isPlaying && _walkAudioSource.clip != null)
                {
                    _walkAudioSource.Play();
                }
            }
            else
            {
                // Jika berhenti bergerak dan suara masih menyala, MATIKAN seketika
                if (_walkAudioSource.isPlaying)
                {
                    _walkAudioSource.Stop();
                }
            }
        }
    }
}