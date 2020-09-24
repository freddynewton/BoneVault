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

    [HideInInspector] public PlayerUnit unit;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 move;

    private float walkSpeed;
    private float baseSpeed;
    private float sprintSpeed;
    private float halfSpeed;
    private float fallSpeed;
    private Vector3 velocity;


    private void Start()
    {
        unit = GetComponent<PlayerUnit>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        walkSpeed = unit.stats.moveSpeed;
        baseSpeed = unit.stats.moveSpeed;
        sprintSpeed = unit.stats.moveSpeed * 2f;
        halfSpeed = unit.stats.moveSpeed / 2;
    }

    private void Update()
    {
        Movement();
        InputButtons();
    }

    private void InputButtons()
    {
        // Left Click Attack
        if (Input.GetButtonDown("Fire1"))
        {
            if (Inventory.Instance.currWeapon != null)
            {
                Inventory.Instance.currWeaponScript.attackLeftClick(true);
            }
        }


        // Right Click Attack
        if (Input.GetButtonDown("Fire2"))
        {
            if (Inventory.Instance.currWeapon != null)
            {
                Inventory.Instance.currWeaponScript.attackRightClick(true);
                walkSpeed = halfSpeed;
            }
        }

        // End Block
        if (Input.GetButtonUp("Fire2"))
        {
            Inventory.Instance.currWeaponScript.attackRightClick(false);
            walkSpeed = baseSpeed;
        }

        // Sprint Input
        if (Input.GetButtonDown("Sprint"))
        {
            walkSpeed = sprintSpeed;
        } else if (Input.GetButtonUp("Sprint"))
        {
            walkSpeed  = baseSpeed;
        }
    }

    private void Movement()
    {
        // Move Input
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * walkSpeed * Time.deltaTime);

        // Gravity
        if (controller.isGrounded)
        {
            fallSpeed = 0;
        }

        // apply gravity acceleration to vertical speed:
        fallSpeed += unit.stats.gravity * Time.deltaTime;
        velocity.y = fallSpeed;
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
