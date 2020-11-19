using System.Collections;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    [SerializeField] private float amplitude;
    [SerializeField] private float baseRange;
    [SerializeField] private float baseIntensity;
    [SerializeField] private float tweenDuration;
    private Light torchLight;

    private void Awake()
    {
        torchLight = gameObject.GetComponent<SpriteRenderer>().GetComponent<Light>();
        animator = GetComponentInParent<Animator>();

        // Get Default State and start at random Time
        animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, Random.Range(0.0f, 1.0f)); ;
        StartCoroutine(setLightFlickering());
    }

    public IEnumerator setLightFlickering()
    {
        yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0, 1));

        LeanTween.value(baseIntensity - UnityEngine.Random.Range(0, amplitude), baseIntensity + UnityEngine.Random.Range(0, amplitude), tweenDuration).setEaseInOutSine().setOnUpdate((float var) =>
        {
            torchLight.intensity = var;
        }).setLoopPingPong();

        LeanTween.value(baseRange - UnityEngine.Random.Range(0, amplitude), baseRange + UnityEngine.Random.Range(0, amplitude), tweenDuration).setEaseInOutSine().setOnUpdate((float var) =>
        {
            torchLight.range = var;
        }).setLoopPingPong();
    }
}