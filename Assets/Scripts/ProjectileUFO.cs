using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileUFO : MonoBehaviour
{
    public float moveSpeed;

    private float speed;
    private float cameraSize;
    public virtual void Start()
    {
        cameraSize = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize;
        GetComponent<Rigidbody>().velocity = transform.rotation * Vector3.up * moveSpeed;
    }

    void FixedUpdate()
    {
        speed += GetComponent<Rigidbody>().velocity.magnitude;
        if (speed / 50 > cameraSize * 2)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
