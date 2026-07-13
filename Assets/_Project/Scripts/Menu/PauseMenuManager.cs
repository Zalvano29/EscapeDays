using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Wajib untuk membaca input tombol Esc dan P
using UnityEngine.SceneManagement; // Wajib untuk fungsi kembali ke Main Menu
using EscapeDays.Core;

namespace EscapeDays.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [Header("Referensi UI")]
        [Tooltip("Tarik grup objek PauseMenuUI (yang berisi tombol & judul) ke sini")]
        [SerializeField] private GameObject _pauseMenuPanel;

        [Header("Audio Settings")]
        [SerializeField] private AudioClip _clickSFX;

        private bool _isPaused = false;

        private void PlayClickSound()
        {
            if (AudioManager.Instance != null && _clickSFX != null)
            {
                AudioManager.Instance.PlaySFX(_clickSFX);
            }
        }

        private void Start()
        {
            // Pastikan panel pause disembunyikan saat game pertama kali dimulai
            if (_pauseMenuPanel != null)
            {
                _pauseMenuPanel.SetActive(false);
            }
        }

        private void Update()
        {
            // Mengecek apakah keyboard tersedia
            if (Keyboard.current != null)
            {
                // Jika tombol ESC ATAU tombol P ditekan pada frame ini
                if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
                {
                    if (_isPaused)
                    {
                        ResumeGame(); // Jika sedang pause, lanjutkan
                    }
                    else
                    {
                        PauseGame(); // Jika sedang main, pause
                    }
                }
            }
        }

        public void PauseGame()
        {
            if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(true);
            
            // Menghentikan waktu di dalam game (membuat semua Rigidbody, Animasi, dan AI berhenti)
            Time.timeScale = 0f; 
            _isPaused = true;
        }

        // Fungsi ini bisa dipanggil dari tombol Esc/P, dan juga dari tombol Resume di UI
        public void ResumeGame()
        {
            PlayClickSound();
            if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(false);
            
            // Mengembalikan waktu berjalan normal (1 detik di dunia nyata = 1 detik di game)
            Time.timeScale = 1f; 
            _isPaused = false;
        }

        // Fungsi untuk tombol MenuButton
        public void GoToMainMenu()
        {
            PlayClickSound();
            StartCoroutine(DelayGoToMainMenu());
        }

        private IEnumerator DelayGoToMainMenu()
        {
            // Beri waktu sejenak agar efek suara klik terdengar sebelum pindah scene
            yield return new WaitForSecondsRealtime(0.2f);

            // SANGAT PENTING: Waktu harus dikembalikan ke normal sebelum pindah scene
            Time.timeScale = 1f; 
            
            // Kembali ke Index 0 (pastikan MainMenu tetap di posisi 0 pada Build Settings)
            SceneManager.LoadScene(0); 
        }
    }
}