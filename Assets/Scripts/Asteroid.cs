using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour
{
    public int score;
    public GameObject asteroid;
    public float moveSpeed;
    public float degreeSpawnAsteroids = 45f;
    public bool childAsteroid;
    public AudioClip explosionSound;

    private GameManager gameManager;
    private float currectSpeed;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (!childAsteroid)
            currectSpeed = Random.Range(moveSpeed - 1, moveSpeed + 1);
        else
            currectSpeed = moveSpeed;
    }

    void Update()
    {
        transform.Translate(Vector3.up * currectSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>())
        {
            if (asteroid != null)
            {
                float speedCreateAsteroid = Random.Range(moveSpeed - 1, moveSpeed + 1);
                AsteroidSpawner(degreeSpawnAsteroids, speedCreateAsteroid);
                AsteroidSpawner(-degreeSpawnAsteroids, speedCreateAsteroid);
            }
            FindObjectOfType<ScoreKeeper>().Score(score);
        }
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        Destroy(gameObject);
    }

    void AsteroidSpawner(float gradus, float speed)
    {
        GameObject createAsteroid = Instantiate(asteroid, transform.position, transform.rotation, gameManager.objectPool.transform);
        createAsteroid.transform.Rotate(0, 0, gradus);
        Asteroid asteroidScript = createAsteroid.GetComponent<Asteroid>();
        asteroidScript.moveSpeed = speed;
        gameManager.poolObject.Add(createAsteroid);
    }
}
