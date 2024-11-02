using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCollections : MonoBehaviour
{
    private int Coin = 0;
    public TextMeshProUGUI coinText;
    
    private GameObject currentCoin; // Menyimpan referensi koin yang saat ini bisa diambil
    public GameObject intText; // UI untuk teks interaksi

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            intText.SetActive(true);
            currentCoin = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Coin"))
        {
            intText.SetActive(false);
            currentCoin = null;
        }
    }

    private void Update()
    {
        if (currentCoin != null && Input.GetKeyDown(KeyCode.E))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        Coin++;
        coinText.text = "Coin: " + Coin.ToString();

        Destroy(currentCoin);
        intText.SetActive(false);
        currentCoin = null;
    }
}
