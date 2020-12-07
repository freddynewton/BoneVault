using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        START_ROOM,
        ENEMIE_ROOM,
        BOSS_ROOM,
        SPECIAL_ROOM,
        HALLWAY
    }

    public enum RoomDirection
    {
        OneDoor,
        TwoDoorLinear,
        TwoDoorCurve,
        ThreeDoor,
        FourDoor,
        Hallway
    }

    [Header("Basic Room Settings")]
    public RoomType roomType;

    public RoomDirection roomDirection;

    [Tooltip("Will setup automatical")] public List<Door> doors;

    [Header("Room Light Settings")]
    [Tooltip("Will setup automatical")] public List<Light> lights;

    public float activateLightDelay = 0.2f;
    public Color mainColor = Color.HSVToRGB(189, 100, 70);
    public Color secColor;
    public AudioClip [] torchSFX;
    public AudioClip [] doorOpenSFX;
    public AudioClip [] doorShutSFX;

    [HideInInspector] public MiniMapPart miniMapPart;
    [HideInInspector] public AudioSource randomSound;

    public virtual void Awake()
    {


        getAllLights();
        getAllDoors();
        setDoors(true);

        foreach (Light l in lights)
        {
            l.gameObject.SetActive(false);

            SpriteRenderer rend = l.gameObject.GetComponent<SpriteRenderer>();
            if (rend != null) l.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void getAllLights()
    {
        foreach (Light l in gameObject.GetComponentsInChildren<Light>()) lights.Add(l);
    }

    public void getAllDoors()
    {
        foreach (Door d in gameObject.GetComponentsInChildren<Door>()) doors.Add(d);
    }

    public void setDoors(bool isOpen)
    {
        foreach (Door d in doors)
        {
            if (isOpen) {
                d.openDoor();
                if (!d.isOpen) playSFX(doorOpenSFX, d.GetComponent<AudioSource>(), false);
                d.isOpen = true;
            }
            else {
                d.closeDoor();
                if (d.isOpen) playSFX(doorShutSFX, d.GetComponent<AudioSource>(), false);
                d.isOpen = false;
            }
        }
    }

    public void setLights(Color color)
    {
        StartCoroutine(lightDelay(color, 0));
    }

    private IEnumerator lightDelay(Color color, int idx)
    {
        yield return new WaitForSecondsRealtime(activateLightDelay);

        Light l = lights[idx];

        SpriteRenderer rend = l.gameObject.gameObject.GetComponent<SpriteRenderer>();

        if (rend != null)
        {
            rend.enabled = true;
            rend.color = color;
        }

        if (!l.gameObject.activeSelf) playSFX(torchSFX, l.transform.parent.GetComponent<AudioSource>(), true);

        l.gameObject.SetActive(true);
        l.color = color;

        if (idx < lights.Count - 1) StartCoroutine(lightDelay(color, idx += 1));
    }

    // SFX Handler
    public void playSFX (AudioClip [] sounds, AudioSource source, bool random) {
        if (random) source.clip = sounds [Random.Range(0, sounds.Length)];
        else source.clip = sounds [0];

        source.pitch = Random.Range(0.8f, 1.2f);

        if (source != null) source.Play();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (miniMapPart != null) miniMapPart.gameObject.SetActive(true);
    }

    public virtual void OnTriggerExit(Collider other)
    {
    }
}