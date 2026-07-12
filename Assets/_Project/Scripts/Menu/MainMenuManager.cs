using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk memanggil Scene

namespace EscapeDays.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        // Fungsi untuk tombol Bermain (Play)
        public void PlayGame()
        {
            // Pastikan ejaan "TestingScene" sama persis dengan nama file Scene Anda
            SceneManager.LoadScene("TestingScene");
            
            // Mengembalikan waktu berjalan normal (mencegah bug nyangkut di kondisi pause)
            Time.timeScale = 1f; 
        }

        // Fungsi untuk tombol Keluar (Quit)
        public void QuitGame()
        {
            // Perintah ini akan menutup aplikasi saat game sudah di-build (menjadi .exe/.apk)
            Application.Quit();

            // PENTING: Application.Quit() TIDAK AKAN menutup Unity Editor saat dites.
            // Kita tambahkan pesan ini agar Anda yakin bahwa tombolnya sudah berfungsi!
            Debug.Log("Permainan Ditutup! (Aplikasi Quit Berhasil)");
        }
    }
}