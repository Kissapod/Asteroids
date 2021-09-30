using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static int score = 0;
    private Text scoreText;

    void Start()
    {
        scoreText = GetComponent<Text>();
        ResetScore();
    }
    public void Score(int points)
    {
        score += points;
        scoreText.text = score.ToString();

    }
    public void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }
}
