using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelMenu : MonoBehaviour
{
    public int level; // Level saat ini
    public int starPoint;
    public List<Image> stars;
    public Sprite fullStar;
    public Sprite emptyStar;
    public TPMovementRB playerScript;
    public AudioSource audioSource;
    
    public TextMeshProUGUI highScoreText; // Text untuk menampilkan high score

    private Timer timer;

    private void Start()
    {
        int minutes;
        int seconds;

        // Ambil data top score dari timer
        if (timer != null)
        {
            minutes = timer.topMinutes;
            seconds = timer.topSeconds;
            starPoint = timer.starPoint;
            foreach (Image img in stars)
            {
                img.sprite = emptyStar;
            }
            for (int i=0; i < starPoint; i++)
            {
                stars[i].sprite = fullStar;
            }
        }
        else
        {
            // Jika timer tidak ditemukan, gunakan nilai default atau dari PlayerPrefs sebagai cadangan
            minutes = PlayerPrefs.GetInt("TopMinutesScore", 0);
            seconds = PlayerPrefs.GetInt("TopSecondsScore", 0);
        }
        highScoreText.text = "Top Score \n" + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void Level1()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void Mainmenu()
    {
        SceneManager.LoadScene("MainMenu");
        //playerScript.enabled = true;
        //audioSource.enabled = true;
        Time.timeScale = 1;
    }
}
