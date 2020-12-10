using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    [Header("Melee Weapon Settings")]
    public float doDamageAfterSec = 0.3f;

    [HideInInspector] public List<GameObject> hitObjects;

    public override void interact() { base.interact(); }

    #region Trigger handling
    
    private void OnTriggerEnter(Collider other)
    {
        // Layer 11 is Enemy Layer
        if (other.gameObject.layer == 11 && !hitObjects.Contains(other.gameObject)) hitObjects.Add(other.gameObject);
        if (other.CompareTag("Interactable") && !hitObjects.Contains(other.gameObject)) hitObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        // Layer 11 is Enemy Layer
        if (other.gameObject.layer == 11 && hitObjects.Contains(other.gameObject)) hitObjects.Remove(other.gameObject);
        if (other.CompareTag("Interactable") && hitObjects.Contains(other.gameObject)) hitObjects.Remove(other.gameObject);
    }
    #endregion
}
