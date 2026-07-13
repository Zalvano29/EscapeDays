using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // WAJIB DITAMBAHKAN: Untuk memuat ulang/berpindah scene
using EscapeDays.Core; // WAJIB DITAMBAHKAN: Untuk mengakses AudioManager

namespace EscapeDays.UI
{
    public class DeathUIManager : MonoBehaviour
    {
        [Header("Referensi Logika")]
        [SerializeField] private Player.PlayerVitals _playerVitals;

        [Header("Referensi Visual UI")]
        [SerializeField] private GameObject _deathHUDGroup;

        [Header("Pengaturan Tampilan")]
        [SerializeField] private float _showDelay = 1.0f;

        [Header("Audio Settings")] // TAMBAHAN BARU: Slot Audio untuk Death Screen
        [SerializeField] private AudioClip _clickSFX;

        private void Awake()
        {
            if (_deathHUDGroup != null) _deathHUDGroup.SetActive(false);
        }

        private void OnEnable()
        {
            if (_playerVitals != null) _playerVitals.OnPlayerDeath += HandlePlayerDeath;
        }

        private void OnDisable()
        {
            if (_playerVitals != null) _playerVitals.OnPlayerDeath -= HandlePlayerDeath;
        }

        private void HandlePlayerDeath()
        {
            if (_deathHUDGroup != null) StartCoroutine(ShowDeathScreenWithDelay());
        }

        private IEnumerator ShowDeathScreenWithDelay()
        {
            yield return new WaitForSeconds(_showDelay);
            _deathHUDGroup.SetActive(true);
        }

        // TAMBAHAN BARU: Fungsi internal untuk memicu suara
        private void PlayClickSound()
        {
            if (AudioManager.Instance != null && _clickSFX != null)
            {
                AudioManager.Instance.PlaySFX(_clickSFX);
            }
        }

        // ==========================================
        // FUNGSI UTAMA TOMBOL (SUDAH DIPERBARUI DENGAN DELAY AUDIO)
        // ==========================================

        public void RestartGame()
        {
            PlayClickSound(); // Putar suara klik
            StartCoroutine(DelayRestartRoutine());
        }

        private IEnumerator DelayRestartRoutine()
        {
            // Beri jeda 0.2 detik agar suara klik tidak langsung terputus oleh loading scene
            yield return new WaitForSecondsRealtime(0.2f);

            // Memuat ulang scene yang saat ini sedang dimainkan
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
            // Memastikan waktu berjalan normal
            Time.timeScale = 1f; 
        }

        public void GoToMainMenu()
        {
            PlayClickSound(); // Putar suara klik
            StartCoroutine(DelayMainMenuRoutine());
        }

        private IEnumerator DelayMainMenuRoutine()
        {
            // Beri jeda 0.2 detik agar suara klik terdengar terlebih dahulu
            yield return new WaitForSecondsRealtime(0.2f);

            // Pindah ke scene Main Menu memakai teks (atau angka 0 jika Anda merubahnya ke index)
            SceneManager.LoadScene("MainMenu"); 
            Time.timeScale = 1f;
        }
    }
}