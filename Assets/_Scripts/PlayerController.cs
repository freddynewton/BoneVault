using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public PlayerUnit unit;
    [HideInInspector] public CharacterController controller;


    private Vector3 velocity;

    private void Start()
    {
        unit = GetComponent<PlayerUnit>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        movement();
    }

    private void movement()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        Vector3 groundVec = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - gameObject.transform.localScale.y, gameObject.transform.position.z);

        bool isGrounded = Physics.CheckSphere(groundVec, unit.stats.groundDistance, unit.stats.groundMask);

        controller.Move(move * unit.stats.movementSpeed * Time.deltaTime);

        Debug.Log(isGrounded);
        

        // Ground Check
        if (isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(unit.stats.jumpHeight * -2f * unit.stats.gravity);
        }

        velocity.y += unit.stats.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - gameObject.transform.localScale.y, gameObject.transform.position.z), 0.4f);
    }
}
