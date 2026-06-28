using UnityEngine;

namespace EscapeDays.CameraSystem
{
    public class CameraFollowPlayer : MonoBehaviour
    {
        [Header("Target Settings")]
        [Tooltip("Tarik objek Player dari Hierarchy ke sini")]
        [SerializeField] private Transform _target;

        [Header("Camera Dynamics")]
        [Tooltip("Semakin kecil nilainya, semakin lambat/halus kamera menyusul")]
        [SerializeField] private float _smoothSpeed = 5f;

        [Tooltip("Jarak kamera (Wajib Z: -10 agar layar tidak blank)")]
        [SerializeField] private Vector3 _offset = new Vector3(0f, 0f, -10f);

        private void LateUpdate()
        {
            // Jika target hancur atau belum diisi, hentikan fungsi
            if (_target == null) return;

            // 1. Tentukan posisi akhir yang seharusnya dicapai kamera
            Vector3 desiredPosition = _target.position + _offset;

            // 2. Transisi halus dari posisi kamera saat ini ke posisi target menggunakan Lerp
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

            // 3. Terapkan posisi baru ke kamera
            transform.position = smoothedPosition;
        }
    }
}