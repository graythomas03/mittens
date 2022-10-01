using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowTower : MonoBehaviour
{
    private Vector3 trapPosition;
    [SerializeField] private int trapRange;

    void Awake()
    {
        trapPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (checkEnemyInSight())
        {
            slowEnemy();
        }
    }

    //returns true if any enemy is in the Overlap Sphere or if the player is on life 10 and in the sphere (as a zombie)
    bool checkEnemyInSight()
    {
        //returns a list of all colliders within trap range radius around trap position
        Collider[] hitColliders = Physics.OverlapSphere(trapPosition, trapRange);
        if (hitColliders != null)
        {
            foreach (Collider hitCollider in hitColliders)
            {

                if (hitCollider.CompareTag("Enemy"))
                {
                    Debug.Log("enemy is in sight due to proper enemy tag");
                    return true;
                }
            }
        }
        return false;
    }

        void slowEnemy()
    {
        //slows the enemy to 90%
        //EnemyMovement.setSpeed(90);
    }
}
