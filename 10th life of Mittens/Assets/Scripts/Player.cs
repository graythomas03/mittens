using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    private Vector3 _dirVec;
    private PlayerAction _input;
    private Rigidbody _rbody;
    
    Draggable grabbedObj;
    private bool dragHeld = false;

    void Awake() {
        // define input system
        _input = new PlayerAction();
        _input.Player.Enable();
    
        _rbody = GetComponent<Rigidbody>();
        // make sure rbody was actuall init
        if(_rbody is null)
            Debug.LogError("Rigidbody is null");

        // init direction vector
        _dirVec = Vector3.zero;
    }

// update player movement every tick
    void FixedUpdate() {
        // read player movement vector
        var dirVec = _input.Player.Move.ReadValue<Vector2>();
        _dirVec.x = dirVec.x;
        _dirVec.z = dirVec.y;

        _rbody.velocity = _moveSpeed * _dirVec;

        // check for drag event
        _input.Player.Drag.performed += ctx => dragHeld = true;
        _input.Player.Drag.canceled += ctx => dragHeld = false;

        // verify space is held if dragging object
        if(grabbedObj != null && !dragHeld)
            ReleaseDraggable();
    }

    // Check to see if the player has initiated dragging an object
    private void OnCollisionStay(Collision collision)
    {
        if (grabbedObj == null)
        {
            // The player isn't currently dragging anything; a new drag is possible
            Draggable other = collision.collider.GetComponent<Draggable>();
            if (other != null && dragHeld)
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
