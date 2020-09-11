using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton Instance
    public static PlayerController Instance { get; private set; }

    [HideInInspector] public PlayerUnit unit;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Animator Animator;

    private Vector3 velocity;
    private float baseSpeed;
    private float sprintSpeed;


    private void Start()
    {
        Animator = GameObject.Find("Canvas/Hands").GetComponent<Animator>();
        unit = GetComponent<PlayerUnit>();
        controller = GetComponent<CharacterController>();
        baseSpeed = unit.stats.moveSpeed;
        sprintSpeed = unit.stats.moveSpeed * 2f;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        bool isGrounded = controller.isGrounded;      

        controller.Move(move * unit.stats.moveSpeed * Time.deltaTime);
        Debug.Log(isGrounded);

        // Ground Check
        if (isGrounded && velocity.y < 0){
            velocity.y = -unit.stats.gravity * Time.deltaTime; ;
        }
        else velocity.y += unit.stats.gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(unit.stats.jumpHeight * -3f * unit.stats.gravity);
        }

        if (Input.GetButton("Sprint")) {
            unit.stats.moveSpeed = sprintSpeed;
        }
        else {
            unit.stats.moveSpeed = baseSpeed;
        }

        if(Input.GetButtonDown("Fire1")) {
            Animator.SetTrigger("hit");
        }

        if (Input.GetButtonDown("Fire2")) {
            Animator.SetBool("block", true);
        }

        if (Input.GetButtonUp("Fire2")) {
            Animator.SetBool("block", false);
        }

        if (move != Vector3.zero) {
            Animator.SetBool("walking", true);
        }
        else Animator.SetBool("walking", false);

        
        controller.Move(velocity * Time.deltaTime);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
