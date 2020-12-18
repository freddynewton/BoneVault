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
            }
            else if (stat == Stat.STAMINA) {
                Debug.Log("Add Schtamina");
            }
            else {
                Debug.Log("Add Skulls");
            }

            // flash screen white
            // play collect SFX

            Destroy(gameObject);
        }
    }
}
