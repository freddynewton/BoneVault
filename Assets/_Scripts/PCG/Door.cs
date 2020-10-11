using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public void openDoor()
    {
        LeanTween.moveLocalY(gameObject, 6, 2).setEaseOutBounce();
    }

    public void closeDoor()
    {
        LeanTween.moveLocalY(gameObject, 0, 2).setEaseOutBounce();
    }
}
