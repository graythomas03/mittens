using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /**the wave manager */
    [SerializeField]private WaveManager waveMan;
    /**The life the cat is currently on. Starts at 1 and ends at 10*/
    [SerializeField]private int currentLife;



    // Start is called before the first frame update
    void Start()
    {
        waveMan = GetComponent<WaveManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void NewWave(){

    }

    public void Die(){
        NewWave();
    }

    
}
