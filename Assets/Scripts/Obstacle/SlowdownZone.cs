using UnityEngine;

public class SlowdownZone : MonoBehaviour
{
    [SerializeField] private float slowFactor = 0.5f;  // Persentase pengurangan kecepatan

    private void OnTriggerEnter(Collider other)
    {
        TPMovementRB player = other.GetComponent<TPMovementRB>();
        if (player != null)
        {
            // Kurangi kecepatan player dengan faktor slowFactor
            player.SetSpeedMultiplier(slowFactor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TPMovementRB player = other.GetComponent<TPMovementRB>();
        if (player != null)
        {
            // Kembalikan kecepatan player ke normal (multiplier 1)
            player.SetSpeedMultiplier(1f);
        }
    }
}
