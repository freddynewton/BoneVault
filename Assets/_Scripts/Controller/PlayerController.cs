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
        baseSpeed = unit.stats.moveSpeed;
        sprintSpeed = unit.stats.moveSpeed * 2f;
        halfSpeed = unit.stats.moveSpeed / 2;
    }

    private void Update()
    {
        Movement();       
    }

    private void Movement()
    {
        // Move Input
        Vector3 move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(move * unit.stats.moveSpeed * Time.deltaTime);

        // Left Click Attack
        if (Input.GetButtonDown("Fire1") && !Inventory.Instance.currWeaponScript.isAttacking) {
            if (Inventory.Instance.currWeapon != null) {
                Inventory.Instance.currWeaponScript.isAttacking = true;
                Inventory.Instance.currWeaponScript.attackLeftClick();
            }
        }
        // Right Click Attack
        else if (Input.GetButtonDown("Fire2")) {
            if (Inventory.Instance.currWeapon != null) {
                Inventory.Instance.currWeaponScript.isBlocking = true;
                Inventory.Instance.currWeaponScript.attackRightClick();
                unit.stats.moveSpeed = halfSpeed;
            }
        }

        // End Block
        if (Input.GetButtonUp("Fire2")) {
            Inventory.Instance.currWeaponScript.blockComplete();
            unit.stats.moveSpeed = baseSpeed;
        }
        
        // Sprint Input
        if (Input.GetButton("Sprint")) {
            unit.stats.moveSpeed = sprintSpeed;
        }

        if (Input.GetButtonUp("Sprint")) {
            unit.stats.moveSpeed = baseSpeed;
        }

        // idle/walking animation
        if (move != Vector3.zero) {
            if (Inventory.Instance.currWeapon != null) {
                Inventory.Instance.currWeaponScript.isMoving = true;
                Inventory.Instance.currWeaponScript.move();
            }
        }
        else {
            if (Inventory.Instance.currWeapon != null) {
                Inventory.Instance.currWeaponScript.isMoving = false;
                Inventory.Instance.currWeaponScript.idle();
            }
        }

        // Gravity
        if (controller.isGrounded) {
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
