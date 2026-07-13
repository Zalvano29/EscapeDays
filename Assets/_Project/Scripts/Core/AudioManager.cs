using UnityEngine;

namespace EscapeDays.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        private void Awake()
        {
            // Singleton pattern to ensure only one AudioManager exists and persists between scenes
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Memutar Background Music (BGM).
        /// </summary>
        public void PlayMusic(AudioClip musicClip)
        {
            if (musicClip == null) return;
            
            // Jika musik yang sama sedang diputar, tidak perlu mengulang dari awal
            if (_musicSource.clip == musicClip && _musicSource.isPlaying) return;

            _musicSource.clip = musicClip;
            _musicSource.Play();
        }

        /// <summary>
        /// Menghentikan Background Music (BGM).
        /// </summary>
        public void StopMusic()
        {
            _musicSource.Stop();
        }

        /// <summary>
        /// Memutar Sound Effect (SFX) sekali jalan.
        /// </summary>
        public void PlaySFX(AudioClip sfxClip)
        {
            if (sfxClip == null) return;
            
            _sfxSource.PlayOneShot(sfxClip);
        }
    }
}
