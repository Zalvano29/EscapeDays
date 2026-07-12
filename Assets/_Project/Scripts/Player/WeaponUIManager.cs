using UnityEngine;
using TMPro; // Wajib untuk mengakses komponen TextMeshProUGUI

namespace EscapeDays.UI
{
    public class WeaponUIManager : MonoBehaviour
    {
        [Header("Referensi Logika")]
        [Tooltip("Tarik objek Player (yang memiliki PlayerShootingController) ke sini")]
        [SerializeField] private Player.PlayerShootingController _shootingScript;

        [Header("Referensi Visual UI")]
        [Tooltip("Tarik objek teks AmmoCount ke sini")]
        [SerializeField] private TextMeshProUGUI _ammoText;

        private void OnEnable()
        {
            if (_shootingScript != null)
            {
                // Berlangganan ke dua event dari senjata
                _shootingScript.OnAmmoChanged += UpdateAmmoDisplay;
                _shootingScript.OnReloadStart += ShowReloadingText;
            }
        }

        private void OnDisable()
        {
            if (_shootingScript != null)
            {
                // Berhenti berlangganan saat UI dimatikan
                _shootingScript.OnAmmoChanged -= UpdateAmmoDisplay;
                _shootingScript.OnReloadStart -= ShowReloadingText;
            }
        }

        // Fungsi ini dipanggil otomatis setiap kali pemain menembak atau selesai reload
        private void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
        {
            if (_ammoText != null)
            {
                // Menampilkan format: "30 / 30"
                _ammoText.text = $"{currentAmmo} / {maxAmmo}";
                _ammoText.color = Color.white; // Pastikan teks kembali putih
            }
        }

        // Fungsi ini dipanggil otomatis tepat saat pemain menekan R atau auto-reload
        private void ShowReloadingText()
        {
            if (_ammoText != null)
            {
                _ammoText.text = "Reloading...";
                _ammoText.color = Color.yellow; // Memberi efek visual warna kuning
            }
        }
    }
}