using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerRotation : MonoBehaviour
{
    private Transform towerLocation;
    [SerializeField] private float towerRotationSpeed;
    [SerializeField] private int towerRange;
    private GameObject enemyLocation;
    private float rotationStep;
    private Vector3 targetDirection;

    public GameObject heading;

    void Awake()
    {
        towerLocation = this.gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyLocation != null)
        {
            targetDirection = enemyLocation.transform.position - towerLocation.position;
            targetDirection.y = 0;

            var rotation = Quaternion.LookRotation(targetDirection);

            heading.transform.rotation = Quaternion.Slerp(heading.transform.rotation, rotation, Time.deltaTime * towerRotationSpeed);

        }
    }


    public void setEnemyLocation(GameObject enemy)
    {
        enemyLocation = enemy;
    }
}
