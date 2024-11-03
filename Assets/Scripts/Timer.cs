using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public GameObject star;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalTimerText;
    public TextMeshProUGUI highScoreText;
    public float elapsedTime = 0;
    public int minutes;
    public int seconds;
    public int topMinutes;
    public int topSeconds;
    public int starPoint = 0;
    public float highScore;
    private string highScoreKey = "HighScore";
    public string topMinutesScore = "TopMinutesScore";
    public string topSecondsScore = "TopSecondsScore";
    public string currentScoreKey = "CurrentScore";
    public string currentStarKey = "CurrentStar";

    void Awake()
    {
        LoadHighScore();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60);
        seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        finalTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        
    }

    public void AddToScore()
    {
        if (elapsedTime < highScore)
        {
            highScore = elapsedTime;
            topMinutes = minutes;
            topSeconds = seconds;
            highScoreText.text = "Top Score : " + string.Format("{0:00}:{1:00}", topMinutes, topSeconds);

            SaveHighScore();
        }
    }

    public void SaveHighScore()
{
    PlayerPrefs.SetFloat(highScoreKey, highScore);
    PlayerPrefs.SetInt(topMinutesScore, topMinutes);
    PlayerPrefs.SetInt(topSecondsScore, topSeconds);

    // Award star points based on elapsed time
    if (topMinutes == 0 && topSeconds <= 30)
    {
        starPoint = 3; // Mendapatkan 3 bintang jika kurang dari 30 detik
    }
    else if (topMinutes < 1 && topSeconds >= 30)
    {
        starPoint = 2; // Mendapatkan 2 bintang jika kurang dari 1 menit
    }
    else if (topMinutes == 1 && topSeconds <= 30)
    {
        starPoint = 1; // Mendapatkan 1 bintang jika kurang dari 1 menit 30 detik
    }
    else
    {
        starPoint = 0; // Tidak mendapatkan bintang jika lebih dari 1 menit 30 detik
    }

    PlayerPrefs.SetInt(currentStarKey, starPoint);
    PlayerPrefs.Save();
    Debug.Log("High Score Saved: " + string.Format("{0:00}:{1:00}", topMinutes, topSeconds));
    Debug.Log("Star Points Saved: " + starPoint);
}

    

    public void LoadHighScore()
    {
        if (PlayerPrefs.HasKey(highScoreKey))
        {
            highScore = PlayerPrefs.GetFloat(highScoreKey);
            topMinutes = PlayerPrefs.GetInt(topMinutesScore);
            topSeconds = PlayerPrefs.GetInt(topSecondsScore);
            highScoreText.text = "Top Score : " + string.Format("{0:00}:{1:00}", topMinutes, topSeconds);

            // Load star points
            starPoint = PlayerPrefs.GetInt(currentStarKey); // Default to 0 if not set
            Debug.Log("High Score Loaded: " + string.Format("{0:00}:{1:00}", topMinutes, topSeconds));
            Debug.Log("Star Points Loaded: " + starPoint);
        }
        else
        {
            Debug.Log("No high score found.");
        }
    }

    public void PlayAgain()
    {
        elapsedTime = 0;
        minutes = 0;
        seconds = 0;
        PlayerPrefs.SetFloat(currentScoreKey, elapsedTime);
        PlayerPrefs.Save();
        Debug.Log("Game restarted. Timer reset.");
    }

    // Temporary method to clear all saved PlayerPrefs data
    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs data reset.");
    }
}
