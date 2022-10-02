using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private int maxHealth;
    private int currentHealth;



    public void takeDamage(int damage){
        currentHealth -= damage;
        if(currentHealth <= 0){
            Debug.Log("health is now 0, killing enemy with the WaveManager");
            WaveManager.Instance.KillEnemy(gameObject);
            //wave manager destroys the object and adds to score
        }
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
