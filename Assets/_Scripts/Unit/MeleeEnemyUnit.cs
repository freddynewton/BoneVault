using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MeleeEnemyUnit : EnemyUnit
{
    public List<GameObject> triggerList = new List<GameObject>();

    public void CallTriggerDamage()
    {
        foreach (GameObject obj in triggerList)
        {
            if (obj.GetComponent<Unit>())
                obj.GetComponent<Unit>().DoDamage(gameObject, damageType);
            else if (obj.GetComponent<DestroyableEntity>())
                obj.GetComponent<DestroyableEntity>().interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !triggerList.Contains(other.gameObject)) triggerList.Add(other.gameObject);
        if (other.gameObject.CompareTag("Interactable") && !triggerList.Contains(other.gameObject)) triggerList.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && triggerList.Contains(other.gameObject)) triggerList.Remove(other.gameObject);
        if (other.gameObject.CompareTag("Interactable") && !triggerList.Contains(other.gameObject)) triggerList.Add(other.gameObject);
    }

    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        base.Update();
    }
    public override void setWalkingAnimation()
    {
        base.setWalkingAnimation();
    }
    public override void knockback(Vector3 otherPos, float kb)
    {
        base.knockback(otherPos, kb);
    }
    public override void hit()
    {
        base.hit();
    }
    public override void DoDamage(GameObject damageObj, DamageType damageType)
    {
        base.DoDamage(damageObj, damageType);
    }
    public override void death()
    {
        base.death();
    }
}
