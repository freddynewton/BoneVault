using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Vector3 forward;


    private void LateUpdate () {
        forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);

        if (forward != Vector3.zero) {
            transform.forward = forward;
        }
    }
}
