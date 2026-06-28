using UnityEngine;

namespace EscapeDays.Core.Bootstrap
{
    /// <summary>
    /// Entry point proyek. Bertugas menginisialisasi semua Manager
    /// agar tidak perlu diletakkan secara manual di setiap scene.
    /// </summary>
    public class GameBootstrapper : MonoBehaviour
    {
        [Header("Core Systems")]
        [SerializeField] private InputManager _inputManagerPrefab;
        // Nanti kita akan tambahkan AudioManagerPrefab, SaveSystemPrefab, dll di sini.

        private void Awake()
        {
            InitializeCoreSystems();
        }

        private void InitializeCoreSystems()
        {
            if (!InputManager.HasInstance && _inputManagerPrefab != null)
            {
                Instantiate(_inputManagerPrefab);
                Debug.Log("[GameBootstrapper] InputManager berhasil diinisialisasi.");
            }
        }
    }
}