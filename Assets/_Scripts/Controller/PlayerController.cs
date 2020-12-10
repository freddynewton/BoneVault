using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Singleton Instance
    public static PlayerController Instance { get; private set; }

    [Header("Scrips")]
    public CameraHandler playerCameraHandler;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayerUnit unit;

    [Header("Sound")]
    public AudioClip[] walkSFX;
    public AudioClip[] dropSFX;
    [HideInInspector] public AudioSource audioSource;
    private float stepTimer;
    private float fallTimer;
    private float interval = 0.75f;

    [Header("Game Objests")]
    public GameObject weaponPos;

    // Movement
    [HideInInspector] public Vector3 move;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] bool isGrounded;
    private float walkSpeed;
    private float baseSpeed;
    private float sprintSpeed;
    private Vector3 velocity;


    private void Start()
    {
        unit = GetComponent<PlayerUnit>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        walkSpeed = unit.baseStats.moveSpeed;
        baseSpeed = unit.baseStats.moveSpeed;
        sprintSpeed = unit.baseStats.moveSpeed * 2f;
        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Update()
    {
        isGrounded = checkIsGrounded();
        Debug.Log(isGrounded);

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            InputButtons();
            Movement();
        }
    }

    private bool checkIsGrounded()
    {
        bool tmp = Physics.Raycast(transform.position, -Vector3.up, 0.2f);

        if (tmp && !isGrounded) SoundManager.Instance.playRandomSFX(dropSFX, audioSource, 0.8f, 1.2f);

        return tmp;
    }

    private void InputButtons()
    {
        // Sprint
        Sprint();

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
                walkSpeed = unit.baseStats.moveSpeed / 2;
            }
        }

        // End Block
        if (Input.GetButtonUp("Fire2"))
        {
            Inventory.Instance.currWeaponScript.attackRightClick(false);
            walkSpeed = baseSpeed * unit.upgradeHandler.baseSpeedPercentageUpgrade;
        }

    }

    private void Sprint()
    {
        if (unit.currentStamina > 0)
        {
            if (Input.GetButtonDown("Sprint"))
            {
                walkSpeed = sprintSpeed * unit.upgradeHandler.sprintSpeedPercentageUpgrade;
                isSprinting = true;
                interval = 0.4f;
            }
            else if (Input.GetButtonUp("Sprint"))
            {
                walkSpeed = baseSpeed * unit.upgradeHandler.baseSpeedPercentageUpgrade;
                isSprinting = false;
                interval = 0.75f;
            }

            if (Input.GetButton("Sprint"))
            {
                unit.setStamina(-((unit.playerStats.sprintCostRate * unit.upgradeHandler.sprintSpeedPercentageCostsUpgrade) * Time.deltaTime));
            }
        }
        else
        {
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
        if (isGrounded && velocity.y > 0)
        {
            velocity.y -= 2 * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(unit.playerStats.jumpHeight * -2f * unit.playerStats.gravity);
        }

        // apply gravity acceleration to vertical speed:
        velocity.y += unit.playerStats.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // movement sounds
        if (move != Vector3.zero && isGrounded)
        {
            walkSounds();
        }
    }

    private void Jump()
    {

    }

    private void walkSounds()
    {
        stepTimer += Time.deltaTime;

        // play step sounds in intervalls from a list of sound files randomly
        if (stepTimer >= interval)
        {
            SoundManager.Instance.playRandomSFX(walkSFX, audioSource, 0.8f, 1.2f);
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