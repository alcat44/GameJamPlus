using UnityEngine;

public class TrapCone : MonoBehaviour
{
    public int damageAmount = 1;  // Jumlah nyawa yang akan dikurangi

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Pastikan objek yang terkena adalah player
        {
            HealthManager healthManager = other.GetComponent<HealthManager>();
            if (healthManager != null)
            {
                healthManager.TakeDamage(damageAmount);
            }
        }
    }
}
