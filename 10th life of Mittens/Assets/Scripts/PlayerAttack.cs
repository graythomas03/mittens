using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timer = 5f;
    private string attackTag;
    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupAttack(float time, string enemy, int damge){
        //Debug.Log("set up attack");
        timer = time;
        attackTag = enemy;
        damage = damge;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer < 0f){
            gameObject.SetActive(false);
        }
        timer-=Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider col){
        Debug.Log("collision");
        if(col.gameObject.tag == attackTag){
            Debug.Log("enemy collision");
            Health enemyHealth = col.gameObject.GetComponent<Health>();
            if(enemyHealth){
                enemyHealth.takeDamage(damage);
            }
        }
    }
}
