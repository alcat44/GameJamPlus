using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private bool isInvincible = false;      // Status kebal sementara

    public HealthUI healthUI;                   // Referensi ke HealthUI
    public List<GameObject> gameOverElements;   // List untuk elemen-elemen UI Game Over
    public GameObject playAgainButton;          // Referensi untuk tombol Play Again

    void Start()
    {
        currentHealth = maxHealth;
        healthUI.UpdateHearts(currentHealth);  // Inisialisasi UI hati
        
        // Sembunyikan semua elemen UI Game Over di awal permainan
        foreach (var element in gameOverElements)
        {
            if (element != null)
                element.SetActive(false);
        }

        if (playAgainButton != null)
            playAgainButton.SetActive(false);  // Sembunyikan tombol Play Again di awal

        // Sembunyikan kursor saat game dimulai
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void TakeDamage(int amount)
    {
        // Jika sedang invincible, abaikan damage
        if (isInvincible)
            return;

        currentHealth -= amount;
        healthUI.UpdateHearts(currentHealth);  // Update UI setiap kali nyawa berkurang

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void ActivateInvincibility(float duration)
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine(duration));
        }
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;  // Aktifkan invincibility
        yield return new WaitForSeconds(duration);  // Tunggu sesuai durasi
        isInvincible = false;  // Matikan invincibility
    }

    public void GameOver()
    {
        Debug.Log("Game Over");

        // Tampilkan semua elemen UI Game Over yang sudah diatur di Inspector
        foreach (var element in gameOverElements)
        {
            if (element != null)
                element.SetActive(true);
        }

        if (playAgainButton != null)
            playAgainButton.SetActive(true);  // Tampilkan tombol Play Again

        // Tampilkan kursor saat Game Over
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;  // Menghentikan permainan
    }

    // Fungsi untuk restart permainan
    public void PlayAgain()
    {
        Time.timeScale = 1;  // Mengembalikan waktu permainan ke normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  // Muat ulang scene saat ini
        
        // Sembunyikan kursor kembali saat permainan dimulai ulang
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
