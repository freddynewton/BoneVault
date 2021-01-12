using UnityEngine;

public class BossRoom : Room
{
    [Header("Boss Spawn Settings")]
    public GameObject bossPF;
    public Transform spawnPos;
    public Transform enemyContainer;
    private bool bossSpawned = false;

    [Header("TrapDoor")]
    public GameObject[] TrapDoor;
    public AudioClip [] trapDoorSFX;

    [Header("Portal Room")]
    public Door portalDoor;

    [Header("Button")]
    public GameObject Button;

    public void openTrapDoor()
    {
        foreach (GameObject trap in TrapDoor)
        {
            LeanTween.rotateX(trap, 90, 4).setEaseOutBounce();
            playSFX(trapDoorSFX, GetComponent<AudioSource>(), false);
        }
    }

    public void spawnBoss()
    {
        if (!bossSpawned)
        {
            bossSpawned = true;
            
            GameObject boss;
            BossUnit bUnit;
            boss = Instantiate(bossPF, spawnPos.position, Quaternion.identity, enemyContainer);
            bUnit = boss.GetComponent<BossUnit>();
            bUnit.bossRoom = this;
            bUnit.room = this;
            UiManager.Instance.setBossHealth(boss.GetComponent<BossUnit>().currentHealth, boss.GetComponent<BossUnit>().baseStats.maxHealth);
            StateMachineController.Instance.enemyUnits.Add(boss.GetComponent<EnemyUnit>());
        }
    }

    public override void Awake()
    {
        getAllLights();
        setDoors(true);
        portalDoor.closeDoor();

        foreach (Light l in lights)
        {
            l.gameObject.SetActive(false);

            SpriteRenderer rend = l.gameObject.GetComponent<SpriteRenderer>();
            if (rend != null) l.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        setLights(mainColor);
        openTrapDoor();
        spawnBoss();
        if (!SoundManager.Instance.musicSource.isPlaying) StartCoroutine(SoundManager.fadeMusic(musicSource, 0, 3f, true));
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}