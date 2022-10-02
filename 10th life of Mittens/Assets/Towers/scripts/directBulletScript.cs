using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directBulletScript : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private int trapRange;
    private bool playerInRange;
    private Vector3 trapPosition;
    private float cooldownTimer = Mathf.Infinity;
    private float rotationTowardsEnemy;
    [SerializeField] private float attackCoolDownTimer;
    [SerializeField] private projectileFollow projectilePrefab;
    private Transform enemyLocation;
    [SerializeField] private towerRotation towerROT;
    private SoundFX TennisBallLaunch;

    void Awake()
    {
        towerROT = this.gameObject.GetComponent<towerRotation>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;
        GameObject enemyInSight = checkEnemyInSight();
        
        towerROT.setEnemyLocation(enemyInSight);
        //checks if player is in sight
        if (enemyInSight != null)
        {
            //Debug.Log("enemy is in sight, now checking if attack is off cooldown");
            if (attackIsOffCooldown())
            {
                //Debug.Log("attack is off cooldown, now running the damage Enemy method");
                //so the enemy will not be null at this poiont so we can shoot bullet and get the target
                shootBullet(enemyInSight.transform);
            }
        }
    }

    //returns true if any enemy is in the Overlap Sphere or if the player is on life 10 and in the sphere (as a zombie)
    GameObject checkEnemyInSight()
    {
        //returns a list of all colliders within trap range radius around trap position
        Collider[] hitColliders = Physics.OverlapSphere(trapPosition, trapRange);
        if (hitColliders != null)
        {
            //Debug.Log("hit colliders is not null, now parsing through each collider in hitCollider");
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


    void damageEnemy()
    {
        //damages the enemy
        //EnemyHealth.TakeDamage(damage);
    }

    private void shootBullet(Transform shootPosition)
    {
        projectileFollow newProjectile = GameObject.Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
        newProjectile.setTargetLocation(shootPosition);
        newProjectile.SetDamage(damage);
        //here is where the sound for shooting tennis ball should be played
        SoundManager.Instance.PlayOnce(SoundFX.TennisBallLaunch);
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
