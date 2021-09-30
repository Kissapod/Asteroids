using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float cameraSize;
    public virtual void Start()
    {
        cameraSize = GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize;
    }

    void FixedUpdate()
    {
        speed += GetComponent<Rigidbody>().velocity.magnitude;
        if (speed / 50 > cameraSize*2)
            Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
