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
            Destroy(this.gameObject);
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
