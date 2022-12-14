using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateDestination : MonoBehaviour
{
    public GameObject[] lanes;

    public GameObject finalDestination;

    private void Awake()
    {

        for (int i = 0; i < lanes.Length; i++)
        {
            foreach(Transform child in lanes[i].transform)
            {
                var meshRenderer = child.GetComponent<MeshRenderer>();
                if(meshRenderer != null)
                {
                    meshRenderer.enabled = false;
                }
            }
        }
    }
    public GameObject[] getDestination()
    {
        List<GameObject> result = new List<GameObject>();

        for(int i = 0; i < lanes.Length; i++)
        {
            var childCount = lanes[i].transform.childCount;
            var randomChildIndex = Random.Range(0, childCount);
            var location = lanes[i].transform.GetChild(randomChildIndex);

            result.Add(location.gameObject);
        }

        result.Add(finalDestination);

        return result.ToArray();
    }
}
