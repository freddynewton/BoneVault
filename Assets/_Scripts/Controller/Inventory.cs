using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Singleton Instance
    public static Inventory Instance { get; private set; }

    [Header("Weapon")]
    public GameObject currWeapon;
    public Weapon currWeaponScript;

    [Header("Currency")]
    public int bones;

    public void setBones(int amount)
    {
        bones += amount;
        UiManager.Instance.setBones(bones.ToString());
    }

    public void equipWeapon(GameObject newWeapon)
    {
        if (newWeapon == null) return;

        if (currWeapon != null) { 
        currWeaponScript.callOnEquip(false);
        Destroy(currWeapon);
        }

        currWeapon = Instantiate(newWeapon, PlayerController.Instance.weaponPos.transform.position, Quaternion.identity, PlayerController.Instance.weaponPos.transform) as GameObject;
        currWeaponScript = currWeapon.GetComponent<Weapon>();
        currWeaponScript.callOnEquip(true);
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

        StartCoroutine(startWeapon());
    }

    private IEnumerator startWeapon()
    {
        yield return new WaitForEndOfFrame();
        
        if (currWeapon == null) equipWeapon(Resources.Load<GameObject>("Items/Weapons/LongSword"));
    }
}
