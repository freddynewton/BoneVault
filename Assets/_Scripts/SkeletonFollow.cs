using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonFollow : MonoBehaviour
{
    public Transform Target;
    public Animator Animator;
    private float distance;
    private float moveSpeed;


    void Start()
    {
        Target = GameObject.Find("Player").transform;
        Animator = transform.GetComponent<Animator>();
        moveSpeed = 3f;
    }

    void Update() {
        Follow();
    }

    void Follow() {
        distance = Vector3.Distance(Target.position, transform.position);

        if (distance > 4f) {
            Animator.SetBool("walking", true);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, Time.deltaTime * moveSpeed);
        }
        else Animator.SetBool("walking", false);
    }
}
