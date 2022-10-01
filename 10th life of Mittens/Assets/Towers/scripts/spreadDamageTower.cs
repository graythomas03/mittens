using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spreadDamageTower : MonoBehaviour
{
    private float[] angleList = new float[] {15f,30f,45f,60f,75f};
    private float coolDown;
    [SerializeField] private float cooldownTimerFast;
    [SerializeField] private float cooldownTimerSlow;
    private float cooldownTimer;
    private Vector3 towerPosition;
    private int currentIndex;
    private bool flipIndexCount;
    [SerializeField] private projectileFollow projectilePrefab;

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //to mimick sprinkler shooting, I want 2 different states
        //1) slow shots while turning ~10-20 degrees each time
        //2) fast shots while turning backto original location
        coolDown += Time.deltaTime;


        Debug.Log("sprinkler going off, going on " + angleList[currentIndex] + " degrees");
        if (attackIsOffCooldown())
        {
            changeOfIndex();
            Debug.Log("sprinkler off cooldown, changing angle now");
            this.transform.rotation = Quaternion.Euler(0,angleList[currentIndex],0);
            shootBullet();
        } 

    }

    bool attackIsOffCooldown()
    {
        //returns true if the attack cooldown has been going longer than the total cooldown
        if (coolDown >= cooldownTimer)
        {
            coolDown = 0;
            return true;
            
        }

        return false;
    }

    private void changeOfIndex()
    {
        if (flipIndexCount)
        {
            currentIndex++;
        }
        else
        {
            currentIndex--;
        }
        
        if (currentIndex >= angleList.Length)
        {
            currentIndex = angleList.Length - 1;
            flipIndexCount = !flipIndexCount;
            cooldownTimer = cooldownTimerFast;
        }
        else if (currentIndex < 0)
        {
            currentIndex = 0;
            flipIndexCount = !flipIndexCount;
            cooldownTimer = cooldownTimerSlow;
        }

    }

    void shootBullet()
    {
        Debug.Log("shoot Bullet method has been called by sprinkler");
        projectileFollow newProjectile = GameObject.Instantiate(projectilePrefab, this.transform.position, Quaternion.identity);
        newProjectile.gameObject.transform.rotation = transform.rotation;
        
    }
}
