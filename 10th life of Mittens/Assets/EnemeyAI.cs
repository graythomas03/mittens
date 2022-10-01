using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyAI : MonoBehaviour
{

    private UnityEngine.AI.NavMeshAgent Nav;
    public GameObject[] destination;
    private int currentIndex = 0; 

    public bool atDesitnation(){
        float result = Vector3.Distance(Vector3.Scale(Nav.transform.position, new Vector3(1,0,1)), Vector3.Scale(destination[currentIndex].transform.position, new Vector3(1,0,1)));
        if(result < 0.1f){
            return true;            
        }
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Nav.SetDestination(destination[currentIndex].transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (atDesitnation()){
            currentIndex++;
            if(currentIndex >= destination.Length){
                currentIndex = 0;
            }
        }
        Nav.SetDestination(destination[currentIndex].transform.position);

    }
}
