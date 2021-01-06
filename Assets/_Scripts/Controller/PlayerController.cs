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

    [Header("Game Objests")]
    public GameObject weaponPos;

    // Movement
    [HideInInspector] public Vector3 move;
    [HideInInspector] public bool isSprinting;
    [HideInInspector] public bool isGrounded;
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
        isSprinting = Input.GetButton("Sprint");

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            InputButtons();
            Jump();
            Sprint();
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


        // Left Click Attack
        if (Input.GetButtonDown("Fire1"))
        {
            if (Inventory.Instance.currWeapon != null)
            {
                Inventory.Instance.currWeaponScript.ability1(true);
            }
        }

        // Right Click Attack
        if (Input.GetButtonDown("Fire2"))
        {
            if (Inventory.Instance.currWeapon != null)
            {
                Inventory.Instance.currWeaponScript.ability2(true);
                walkSpeed = unit.baseStats.moveSpeed / 2;
            }
        }

        // End Block
        if (Input.GetButtonUp("Fire2"))
        {
            Inventory.Instance.currWeaponScript.ability2(false);
            walkSpeed = baseSpeed * unit.upgradeHandler.baseSpeedPercentageUpgrade;
        }

    }

    private void Sprint()
    {
        if (unit.currentStamina > 0)
        {
            if (isSprinting)
            {
                walkSpeed = sprintSpeed * unit.upgradeHandler.sprintSpeedPercentageUpgrade;

                unit.setStamina(-((unit.playerStats.sprintCostRate * unit.upgradeHandler.sprintSpeedPercentageCostsUpgrade) * Time.deltaTime));
            }
            else
            {
                walkSpeed = baseSpeed * unit.upgradeHandler.baseSpeedPercentageUpgrade;
            }
        }
        else
        {
            isSprinting = false;
            walkSpeed = baseSpeed * unit.upgradeHandler.baseSpeedPercentageUpgrade;
        }
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(unit.playerStats.jumpHeight * -3f * unit.playerStats.gravity);
        }
    }

    private void Movement()
    {
        // Move Input
        move = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        // Move Character Controller
        controller.Move(move * walkSpeed * Time.deltaTime);

        // apply gravity acceleration to vertical speed:
        velocity.y += unit.playerStats.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // movement sounds
        if (move != Vector3.zero && isGrounded)
        {
            walkSounds();
        }
    }

    private void walkSounds()
    {
        stepTimer += Time.deltaTime;

        // play step sounds in intervalls from a list of sound files randomly
        if (stepTimer >= (isSprinting ? 0.4 : 0.75))
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