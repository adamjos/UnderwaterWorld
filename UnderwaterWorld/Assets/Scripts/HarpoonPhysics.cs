using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonPhysics : MonoBehaviour
{

    public float moveSpeed = 25f;

    public void Setup (Vector3 shootDir)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(shootDir * moveSpeed, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }
}
