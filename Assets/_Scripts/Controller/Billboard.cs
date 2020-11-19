using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool turnOnly;

    private Vector3 forward;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        if (turnOnly)
        {
            forward = new Vector3(Camera.main.transform.forward.x, transform.forward.y, Camera.main.transform.forward.z);
            Vector3 camPos = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z);

            if (forward != Vector3.zero)
            {
                // transform.forward = forward;
                transform.LookAt(camPos, Vector3.up);
            }
        }
        else
        {
            //transform.LookAt(Camera.main.transform.position, Vector3.up);
            transform.forward = Camera.main.transform.forward;
        }
    }
}