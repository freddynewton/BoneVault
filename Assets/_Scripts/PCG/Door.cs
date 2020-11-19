using UnityEngine;

public class Door : MonoBehaviour
{
    public void openDoor()
    {
        // TODO Dust Particle

        LeanTween.moveLocalY(gameObject, 6, 2).setEaseOutBounce();
    }

    public void closeDoor()
    {
        // TODO Dust Particle

        LeanTween.moveLocalY(gameObject, 0, 1).setEaseOutBounce();
    }
}