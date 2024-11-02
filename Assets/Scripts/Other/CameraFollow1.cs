using UnityEngine;

public class CameraFollow1 : MonoBehaviour
{
    [SerializeField] private Transform target; // Target yang akan diikuti

    private void LateUpdate()
    {
        // Cek jika target tersedia
        if (target == null) return;

        // Buat kamera melihat ke arah target tanpa mengubah posisinya
        transform.LookAt(target.position);
    }
}
