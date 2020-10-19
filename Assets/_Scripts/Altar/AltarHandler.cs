using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AltarHandler : MonoBehaviour
{
    [HideInInspector] public List<Outline> outlines;

    [Header("Altar Settings")]
    public float distance = 5;
    public GameObject hand;
    public SpriteRenderer spriteRend;
    public Light altarLight;

    [HideInInspector] public SpecialRoom room;
    [HideInInspector] public SphereCollider col;
    public AltarUpgrade upgrade;

    public bool used = false;

    public void setOutline(bool active)
    {
        foreach (Outline o in outlines)
        {
            o.enabled = active;
        }
    }

    public void setUpgrade(AltarUpgrade _upgrade)
    {
        upgrade = _upgrade;
        spriteRend.sprite = upgrade.Icon;
        altarLight.color = upgrade.lightColor;
        spriteRend.color = Color.white;
        spriteRend.enabled = true;
        altarLight.enabled = true;
        spriteRend.gameObject.SetActive(true);
    }

    public void use()
    {
        if (Inventory.Instance.bones >= upgrade.BoneCost)
        {
            used = true;
            Inventory.Instance.setBones(-upgrade.BoneCost);
            upgrade.use();
            altarLight.color = room.mainColor;
            altarLight.intensity = 2;

            // SWAP SPRITE TO "EMPTY SPRITE"
            spriteRend.sprite = null;

            // DISABLE DIALOG TEXT BOX
            UiManager.Instance.showText(false, "", 0, 0);

            // Shake GameObject
            LeanTween.moveLocalX(hand.gameObject.transform.parent.gameObject, 0.2f, 0.2f).setEaseOutBounce().setOnComplete(() =>
            {
                LeanTween.moveLocalX(hand.gameObject.transform.parent.gameObject, 0, 0.1f).setEaseOutBounce();
            }).setLoopCount(2);

        }
        else
        {
            // TODO ERROR MESSAGE
            UiManager.Instance.showErrorMessage(upgrade, 0.5f);
        }
    }

    public void showText(bool acitve)
    {
        if (!used) UiManager.Instance.showText(acitve, upgrade.uiText, upgrade.BoneCost, 0.1f);
        else UiManager.Instance.showText(false, "", 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        showText(true);
    }

    private void OnTriggerExit(Collider other)
    {
        showText(false);
    }



    private void Awake()
    {
        StartCoroutine(startHandmoving());
    }

    private IEnumerator startHandmoving()
    {
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0, 2f));
        LeanTween.moveY(hand, 2.8f, 2f).setEaseInOutQuad().setDelay(UnityEngine.Random.Range(0, 0.3f)).setLoopPingPong();
    }

    // Start is called before the first frame update
    void Start()
    {
        outlines = gameObject.GetComponentsInChildren<Outline>().ToList();
        col = gameObject.GetComponent<SphereCollider>();
        col.radius = distance;
    }
}
