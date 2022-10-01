using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawnBehavior : MonoBehaviour
{
    public Vector3 startVel;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = startVel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
