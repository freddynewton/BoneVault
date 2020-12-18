using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class Collectable : MonoBehaviour
{
    public int statBonus;

    [Serializable]
    public enum Stat {
        HEALTH,
        STAMINA,
        SKULLS
    }
    public Stat stat;


    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag("Player")) {
            if (stat == Stat.HEALTH) {
                Debug.Log("Add Health");
                GameObject.Find("Player").GetComponent<PlayerUnit>().setHealthPlayer(10);
            }
            else if (stat == Stat.STAMINA) {
                Debug.Log("Add Schtamina");
                GameObject.Find("Player").GetComponent<PlayerUnit>().setStamina(10);
            }
            else {
                Debug.Log("Add Skulls");
                Inventory.Instance.setBones(1);
            }

            UiManager.Instance.flashScreen.flashScreen(1, new Color(1f, 1f, 1f, 0.5f));
            SoundManager.Instance.playSFX(GetComponent<AudioSource>().clip, GetComponent<AudioSource>(), 0.8f, 1.2f);

            Destroy(gameObject, 0.1f);
        }
    }
}
