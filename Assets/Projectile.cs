using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Animator animator;
    public LayerMask ignoreRayCastMask;
    [HideInInspector] public DamageType damageType;
    [HideInInspector] public GameObject circleAroundObj;
    [HideInInspector] public BossUdokEnemyUnit bossUdok;
    [HideInInspector] public bool isCirclingAround = false;
    [HideInInspector] public bool isHittingEnemies = false;

    public void ShootToTarget(Vector3 pos, float time)
    {
        isCirclingAround = false;
        LeanTween.cancel(gameObject);
        LeanTween.move(gameObject, pos, time).setEaseInBack().setOnComplete(() =>
        {
            DestroyProj();
        });
    }



    private void Update()
    {
        if (isCirclingAround)
        {
            gameObject.transform.RotateAround(circleAroundObj.transform.position, Vector3.up, 30 * Time.deltaTime);
        }
    }

    public void startCircleAround()
    {
        isCirclingAround = true;
        
        LeanTween.moveLocalY(gameObject, 2.5f, 1.3f).setEaseInOutQuad().setLoopPingPong();
    }

    public void DestroyProj()
    {
        LeanTween.cancel(gameObject);
        animator.SetTrigger("Explode");
        Destroy(gameObject, 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("Projectile Hit: " + other.tag);

        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Unit>().DoDamage(gameObject, damageType);
        }
        else if (other.CompareTag("Enemy") && isHittingEnemies)
        {
            other.gameObject.GetComponent<Unit>().DoDamage(gameObject, damageType);
            LeanTween.cancel(gameObject);
            animator.SetTrigger("Explode");
            Invoke("DestroyProj", 1);
        }
    }
}
