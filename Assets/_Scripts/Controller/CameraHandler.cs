using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public PlayerController player;

    // private values
    private float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked) lookAtMouse();
    }

    private void lookAtMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * player.unit.stats.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * player.unit.stats.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);
    }
}
