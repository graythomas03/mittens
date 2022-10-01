using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    private bool placed = false;

    public void ToggleFixedPosition(bool posIsFixed)
    {
        if (posIsFixed)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            placed = true;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }

    public bool isPlaced() {
        return placed;
    }
}