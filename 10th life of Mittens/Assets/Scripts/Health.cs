using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    private int currentHealth;
    [SerializeField] private float iFrames = .5f;
    private float iTimer = 0f;



    public void takeDamage(int damage){
        currentHealth -= damage;
        if(iTimer >0){
            return;
        }
        else{
            Debug.Log("yeowch");
            if(currentHealth <= 0){
                Debug.Log("health is now 0, killing enemy with the WaveManager");
                if(WaveManager.Instance){
                    WaveManager.Instance.KillEnemy(gameObject);
                }
                //wave manager destroys the object and adds to score
            }
            iTimer = iFrames;
        }
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        iTimer = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(iTimer >=0){
            iTimer -= Time.fixedDeltaTime;
        }
    }
}
