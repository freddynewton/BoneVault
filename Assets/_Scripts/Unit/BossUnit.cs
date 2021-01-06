using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUnit : EnemyUnit
{
    [Header("Boss Unit Setting")]
    public BossRoom bossRoom;
    public int phase = 1;

    public override void death()
    {
        bossRoom.portalDoor.openDoor();
        StartCoroutine(SoundManager.fadeMusic(SoundManager.Instance.GetComponent<AudioSource>(), 0, 3f, false));

        Inventory.Instance.setBones(Random.Range(0, 10));

        StateMachineController.Instance.ClearEnemies();

        base.death();
    }
}