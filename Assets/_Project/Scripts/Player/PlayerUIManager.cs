using UnityEngine;
using UnityEngine.UI;

namespace EscapeDays.UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("Referensi Logika Pemain")]
        [Tooltip("Tarik objek Player ke sini")]
        [SerializeField] private Player.PlayerVitals _playerVitals;

        [Header("Referensi Komponen UI")]
        [Tooltip("Tarik objek Health_Fill ke sini")]
        [SerializeField] private Image _healthFillBar;
        
        [Tooltip("Tarik objek Hunger_Fill ke sini")]
        [SerializeField] private Image _hungerFillBar;

        [Tooltip("Tarik objek Thirst_Fill ke sini")] // TAMBAHAN THIRST
        [SerializeField] private Image _thirstFillBar;

        private void OnEnable()
        {
            if (_playerVitals != null)
            {
                _playerVitals.OnHealthChanged += UpdateHealthUI;
                _playerVitals.OnHungerChanged += UpdateHungerUI;
                _playerVitals.OnThirstChanged += UpdateThirstUI; // TAMBAHAN THIRST
            }
        }

        private void OnDisable()
        {
            if (_playerVitals != null)
            {
                _playerVitals.OnHealthChanged -= UpdateHealthUI;
                _playerVitals.OnHungerChanged -= UpdateHungerUI;
                _playerVitals.OnThirstChanged -= UpdateThirstUI; // TAMBAHAN THIRST
            }
        }

        private void UpdateHealthUI(float currentHealth, float maxHealth)
        {
            if (_healthFillBar != null)
            {
                _healthFillBar.fillAmount = currentHealth / maxHealth;
            }
        }

        private void UpdateHungerUI(float currentHunger, float maxHunger)
        {
            if (_hungerFillBar != null)
            {
                _hungerFillBar.fillAmount = currentHunger / maxHunger;
            }
        }

        private void UpdateThirstUI(float currentThirst, float maxThirst) // TAMBAHAN THIRST
        {
            if (_thirstFillBar != null)
            {
                _thirstFillBar.fillAmount = currentThirst / maxThirst;
            }
        }
    }
}