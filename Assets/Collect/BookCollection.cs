using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BookCollection : MonoBehaviour
{
    private int Book = 0;
    public TextMeshProUGUI bookText;
    public TextMeshProUGUI timerText;
    public GameObject enemy; // Referensi ke objek enemy
    private bool isEnemyActivated = false; // Flag untuk menghindari aktivasi ganda
    public GameObject trigFinishGame;
    public List<GameObject> gameFinishElements;
    public GameObject nextLevel;
    public GameObject mainMenu; 
    private bool isFinishActivated = false;
    public GameObject timer;
    
    private GameObject currentCoin; // Menyimpan referensi koin yang saat ini bisa diambil
    public GameObject intText; // UI untuk teks interaksi

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Book"))
        {
            intText.SetActive(true);
            currentCoin = other.gameObject;
        }

        if(other.CompareTag("Finish"))
        {
            GameFinish();
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
    
    private void Start() 
    {
        trigFinishGame.SetActive(false);
        foreach (var element in gameFinishElements)
        {
            if (element != null)
                element.SetActive(false);
        }

        if (mainMenu != null)
            mainMenu.SetActive(false);
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
            ActivateFinish();
        }
    }

    private void ActivateEnemy()
    {
        isEnemyActivated = true;
        
        // Misalnya, tambahkan skrip "ChasePlayer" pada enemy
        enemy.GetComponent<EnemyChase>().StartChasingPlayer();
    }
    private void ActivateFinish()
    {
        isFinishActivated = true;
        
        trigFinishGame.SetActive(true);
    }
    public void GameFinish()
    {
        Debug.Log("Game Finish");
        timer.GetComponent<Timer>().AddToScore();
        //timer.LoadHighScore();

        // Tampilkan semua elemen UI Game Over yang sudah diatur di Inspector
        foreach (var element in gameFinishElements)
        {
            if (element != null)
                element.SetActive(true);
        }

        if (mainMenu != null)
            mainMenu.SetActive(true);  // Tampilkan tombol Play Again

        // Tampilkan kursor saat Game Over
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0;  // Menghentikan permainan
    }
}
