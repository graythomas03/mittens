using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;
    private Vector3 _dirVec;
    private PlayerAction _input;
    private Rigidbody _rbody;

    Vector3 pos_vec;    // unit vector controlling direction player should move

    void Awake() {
        // define input system
        _input = new PlayerAction();
    
        _rbody = GetComponent<Rigidbody>();
        // make sure rbody was actuall init
        if(_rbody is null)
            Debug.LogError("Rigidbody is null");

        // init direction vector
        _dirVec = Vector3.zero;
    }

    void FixedUpdate() {
        // read player movement vector
        var dirVec = _input.Player.Move.ReadValue<Vector2>();
        _dirVec.x = dirVec.x;
        _dirVec.z = dirVec.y;

        _rbody.velocity = _moveSpeed * _dirVec;
    }

    // functions for enabling player actionmap
    void OnEnable() {
        _input.Player.Enable();
    }
    void OnDisable() {
        _input.Player.Disable();
    }

    // draggable interface stand-in
    private interface Draggable {

    }
}
