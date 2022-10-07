using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayLightMovement : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float startAngle;
    [SerializeField] private float finalAngle;
    private float elapsedTime;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > duration)
        {
            elapsedTime = 0;
        }

        float angle = Mathf.Lerp(startAngle, finalAngle, elapsedTime/duration);
        Vector3 rotation = this.transform.rotation.eulerAngles;
        rotation.x = angle;

        this.transform.rotation = Quaternion.Euler(rotation);
    }
}
