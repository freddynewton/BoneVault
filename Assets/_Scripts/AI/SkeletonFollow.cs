using UnityEngine;

public class SkeletonFollow : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    private float distance;
    private float moveSpeed;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
        animator = transform.GetComponent<Animator>();
        moveSpeed = 3f;
    }

    private void Update()
    {
        Follow();
    }

    private void Follow()
    {
        distance = Vector3.Distance(target.position, transform.position);

        if (distance > 4f)
        {
            animator.SetBool("walking", true);
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
        }
        else animator.SetBool("walking", false);
    }
}