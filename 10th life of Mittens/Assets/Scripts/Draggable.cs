using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    
    public void ToggleFixedPosition(bool posIsFixed)
    {
        if (posIsFixed)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }
}