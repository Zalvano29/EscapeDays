using UnityEngine;
using UnityEngine.InputSystem; // Wajib untuk membaca input tombol Esc dan P
using UnityEngine.SceneManagement; // Wajib untuk fungsi kembali ke Main Menu

namespace EscapeDays.UI
{
    public class PauseMenuManager : MonoBehaviour
    {
        [Header("Referensi UI")]
        [Tooltip("Tarik grup objek PauseMenuUI (yang berisi tombol & judul) ke sini")]
        [SerializeField] private GameObject _pauseMenuPanel;

        private bool _isPaused = false;

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
            if (_pauseMenuPanel != null) _pauseMenuPanel.SetActive(false);
            
            // Mengembalikan waktu berjalan normal (1 detik di dunia nyata = 1 detik di game)
            Time.timeScale = 1f; 
            _isPaused = false;
        }

        // Fungsi untuk tombol MenuButton
        public void GoToMainMenu()
        {
            // SANGAT PENTING: Waktu harus dikembalikan ke normal sebelum pindah scene
            // Jika tidak, Main Menu Anda akan ikut "membeku" (animasinya tidak jalan)
            Time.timeScale = 1f; 
            
            // Kembali ke Index 0 (pastikan MainMenu tetap di posisi 0 pada Build Settings)
            SceneManager.LoadScene(0); 
        }
    }
}