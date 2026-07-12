using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; // WAJIB DITAMBAHKAN: Untuk memuat ulang/berpindah scene

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

        // ==========================================
        // FUNGSI BARU UNTUK TOMBOL
        // ==========================================

        public void RestartGame()
        {
            // Memuat ulang scene yang saat ini sedang dimainkan.
            // Ini akan secara otomatis mereset pemain, menghidupkan kembali musuh, dan mengulang posisi awal.
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
            // Memastikan waktu berjalan normal (berjaga-jaga jika ada fitur pause nantinya)
            Time.timeScale = 1f; 
        }

        public void GoToMainMenu()
        {
            // Pindah ke scene Main Menu. 
            // PASTIKAN ejaan "MainMenu" di bawah ini sama persis dengan nama file Scene Anda!
            SceneManager.LoadScene("MainMenu"); 
            Time.timeScale = 1f;
        }
    }
}