using UnityEngine;
using System.Collections;

public class TrapCone : MonoBehaviour
{
    public int damageAmount = 1;                      // Jumlah nyawa yang akan dikurangi
    public float damageCooldown = 3f;                 // Cooldown dalam detik
    public Animator cameraAnimator;                   // Animator kamera yang bisa diatur di Inspector
    public Color cooldownColor = Color.red;           // Warna yang digunakan saat cooldown
    public GameObject objectDuringCooldown;
    public GameObject objectDelete; 
    private bool canDamage = true;                    // Flag untuk mengecek apakah bisa memberikan damage
    private Color originalColor;                      // Menyimpan warna asli dari material player
    private MaterialPropertyBlock propertyBlock;      // Property block untuk mengubah parameter material

    private void Start()
    {
        propertyBlock = new MaterialPropertyBlock();  // Inisialisasi MaterialPropertyBlock
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canDamage)  // Pastikan objek yang terkena adalah player dan bisa memberikan damage
        {
            HealthManager healthManager = other.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.TakeDamage(damageAmount);
                StartCoroutine(StopCameraAnimationAndChangeMaterial(other));  // Jalankan coroutine untuk menghentikan animasi kamera dan mengubah material
            }
        }
    }

    private IEnumerator StopCameraAnimationAndChangeMaterial(Collider player)
    {
        canDamage = false;  // Matikan kemampuan untuk memberikan damage

        // Aktifkan objectDuringCooldown
        if (objectDuringCooldown != null)
            objectDuringCooldown.SetActive(true);

        if (objectDelete != null)
            objectDelete.SetActive(false);
        
        // Hentikan animasi kamera jika ada animator yang disetel
        if (cameraAnimator != null)
        {
            cameraAnimator.enabled = false;  // Nonaktifkan animasi pada kamera
        }

        // Ganti warna material player ke cooldownColor
        Renderer playerRenderer = player.GetComponent<Renderer>();
        if (playerRenderer != null)
        {
            playerRenderer.GetPropertyBlock(propertyBlock);                // Ambil properti material saat ini
            originalColor = propertyBlock.GetColor("_BaseColor");          // Simpan warna asli (gunakan "_BaseColor")
            propertyBlock.SetColor("_BaseColor", cooldownColor);           // Set warna cooldown
            playerRenderer.SetPropertyBlock(propertyBlock);                // Terapkan perubahan
        }

        yield return new WaitForSeconds(damageCooldown);  // Tunggu selama cooldown

        // Aktifkan kembali animasi kamera jika ada animator yang disetel
        if (cameraAnimator != null)
        {
            cameraAnimator.enabled = true;  // Aktifkan animasi kembali
        }

        // Kembalikan warna asli material player
        if (playerRenderer != null)
        {
            propertyBlock.SetColor("_BaseColor", originalColor);           // Set kembali ke warna asli
            playerRenderer.SetPropertyBlock(propertyBlock);                // Terapkan perubahan
        }

        // Nonaktifkan objectDuringCooldown
        if (objectDuringCooldown != null)
            objectDuringCooldown.SetActive(false);

        if (objectDelete != null)
            objectDelete.SetActive(true);

        canDamage = true;  // Aktifkan kembali kemampuan untuk memberikan damage
    }
}
