using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    public AnimationCurve animationCurve;
    private float animationTime;
    private Light torchLight;
    private float timeElapsed;


    void Start()
    {
        torchLight = transform.GetComponent<Light>();
        timeElapsed = 0;
        animationTime = animationCurve[animationCurve.length - 1].time;
        animator = GetComponentInParent<Animator>();

        animator.Play("Torch_Idle", 0, Random.Range(0.0f, 1.0f));
    }

    void FixedUpdate()
    {
        if (timeElapsed < animationTime) {
            timeElapsed += Time.deltaTime;
        }
        else timeElapsed = 0;

        torchLight.intensity = animationCurve.Evaluate(timeElapsed);
    }
}