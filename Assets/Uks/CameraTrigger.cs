using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public Camera mainCamera;         // Kamera utama
    public Camera alternateCamera;    // Kamera alternatif
    private bool isMainCameraActive = true; // Status kamera saat ini

    private void Start()
    {
        // Pastikan kamera utama aktif di awal
        mainCamera.gameObject.SetActive(true);
        alternateCamera.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah objek yang menyentuh adalah player
        if (other.CompareTag("Player"))
        {
            // Toggle status kamera
            isMainCameraActive = !isMainCameraActive;

            // Aktifkan kamera berdasarkan status terbaru
            mainCamera.gameObject.SetActive(isMainCameraActive);
            alternateCamera.gameObject.SetActive(!isMainCameraActive);
        }
    }
}
