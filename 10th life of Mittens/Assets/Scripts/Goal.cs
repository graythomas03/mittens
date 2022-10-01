using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public string enemyTag = "enemy";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col){
        Debug.Log("collision");
        if(col.gameObject.tag == enemyTag){
            Debug.Log("collision with enemy");
            WaveManager.Instance.LoseWave();
        }
    }
}
