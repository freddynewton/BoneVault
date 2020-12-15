using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIAvoidance : MonoBehaviour
{

    GameObject[] otherAgents;
    public float SpaceBetween = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        otherAgents = GameObject.FindGameObjectsWithTag("AI");
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject go in otherAgents)
        {
            if(go != gameObject)
            {
                float distance = Vector3.Distance(go.transform.position, transform.position);
                if (distance <= SpaceBetween)
                {
                    Vector3 direction = transform.position - go.transform.position;
                    transform.Translate(direction * Time.deltaTime);
                }
            }
        }
    }
}
