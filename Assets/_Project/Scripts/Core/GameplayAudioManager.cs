using UnityEngine;

namespace EscapeDays.Core
{
    public class GameplayAudioManager : MonoBehaviour
    {
        [Header("Gameplay Audio")]
        [SerializeField] private AudioClip _gameplayBGM;

        private void Start()
        {
            // Memutar BGM Gameplay saat scene ini dimuat
            if (AudioManager.Instance != null && _gameplayBGM != null)
            {
                AudioManager.Instance.PlayMusic(_gameplayBGM);
            }
            else
            {
                Debug.LogWarning("AudioManager tidak ditemukan di scene, atau BGM belum diatur!");
            }
        }
    }
}
