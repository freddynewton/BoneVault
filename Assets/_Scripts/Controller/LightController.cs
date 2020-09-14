using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public AnimationCurve animationCurve;
    private float animationTime;
    private Light torchLight;
    private float timeElapsed;


    void Start()
    {
        torchLight = transform.GetComponent<Light>();
        timeElapsed = 0;
        animationTime = animationCurve[animationCurve.length - 1].time;
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