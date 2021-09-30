using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float distance;

    private void Update()
    {
        if (distance > Screen.width/2) 
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        distance += GetComponent<Rigidbody>().velocity.magnitude;
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
