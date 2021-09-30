using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public static int score = 0;
    private Text myText;
    // Start is called before the first frame update

    void Start()
    {
        myText = GetComponent<Text>();
        ResetScore();
    }
    public void Score(int points)
    {
        score += points;
        myText.text = score.ToString();

    }
    public void ResetScore()
    {
        score = 0;
        myText.text = score.ToString();
    }
}
