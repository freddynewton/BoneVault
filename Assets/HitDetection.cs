using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitDetection : MonoBehaviour
{
    [HideInInspector] public bool isInTouch;

    public void OnTriggerStay (Collider collider) {
        if (collider.gameObject.tag == "Enemy") {
            Debug.Log("Entered Object: " + collider);
            isInTouch = true;
        }
        else isInTouch = false;
    }
}