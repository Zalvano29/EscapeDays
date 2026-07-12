using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; 

    void Update()
    {
        // Tekan ESC untuk pause/unpause
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Start()
    {
        GameIsPaused = false; 
        Time.timeScale = 1f;  
        
        // Cek agar tidak error jika lupa menarik objek ke Inspector
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Sembunyikan layar gelap dan tombol
        Time.timeScale = 1f;          // Waktu game jalan lagi
        GameIsPaused = false;
    }
    

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Munculkan layar gelap dan tombol kayu
        Time.timeScale = 0f;          // Waktu game berhenti (freeze)
        GameIsPaused = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;          // Kembalikan waktu normal sebelum pindah
        PauseMenu.GameIsPaused = false;
        SceneManager.LoadScene(0);    // Pindah ke Main Menu (Indeks 0)
    }
}