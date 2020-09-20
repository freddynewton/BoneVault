using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton Instance
    public static PlayerController Instance { get; private set; }
    public GameObject weaponPos;

    [HideInInspector] public PlayerUnit Unit;
    [HideInInspector] public CharacterController Controller;

    private Vector3 velocity;
    private float baseSpeed;
    private float sprintSpeed;


    private void Start()
    {
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
        

        // Ground Check
        if (isGrounded && velocity.y < 0){
            velocity.y = -Unit.stats.gravity * Time.deltaTime; ;
        }
        else velocity.y += Unit.stats.gravity * Time.deltaTime;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(Unit.stats.jumpHeight * -3f * Unit.stats.gravity);
        }


        // Sprint Input
        if (Input.GetButton("Sprint")) {
            Unit.stats.moveSpeed = sprintSpeed;
        }
        else {
            Unit.stats.moveSpeed = baseSpeed;
        }

        // Left Click Attack
        if(Input.GetButtonDown("Fire1")) {
            if (Inventory.Instance.currWeapon != null)
                Inventory.Instance.currWeaponScript.attackLeftClick();
        }

        // Right Click Attack
        if (Input.GetButtonDown("Fire2")) {
            if (Inventory.Instance.currWeapon != null)
                Inventory.Instance.currWeaponScript.attackRightClick();
        }

        

        /*
        if (move != Vector3.zero) {
            Animator.SetBool("walking", true);
        }
        else Animator.SetBool("walking", false);
        */
        
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

        if (Inventory.Instance.currWeapon == null) Inventory.Instance.equipWeapon(Resources.Load<GameObject>("Items/Weapons/LongSword"));
    }
}
