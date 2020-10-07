using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Billboard : MonoBehaviour
{
    public bool turnOnly;

    private Vector3 forward;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate () {
        if (turnOnly) {
            forward = new Vector3(-Camera.main.transform.forward.x, transform.forward.y, transform.forward.z);
        }
        else {
            forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);          
        }

        if (forward != Vector3.zero) {
            transform.forward = forward;
        }
    }
}
