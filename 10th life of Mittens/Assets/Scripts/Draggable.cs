using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [Tooltip("3 = heavy, 2 = medium, 1 = light")][Range(1,4)]public int weight = 1;
    private bool placed = false;

    void Start() {
        ToggleFixedPosition(true);
    }

    public void ToggleFixedPosition(bool posIsFixed)
    {
        if (posIsFixed)
        {
            Debug.Log("Called TFP true");
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            //placed = true;
        }
        else
        {
            Debug.Log("Called TFP false");
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        }
    }

    public bool isPlaced() {
        return placed;
    }

    public int getWeight(){
        return weight;
    }
}