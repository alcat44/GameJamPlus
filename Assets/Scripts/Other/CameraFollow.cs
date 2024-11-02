using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Target yang akan diikuti, biasanya karakter
    [SerializeField] private float followSpeed = 5f; // Kecepatan kamera mengikuti target
    [SerializeField] private Vector3 offset; // Jarak offset kamera dari target

    private void LateUpdate()
    {
        // Cek jika target tersedia
        if (target == null) return;

        // Tentukan posisi target dengan offset
        Vector3 targetPosition = target.position + offset;

        // Hanya ikuti pada sumbu X dan Y (untuk tampilan 2.5D)
        targetPosition.z = transform.position.z;

        // Interpolasi untuk pergerakan kamera yang halus
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
