using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDestination : MonoBehaviour
{

    public GameObject[] destination1;
    public GameObject[] destination2;
    public GameObject[] destination3;
    
    public GameObject[] getDestination(){
        GameObject[] finalDestinaiton = {destination1[Random.Range(0, destination1.Length)],
            destination2[Random.Range(0, destination2.Length)],destination3[Random.Range(0, destination3.Length)]};
        return finalDestinaiton;
    }
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
