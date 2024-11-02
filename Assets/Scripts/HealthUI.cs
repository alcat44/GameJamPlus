using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthUI : MonoBehaviour
{
    public List<Image> hearts;  // Daftar gambar hati

    // Memperbarui tampilan hati sesuai dengan nyawa player saat ini
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }
}
