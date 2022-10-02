using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slowTower : MonoBehaviour
{
    private Vector3 trapPosition;
    [SerializeField] private int trapRange;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private float attackCoolDownTimer;

    void Awake()
    {
        trapPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject enemyInSight = checkEnemyInSight();
        cooldownTimer += Time.deltaTime;


        if (enemyInSight != null)
        {
            //the enemy in sight is the game object that stores the enemy
            slowEnemy();
        }
    }

    //returns true if any enemy is in the Overlap Sphere or if the player is on life 10 and in the sphere (as a zombie)
    GameObject checkEnemyInSight()
    {
        //returns a list of all colliders within trap range radius around trap position
        Collider[] hitColliders = Physics.OverlapSphere(trapPosition, trapRange);
        if (hitColliders != null)
        {
            foreach (Collider hitCollider in hitColliders)
            {

                if (hitCollider.CompareTag("Enemy"))
                {
                    //Debug.Log("enemy is in sight due to proper enemy tag");
                    return hitCollider.gameObject;
                }
            }
        }
        return null;
    }

        void slowEnemy()
    {
        //slows the enemy to 90%
        //EnemyMovement.setSpeed(90);
    }


    bool attackIsOffCooldown()
    {
        //returns true if the attack cooldown has been going longer than the total cooldown
        if (attackCoolDownTimer <= cooldownTimer)
        {
            cooldownTimer = 0;
            return true;
           
        }

        return false;
    }
}
