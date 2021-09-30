using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject continueButton;
    public Text controlButtonText;
    public GameObject objectPool;
    public GameObject asteroid;
    public GameObject UFO;
    public float speedUFO;
    public GameObject player;
    [HideInInspector] public List<GameObject> destroyObject;
    public static bool controlKeyword;

    private int asteroidCounter;
    private bool asteroidSpawn;
    private bool startGame;
    private float timer;
    private HealthController lifePlayer;
    private Vector3 leftBot;
    private Vector3 rightTop;
    private float timeCreateUFO;
    private float cameraSize;

    void Start()
    {
        controlKeyword = true;
        leftBot = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        rightTop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
        continueButton.SetActive(false);
        startGame = false;
        cameraSize = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize;
        timeCreateUFO = Random.Range(20f, 40f);
        if (speedUFO <= 0)
            speedUFO = 0.25f;
    }

    public void NewGame()
    {
        continueButton.SetActive(true);
        menu.SetActive(false);
        asteroidCounter = 1;
        asteroidSpawn = false;
        if (!startGame)
        {
            PlayerSpawner();
            AsteroidSpawner();
            startGame = true;
        }
        else
        {
            ResetGame();
            PlayerSpawner();
            AsteroidSpawner();
        }
    }

    private void ResetGame()
    {
        for (int i = 0; i <= destroyObject.Count - 1; i++)
        {
            Destroy(destroyObject[i]);
        }
        destroyObject.Clear();
        lifePlayer.life = 3;
        FindObjectOfType<ScoreKeeper>().ResetScore();
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        startGame = false;
        menu.SetActive(true);
        continueButton.SetActive(false);
        asteroidCounter = 1;
        asteroidSpawn = false;
        ResetGame();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
    }


    void LateUpdate()
    {
        if (startGame)
        {
            timer += Time.deltaTime;
            if (destroyObject.Count > 0)
                destroyObject.RemoveAll(x => x == null);
            GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
            
            if (asteroids.Length == 0 && asteroidSpawn)
            {
                asteroidSpawn = false;
                Invoke(nameof(AsteroidSpawner), 2f);
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
                menu.SetActive(true);
            }
            
            if (timer >= timeCreateUFO)
            {
                timer = 0;
                timeCreateUFO = Random.Range(20f, 40f);
                SpawnerUFO();
            }
        }
    }

    void AsteroidSpawner()
    {
        for (int i = 0; i <= asteroidCounter; i++)
        {
            Vector3 randomPositionVector = new Vector3(Random.Range(leftBot.x, rightTop.x), Random.Range(rightTop.y, leftBot.y), 0);
            GameObject createAsteroid = Instantiate(asteroid, randomPositionVector, Quaternion.Euler(0, 0, Random.Range(0, 359)), objectPool.transform);
            destroyObject.Add(createAsteroid);
        }
        asteroidCounter++;
        asteroidSpawn = true;
    }

    void PlayerSpawner()
    {
        GameObject createPlayer = Instantiate(player, Vector3.zero, Quaternion.identity);
        lifePlayer = createPlayer.GetComponent<HealthController>();
        destroyObject.Add(createPlayer);
    }

    void SpawnerUFO()
    {
        int randomSide = Random.Range(0, 2);
        Vector3 direction;
        float side;
        if (randomSide == 0)
        {
            side = leftBot.x+0.001f;
            direction = Vector3.right;
        }
        else
        {
            side = rightTop.x-0.001f;
            direction = Vector3.left;
        }
        Vector3 randomPositionVector = new Vector3(side, Random.Range(cameraSize * -0.8f, cameraSize*0.8f), 0);
        GameObject createUFO = Instantiate(UFO, randomPositionVector, Quaternion.identity, objectPool.transform);
        createUFO.GetComponent<Rigidbody>().AddForce(direction * speedUFO, ForceMode.Impulse);
        destroyObject.Add(createUFO);
    }

    public void ChangeControl()
    {
        if (controlKeyword)
        {
            controlButtonText.text = "Управление: клавиатура + мышь";
            controlKeyword = false;
        }
        else
        {
            controlButtonText.text = "Управление: клавиатура";
            controlKeyword = true;
        }
    }
    public void QuitGame()
    {
        Debug.Log("Quit requested");
        Application.Quit();
    }
}
