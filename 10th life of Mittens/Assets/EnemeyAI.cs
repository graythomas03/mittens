using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyAI : MonoBehaviour
{

    private UnityEngine.AI.NavMeshAgent Nav;
    public GameObject[] destination;
    public GenerateDestination AIDirection; 
    private int currentIndex = 0; 
    public GameObject player; 
    public float chaseDistance = 25;

    //Checks if AI is at desitation
    public bool atDesitnation(){
        //Checks distance
        float result = Vector3.Distance(Vector3.Scale(Nav.transform.position, new Vector3(1,0,1)), Vector3.Scale(destination[currentIndex].transform.position, new Vector3(1,0,1)));
        
        if(result < 0.2f){
            return true;            
        }
        return false;
    }


    // Start is called before the first frame update
    void Start()
    {
        destination = AIDirection.getDestination();
        Nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        Nav.SetDestination(destination[currentIndex].transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Checks distance
        if (atDesitnation()){
            //move to next destination
            currentIndex++;
            if(currentIndex >= destination.Length){
                currentIndex = 0;
            }
        }
        //check if already past destination so its not going backwards
        if(Nav.transform.position.z - destination[currentIndex].transform.position.z < -.5){
             //move to next destination
            currentIndex++;
            if(currentIndex >= destination.Length){
                currentIndex = 0;
            }
        }
        
        //distance from player to zombie
        float result = Vector3.Distance(Vector3.Scale(transform.position, new Vector3(1,0,1)), Vector3.Scale(player.transform.position, new Vector3(1,0,1)));

        //CHECK FOR TOO MANY ZOMBIES HERE

        if( WaveManager.Instance.CanChase(gameObject) && result < chaseDistance){
            //set destination to player
            Nav.SetDestination(player.transform.position);
            WaveManager.Instance.StartChasing(gameObject);
        }
        else{
            //Set new destination to marker
        Nav.SetDestination(destination[currentIndex].transform.position);
        }
       

    }
}
