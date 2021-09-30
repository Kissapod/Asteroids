using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float rotationOffset = -90f;
    public GameObject gun;
    public GameObject projectile;
    public float invulnerabilityTime;
    public AudioClip fireSound;
    public AudioClip thrustSound;
    public AudioClip explosionSound;

    private MeshRenderer[] bodyShip;
    private GameManager gameManager;
    private Rigidbody rgdbody;
    private float fireTimer;
    private AudioSource audioSource;
    // Start is called before the first frame update

    void Start()
    {
        bodyShip = GetComponentsInChildren<MeshRenderer>();
        gameManager = FindObjectOfType<GameManager>();
        rgdbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Blinking(bodyShip));
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        if (GameManager.controlKeyword)
        {
            KeyControl();
        }
        else
        {
            MouseAndKeyControl();
        }
    }

    private void FixedUpdate()
    {
        rgdbody.velocity = Vector3.ClampMagnitude(rgdbody.velocity, maxSpeed);
    }

    private void MouseAndKeyControl()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            InvokeRepeating("VelocityUpgrade", 0.000001f, 0.01f);
            AudioSource.PlayClipAtPoint(thrustSound, transform.position);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            CancelInvoke("VelocityUpgrade");
        }
        if (Time.timeScale > 0 && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space)))
            Fire();
        RotationShip(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    private void KeyControl()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            InvokeRepeating("VelocityUpgrade", 0.000001f, 0.01f);
            audioSource.Play();
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            CancelInvoke("VelocityUpgrade");
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.forward * rotationSpeed * 100 * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.forward * rotationSpeed * -100 * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space) && Time.timeScale > 0)
            Fire();
    }

    void RotationShip(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //расчитываем угол поворота врага
        Quaternion rotation = Quaternion.AngleAxis(rotZ + rotationOffset, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    void VelocityUpgrade()
    {
        rgdbody.velocity += transform.rotation * Vector3.up * moveSpeed * 0.01f;
    }

    void Fire()
    {
        if (fireTimer > 0.33f)
        {
            fireTimer = 0;
            GameObject createProjectile = Instantiate(projectile, gun.transform.position, Quaternion.identity, gameManager.objectPool.transform);
            createProjectile.GetComponent<Rigidbody>().velocity = transform.rotation * Vector3.up * moveSpeed * 2;
            gameManager.destroyObject.Add(createProjectile);
            AudioSource.PlayClipAtPoint(fireSound, transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<HealthController>().life--;
        AudioSource.PlayClipAtPoint(thrustSound, transform.position);
        Destroy(other.gameObject);
        StartCoroutine(Blinking(bodyShip));
    }

    IEnumerator Blinking(MeshRenderer[] bodyShip)
    {
        transform.position = Vector3.zero;
        GetComponent<BoxCollider>().enabled = false;
        float timer = 0;
        float invulnerabilityTimer = 0.25f;
        bool invisible = false;
        while (timer < invulnerabilityTime)
        {
            timer += Time.deltaTime;
            if (timer > invulnerabilityTimer)
            {
                invulnerabilityTimer += 0.25f;
                if (!invisible)
                {
                    foreach (MeshRenderer body in bodyShip)
                        body.enabled = false;
                    invisible = true;
                }
                else
                {
                    foreach (MeshRenderer body in bodyShip)
                        body.enabled = true;
                    invisible = false;
                }
            }
            yield return null;
        }
        foreach (MeshRenderer body in bodyShip)
            body.enabled = true;
        GetComponent<BoxCollider>().enabled = true;
        yield break;
    }
}
