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
    Droppable heldObj;

    [SerializeField] 
    private GameObject sprite;
    private Animator anim;

    private bool dragEvent = false;
    private bool swipeEvent = false;

    private string enemyTag;    // tag of GameObjects player should be able to attack

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

    void Start() {
        this.tag = "Player";
        enemyTag = "Enemy";

        anim = sprite.GetComponent<Animator>();
    }

// update player movement every tick
    void FixedUpdate() {
        // read player movement vector
        var tmp = _input.Player.Move.ReadValue<Vector2>();
        Vector3 dirVec = new Vector3(tmp.x, 0, tmp.y);

        if(dirVec != Vector3.zero) {
            // update palyer sprite rotation
            sprite.transform.rotation.SetFromToRotation(_dirVec, dirVec);

            anim.SetBool("moving", true);
        } else {
            anim.SetBool("moving", false);
        }

        _rbody.velocity = _moveSpeed * _dirVec;

        // check for swipe event
        _input.Player.Swipe.performed += ctx => swipeEvent = true;
        _input.Player.Swipe.canceled += ctx => swipeEvent = false;

        // check for drag event (only if player)
        if(this.tag.Equals("Player")) {
            _input.Player.Drag.performed += ctx => dragEvent = true;
            _input.Player.Drag.canceled += ctx => dragEvent = false;
        }

        // verify space is held if dragging object
        if(grabbedObj != null && !dragEvent)
            ReleaseDraggable();

        // swipe event operations
        if(swipeEvent) {
            // if holding object, drop
            if(heldObj != null) {
                heldObj.Drop();
                heldObj = null;
            } else {
                // player attack code
            }
        }
    }

/** GAMEMANAGER METHODS **/
    public void changeSide() {
        this.tag = "Enemy";
        enemyTag = "Player";    // or whichever tag turret bullets use
    }

/** INTERNAL METHODS **/

    // Check to see if the player has initiated dragging an object
    private void OnCollisionStay(Collision collision)
    {
        if (dragEvent && grabbedObj == null)
        {
            // The player isn't currently dragging anything; a new drag is possible
            Draggable other = collision.collider.GetComponent<Draggable>();
            if (other != null)
                GrabDraggable(other, collision);
        }

        // if player attempting to hold object and there is no object already being held
        if(swipeEvent && heldObj == null) {
            Droppable obj = collision.collider.GetComponent<Droppable>();
            if(obj != null)
                heldObj = obj;
        }

        // if player hits enemy
        GameObject enemy = collision.collider.gameObject;
        if(enemy.tag.Equals("Enemy") && gameObject.tag.Equals("Player")) {
            WaveManager.Instance.DamagePlayer();
        }
    }

    private void GrabDraggable(Draggable target, Collision collision)
    {
        // make sure player cant move already placed objects
        Debug.Log(target.isPlaced());
        if(target.isPlaced())
            return;
        int weight = target.getWeight();
        int strength = (11 - GameManager.Instance.GetLife()) / 3;
        if( weight > strength){
            Debug.Log("too weak to move haha lmao");
            return;
        }
        grabbedObj = target;
        FixedJoint grabJoint = gameObject.AddComponent<FixedJoint>();
        grabJoint.anchor = collision.GetContact(0).point;
        grabJoint.connectedBody = collision.GetContact(0).otherCollider.transform.GetComponentInParent<Rigidbody>();
        grabJoint.enableCollision = false;
        target.ToggleFixedPosition(false);

        anim.SetBool("dragging", true);
    }

    private void ReleaseDraggable()
    {
        grabbedObj.ToggleFixedPosition(true);
        Destroy(GetComponent<FixedJoint>());
        grabbedObj = null;

        anim.SetBool("dragging", false);
    }
}
