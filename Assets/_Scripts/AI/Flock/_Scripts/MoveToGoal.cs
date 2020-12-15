using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToGoal : MonoBehaviour
{
    public Transform Target;
    public float SpaceBetween = 1.5f;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(Target.position, transform.position) >= SpaceBetween)
        {
            Vector3 direction = Target.position - transform.position;
            transform.Translate(direction * Time.deltaTime);
        }
    }
}
