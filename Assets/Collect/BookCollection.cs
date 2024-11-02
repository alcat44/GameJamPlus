using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookCollection : MonoBehaviour
{
    private int Book = 0;
    public TextMeshProUGUI bookText;
    public GameObject enemy; // Referensi ke objek enemy
    private bool isEnemyActivated = false; // Flag untuk menghindari aktivasi ganda
    
    private GameObject currentCoin; // Menyimpan referensi koin yang saat ini bisa diambil
    public GameObject intText; // UI untuk teks interaksi

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Book"))
        {
            intText.SetActive(true);
            currentCoin = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Book"))
        {
            intText.SetActive(false);
            currentCoin = null;
        }
    }

    private void Update()
    {
        if (currentCoin != null && Input.GetKeyDown(KeyCode.E))
        {
            CollectBook();
        }
    }

    private void CollectBook()
    {
        Book++;
        bookText.text = "Book: " + Book.ToString();
        Debug.Log(Book);

        Destroy(currentCoin);
        intText.SetActive(false);
        currentCoin = null;

        // Cek jika jumlah buku adalah tiga dan enemy belum diaktifkan
        if (Book == 3 && !isEnemyActivated)
        {
            ActivateEnemy();
        }
    }

    private void ActivateEnemy()
    {
        isEnemyActivated = true;
        
        // Misalnya, tambahkan skrip "ChasePlayer" pada enemy
        enemy.GetComponent<EnemyChase>().StartChasingPlayer();
    }
}
