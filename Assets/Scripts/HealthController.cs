using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public GameObject scoreAndLifes;
    public int life;
    public int numberOfLifes;
    public Sprite fullLife;
    public Sprite emptyLife;
    public List<GameObject> lifes;
    [HideInInspector] public int currectLife;

    private GameManager gameManager;

    void Start()
    {
        CurrectLifeCounter();
        gameManager = FindObjectOfType<GameManager>();
    }

    public void CurrectLifeCounter()
    {
        currectLife = life;
        for (int i = 1; i <= numberOfLifes; i++)
        {
            lifes.Add(GameObject.Find(string.Format("Life {0}", i)));
        }
        for (int i = 0; i < lifes.Count; i++)
        {
            if (i > life - 1)
                lifes[i].SetActive(false);
        }
    }

    public void Update()
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
