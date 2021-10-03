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
    public int amountStartAsteroids = 2;
    public float timeSpawnAsteroids = 2f;
    public GameObject UFO;
    public float minTimeSpawnUFO = 20f;
    public float maxTimeSpawnUFO = 40f;
    public float speedUFO;
    public GameObject player;
    public static bool controlKeyword;
    [HideInInspector] public List<GameObject> poolObject;

    private int asteroidCounter;
    private bool asteroidSpawn;
    private bool startGame;
    private float timer;
    private HealthController lifePlayer;
    private Vector3 leftBot;
    private Vector3 rightTop;
    private float timeCreateUFO;
    private float cameraSize;
    private AudioListener audioList;

    void Start()
    {
        audioList = GameObject.Find("Main Camera").GetComponent<AudioListener>();
        controlKeyword = true;
        leftBot = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        rightTop = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
        continueButton.SetActive(false);
        startGame = false;
        cameraSize = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize;
        timeCreateUFO = Random.Range(minTimeSpawnUFO, maxTimeSpawnUFO);
        lifePlayer = GetComponent<HealthController>();
        lifePlayer.scoreAndLifes.SetActive(false);
        lifePlayer.enabled = false;
        if (speedUFO <= 0)
            speedUFO = 0.25f;
    }

    public void NewGame()
    {
        audioList.enabled = true;

        lifePlayer.enabled = true;
        lifePlayer.scoreAndLifes.SetActive(true);
        
        continueButton.SetActive(true);
        menu.SetActive(false);
        
        asteroidCounter = amountStartAsteroids;
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
            lifePlayer.enabled = true;
            lifePlayer.scoreAndLifes.SetActive(true);
            PlayerSpawner();
            AsteroidSpawner();
        }
    }

    private void ResetGame()
    {
        for (int i = 0; i <= poolObject.Count - 1; i++)
        {
            Destroy(poolObject[i]);
        }
        poolObject.Clear();
        timer = 0;
        lifePlayer.life = lifePlayer.currectLife;
        FindObjectOfType<ScoreKeeper>().ResetScore();
        lifePlayer.scoreAndLifes.SetActive(false);
        lifePlayer.enabled = false;
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        startGame = false;
        menu.SetActive(true);
        continueButton.SetActive(false);
        asteroidCounter = amountStartAsteroids;
        asteroidSpawn = false;
        ResetGame();
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        audioList.enabled = true;
        menu.SetActive(false);
    }


    void LateUpdate()
    {
        if (startGame)
        {
            timer += Time.deltaTime;
            if (poolObject.Count > 0)
                poolObject.RemoveAll(x => x == null);
            GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
            
            if (asteroids.Length == 0 && asteroidSpawn)
            {
                asteroidSpawn = false;
                Invoke(nameof(AsteroidSpawner), timeSpawnAsteroids);
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                audioList.enabled = false;
                Time.timeScale = 0;
                menu.SetActive(true);
            }
            
            if (timer >= timeCreateUFO)
            {
                timer = 0;
                timeCreateUFO = Random.Range(minTimeSpawnUFO, maxTimeSpawnUFO);
                SpawnerUFO();
            }
        }
    }

    void AsteroidSpawner()
    {
        for (int i = 0; i < asteroidCounter; i++)
        {
            Vector3 randomPositionVector = new Vector3(Random.Range(leftBot.x, rightTop.x), Random.Range(rightTop.y, leftBot.y), 0);
            GameObject createAsteroid = Instantiate(asteroid, randomPositionVector, Quaternion.Euler(0, 0, Random.Range(0, 359)), objectPool.transform);
            poolObject.Add(createAsteroid);
        }
        asteroidCounter++;
        asteroidSpawn = true;
    }

    void PlayerSpawner()
    {
        GameObject createPlayer = Instantiate(player, Vector3.zero, Quaternion.identity);
        poolObject.Add(createPlayer);
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
        poolObject.Add(createUFO);
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
