using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton Instance
    public static PlayerController Instance { get; private set; }
    public GameObject Hands;
    public GameObject HitDetection;

    [HideInInspector] public PlayerUnit Unit;
    [HideInInspector] public CharacterController Controller;
    [HideInInspector] public Animator Animator;

    private Vector3 velocity;
    private float baseSpeed;
    private float sprintSpeed;


    private void Start()
    {
        Animator = Hands.GetComponent<Animator>();
        Unit = GetComponent<PlayerUnit>();
        Controller = GetComponent<CharacterController>();
        baseSpeed = Unit.stats.moveSpeed;
        sprintSpeed = Unit.stats.moveSpeed * 2f;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        bool isGrounded = Controller.isGrounded;      

        Controller.Move(move * Unit.stats.moveSpeed * Time.deltaTime);
        //Debug.Log(isGrounded);

        // Ground Check
        if (isGrounded && velocity.y < 0){
            velocity.y = -Unit.stats.gravity * Time.deltaTime; ;
        }
        else velocity.y += Unit.stats.gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(Unit.stats.jumpHeight * -3f * Unit.stats.gravity);
        }

        if (Input.GetButton("Sprint")) {
            Unit.stats.moveSpeed = sprintSpeed;
        }
        else {
            Unit.stats.moveSpeed = baseSpeed;
        }

        if(Input.GetButtonDown("Fire1")) {
            Animator.SetTrigger("hit");

            HitDetection.GetComponent<HitDetection>().enabled = true;

            //var currentClipInfo = Animator.GetCurrentAnimatorClipInfo(0);
            //var currentClipLength = currentClipInfo [0].clip.length;
            //var clipName = currentClipInfo [0].clip.name;
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

        
        Controller.Move(velocity * Time.deltaTime);
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
