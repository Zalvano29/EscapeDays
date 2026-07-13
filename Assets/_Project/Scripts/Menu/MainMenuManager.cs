using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk memanggil Scene
using EscapeDays.Core;

namespace EscapeDays.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField] private AudioClip _mainMenuBGM;
        [SerializeField] private AudioClip _clickSFX;

        private void Start()
        {
            // Memutar BGM Main Menu saat scene ini dimuat
            if (AudioManager.Instance != null && _mainMenuBGM != null)
            {
                AudioManager.Instance.PlayMusic(_mainMenuBGM);
            }
        }

        private void PlayClickSound()
        {
            if (AudioManager.Instance != null && _clickSFX != null)
            {
                AudioManager.Instance.PlaySFX(_clickSFX);
            }
        }

        // Fungsi untuk tombol Bermain (Play)
        public void PlayGame()
        {
            PlayClickSound();
            StartCoroutine(DelayAction(() => 
            {
                // Pastikan ejaan "TestingScene" sama persis dengan nama file Scene Anda
                SceneManager.LoadScene("TestingScene");
                
                // Mengembalikan waktu berjalan normal (mencegah bug nyangkut di kondisi pause)
                Time.timeScale = 1f; 
            }));
        }

        // Fungsi untuk tombol Keluar (Quit)
        public void QuitGame()
        {
            PlayClickSound();
            StartCoroutine(DelayAction(() => 
            {
                // Perintah ini akan menutup aplikasi saat game sudah di-build (menjadi .exe/.apk)
                Application.Quit();

                // PENTING: Application.Quit() TIDAK AKAN menutup Unity Editor saat dites.
                Debug.Log("Permainan Ditutup! (Aplikasi Quit Berhasil)");
            }));
        }

        private IEnumerator DelayAction(System.Action action)
        {
            // Beri waktu 0.2 detik agar suara klik terdengar sebelum pindah layar
            yield return new WaitForSecondsRealtime(0.2f);
            action?.Invoke();
        }
    }
}