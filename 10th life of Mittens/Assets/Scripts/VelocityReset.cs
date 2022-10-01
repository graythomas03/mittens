using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityReset : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
