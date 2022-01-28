using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Score score;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI scoreText;

    public void GameWon()
    {
        title.text = "Congratulations";
        ShowScore();
    }

    public void GameLost()
    {
        title.text = "Game Over!";
        ShowScore();
    }

    private void ShowScore()
    {
        scoreText.text = "Score:\n" + score.ScoreValue.ToString("000");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
