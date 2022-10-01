using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour
{
    // Factor (from 0 - 1) to multiply the velocity of the object by every frame
    [SerializeField] private float factor;
    private const float zeroClampVal = 0.01f;

    public void FixedUpdate()
    {
        Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;
        float currentSpeed = currentVelocity.magnitude;
        GetComponent<Rigidbody>().velocity = currentSpeed < zeroClampVal ? Vector3.zero : currentVelocity * factor;
    }
}
