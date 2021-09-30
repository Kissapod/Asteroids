using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
    public int score;
    public GameObject projectile;
    public float rotationProjectileOffset = -90f;
    public AudioClip fireSound;
    public AudioClip explosionSound;

    private GameManager gameManager;
    private float timer;
    private float timerFire;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        timerFire = Random.Range(2f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timerFire)
        {
            timer = 0;
            timerFire = Random.Range(2f, 5f);
            Fire();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Projectile>())
        {
            FindObjectOfType<ScoreKeeper>().Score(score);
        }
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);
        Destroy(gameObject);
    }

    void Fire()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        Vector3 direction = (player.transform.position- transform.position).normalized; 
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(rotZ + rotationProjectileOffset, Vector3.forward);
        GameObject createProjectile = Instantiate(projectile, transform.position, Quaternion.identity, gameManager.objectPool.transform);
        createProjectile.GetComponent<Rigidbody>().velocity = rotation * Vector3.up * player.moveSpeed * 2;
        gameManager.destroyObject.Add(createProjectile);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
}
