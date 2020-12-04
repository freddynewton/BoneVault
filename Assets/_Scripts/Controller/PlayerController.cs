using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton Instance
    public static PlayerController Instance { get; private set; }
    public CameraHandler playerCameraHandler;
    public GameObject weaponPos;
    public AudioClip [] walkSFX;

    [HideInInspector] public PlayerUnit unit;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Animator animator;
    [HideInInspector] public Vector3 move;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public AudioSource randomSound;

    private float walkSpeed;
    private float baseSpeed;
    private float sprintSpeed;
    private float fallSpeed;
    private Vector3 velocity;
    private float stepTimer;
    private float interval = 0.75f;

    private void Start()
    {
        unit = GetComponent<PlayerUnit>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        walkSpeed = unit.stats.moveSpeed;
        baseSpeed = unit.stats.moveSpeed;
        sprintSpeed = unit.stats.moveSpeed * 2f;
        randomSound = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        Movement();

        if (Cursor.lockState == CursorLockMode.Locked) InputButtons();
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
                walkSpeed = unit.stats.moveSpeed / 2;
            }
        }

        // End Block
        if (Input.GetButtonUp("Fire2"))
        {
            Inventory.Instance.currWeaponScript.attackRightClick(false);
            walkSpeed = baseSpeed * unit.upgradeHandler.baseSpeedPercentageUpgrade;
        }

        Sprint();
    }

    private void Sprint()
    {
        if (unit.currentStamina > 0)
        {
            if (Input.GetButtonDown("Sprint")) {
                walkSpeed = sprintSpeed * unit.upgradeHandler.sprintSpeedPercentageUpgrade;
                isSprinting = true;
                interval = 0.4f;
            }
            else if (Input.GetButtonUp("Sprint")) {
                walkSpeed = baseSpeed * unit.upgradeHandler.baseSpeedPercentageUpgrade;
                isSprinting = false;
                interval = 0.75f;
            }

            if (Input.GetButton("Sprint")) {
                unit.setStamina(-((unit.stats.sprintCostRate * unit.upgradeHandler.sprintSpeedPercentageCostsUpgrade) * Time.deltaTime));
            }
        }
        else {
            walkSpeed = baseSpeed * unit.upgradeHandler.baseSpeedPercentageUpgrade;
            isSprinting = false;
            interval = 0.75f;
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

        // movement sounds
        if (move != Vector3.zero) {
            RandomWalkSFX();
        }
    }

    private void RandomWalkSFX() {
        stepTimer += Time.deltaTime;

        // play step sounds in intervalls from a list of sound files randomly
        if (stepTimer >= interval) {
            randomSound.clip = walkSFX [Random.Range(0, walkSFX.Length)];
            randomSound.pitch = Random.Range(0.8f, 1.2f);

            if (randomSound != null) randomSound.Play();
            stepTimer = 0.0f;
        }
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