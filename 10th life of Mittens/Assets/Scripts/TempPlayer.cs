using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float slowDownFactor;
    [SerializeField] private float walkTopSpeed;
    [SerializeField] private float pushBackStrength;
    private const float zeroClampVal = 0.01f;

    Rigidbody myRigidbody;

    KeyCode leftKey = KeyCode.LeftArrow;
    KeyCode rightKey = KeyCode.RightArrow;
    KeyCode upKey = KeyCode.UpArrow;
    KeyCode downKey = KeyCode.DownArrow;
    KeyCode grabKey = KeyCode.Space;
    KeyCode bombKey = KeyCode.E;

    [SerializeField] private GameObject dropBombPrefab;
    Draggable grabbedObj;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 appliedForce = new Vector2(0, 0);
        bool inputRead = false;

        if (Input.GetKey(leftKey))
        {
            appliedForce.x -= walkSpeed;
            inputRead = true;
        }
        if (Input.GetKey(rightKey))
        {
            appliedForce.x += walkSpeed;
            inputRead = true;
        }
        if (Input.GetKey(upKey))
        {
            appliedForce.y += walkSpeed;
            inputRead = true;
        }
        if (Input.GetKey(downKey))
        {
            appliedForce.y -= walkSpeed;
            inputRead = true;
        }

        float currentSpeed = myRigidbody.velocity.magnitude;
        float prospectiveSpeed = appliedForce.magnitude;
        Debug.Log("current speed:" + currentSpeed);
        Debug.Log("prospective speed:" + prospectiveSpeed);
        if (currentSpeed + prospectiveSpeed > walkTopSpeed)
        {
            Debug.Log("Before: " + appliedForce);
            appliedForce.Normalize();
            appliedForce *= walkTopSpeed - currentSpeed;
            Debug.Log("After: " + appliedForce);
        }

        Debug.Log(new Vector3(appliedForce.x, 0, appliedForce.y) / myRigidbody.mass);
        myRigidbody.AddForce(new Vector3(appliedForce.x, 0, appliedForce.y) / myRigidbody.mass);

        currentSpeed = myRigidbody.velocity.magnitude;
        myRigidbody.velocity = currentSpeed < zeroClampVal ? Vector3.zero : myRigidbody.velocity * slowDownFactor;

        // Let go of the currently dragged object if the key is not kept held
        if (grabbedObj != null && !Input.GetKey(grabKey))
        {
            ReleaseDraggable();
        }
    }

    // Check to see if the player has initiated dragging an object
    private void OnCollisionStay(Collision collision)
    {
        if (grabbedObj == null)
        {
            // The player isn't currently dragging anything; a new drag is possible
            Draggable other = collision.collider.GetComponent<Draggable>();
            if (other != null && Input.GetKey(grabKey))
            {
                // What the player has collided with is draggable, and the player is holding the grab key
                GrabDraggable(other, collision);
            }
        }
    }

    private void GrabDraggable(Draggable target, Collision collision)
    {
        grabbedObj = target;
        FixedJoint grabJoint = gameObject.AddComponent<FixedJoint>();
        grabJoint.anchor = collision.GetContact(0).point;
        grabJoint.connectedBody = collision.GetContact(0).otherCollider.transform.GetComponentInParent<Rigidbody>();
        grabJoint.enableCollision = false;
        target.ToggleFixedPosition(false);
    }

    private void ReleaseDraggable()
    {
        grabbedObj.ToggleFixedPosition(true);
        Destroy(GetComponent<FixedJoint>());
        grabbedObj = null;
    }
}
