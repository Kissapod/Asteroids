using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int life;
    public int numberOfLifes;
    public Sprite fullLife;
    public Sprite emptyLife;
    public List<GameObject> lifes;

    private GameManager gameManager;

    void Start()
    {
        lifes.Add(GameObject.Find("Life 1"));
        lifes.Add(GameObject.Find("Life 2"));
        lifes.Add(GameObject.Find("Life 3"));
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (life <= 0)
        {
            gameManager.GameOver();
        }
        if (life > numberOfLifes)
            life = numberOfLifes;
        for (int i = 0; i < lifes.Count; i++)
        {
            if (i < life)
                lifes[i].GetComponent<Image>().sprite = fullLife;
            else
                lifes[i].GetComponent<Image>().sprite = emptyLife;
            if (i < numberOfLifes)
                lifes[i].GetComponent<Image>().enabled = true;
            else
                lifes[i].GetComponent<Image>().enabled = false;
        }
    }
}
