using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileFollow : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private Transform targetLocation;
    private Vector3 projectilePosition;
    private Vector3 targetDirection;
    [SerializeField] private float projectileRotationSpeed;
    private float rotationStep;
    [SerializeField] private float collideRange;
    [SerializeField] private float desiredDurationOfProjectile;
    private float projectileDuration;

    void Awake()
    {
        projectilePosition = this.transform.position;
        //have bullet timer start
    }

    // Update is called once per frame
    void Update()
    {
        projectileDuration += Time.deltaTime;
        if (targetLocation != null)
        {
            targetDirection = targetLocation.position - transform.position;
            rotationStep = projectileRotationSpeed * Time.deltaTime;
            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(this.transform.forward, targetDirection, rotationStep, 0.0f);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            //Debug.Log("rotating projectile");
            transform.rotation = Quaternion.LookRotation(newDirection);

            //if reached target then destroy bullet game object
            if (Vector3.Distance(this.transform.position, targetLocation.position) <= collideRange)
            {
                //damage target
                Health targetsHealth = targetLocation.gameObject.GetComponent<Health>();
                if (targetsHealth != null)
                {
                    targetsHealth.takeDamage(1);
                }
                Destroy(this.gameObject);
            }
        }

        this.transform.position += this.transform.forward * projectileSpeed * Time.deltaTime;

        if (projectileExpired())
            {
               Destroy(this.gameObject);
            }
        
    }

    public void setTargetLocation(Transform target)
    {
        //Debug.Log("target is set");
        targetLocation = target;
    }

    private bool projectileExpired()
    {
        if (projectileDuration >= desiredDurationOfProjectile)
        {
            return true;
        }


        return false;
    }
}
