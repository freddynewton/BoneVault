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

    private void Awake()
    {
        torchLight = gameObject.GetComponent<Light>();
        animationTime = animationCurve[animationCurve.length - 1].time;
        animator = GetComponentInParent<Animator>();

        // Get Default State and start at random Time
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, Random.Range(0.0f, 1.0f)); ;

        // StartCoroutine(setLightFlickering());
    }

    public IEnumerator setLightFlickering()
    {
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0, 1));

        LeanTween.value(UnityEngine.Random.Range(4, 3.95f), UnityEngine.Random.Range(3.90f, 3.85f), UnityEngine.Random.Range(1, 0.5f)).setEaseInOutSine().setOnUpdate((float var) =>
        {
            torchLight.intensity = var;
        }).setLoopPingPong();

        LeanTween.value(UnityEngine.Random.Range(16, 15.5f), UnityEngine.Random.Range(14, 13.5f), UnityEngine.Random.Range(4, 0.5f)).setEaseInOutSine().setOnUpdate((float var) =>
        {
            torchLight.range = var;
        }).setLoopPingPong();

    }
}