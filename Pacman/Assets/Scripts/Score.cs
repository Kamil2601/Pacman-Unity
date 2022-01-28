using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private int score = 0;
    private Text text;

    public int ScoreValue { get => score; }

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable() 
    {
        Dots.dotEaten += DotEaten;
        Ghost.ghostEaten += GhostEaten;
    }

    private void OnDisable()
    {
        Dots.dotEaten -= DotEaten;
        Ghost.ghostEaten -= GhostEaten;
    }

    private void GhostEaten()
    {
        UpdateScore(200);
    }

    private void DotEaten()
    {
        UpdateScore(10);
    }

    private void UpdateScore(int points)
    {
        score += points;

        text.text = score.ToString("000");
    }
}
