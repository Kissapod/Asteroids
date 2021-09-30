using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int life;
    public int numberOfLifes;
    [HideInInspector] public GameObject[] lifes;
    public Sprite fullLife;
    public Sprite emptyLife;

    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        lifes = GameObject.FindGameObjectsWithTag("Life");
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            gameManager.GameOver();
        }
        if (life > numberOfLifes)
            life = numberOfLifes;
        for (int i = 0; i < lifes.Length; i++)
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
