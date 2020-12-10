using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public PlayerController player;

    // private values
    private float xRotation = 0f;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked) lookAtMouse();
    }

    private void lookAtMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * player.unit.playerStats.mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * player.unit.playerStats.mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.transform.Rotate(Vector3.up * mouseX);
    }
}