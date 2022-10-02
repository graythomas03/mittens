using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public override void takeDamage(int damage)
    {
        currentHealth -= damage;
        if (iTimer > 0)
        {
            return;
        }
        else
        {
            //Debug.Log("yeowch");
            GameManager.Instance.LoseLife();
            if (currentHealth <= 0)
            {
                //Debug.Log("health is now 0, killing enemy with the WaveManager");
                if (WaveManager.Instance)
                {
                    // Game over
                }
            }
            iTimer = iFrames;
        }
    }

}
