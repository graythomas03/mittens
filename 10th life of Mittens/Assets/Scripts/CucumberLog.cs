using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CucumberLog : MonoBehaviour
{
    //private const float zeroClampVal = 0.01f;

    Rigidbody myRigidbody;
    [SerializeField] private Vector3 _velocity;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        myRigidbody.velocity = _velocity;
    }
}
