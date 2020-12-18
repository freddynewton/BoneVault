using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUnit : EnemyUnit
{
    [Header("Boss Unit Setting")]
    public BossRoom bossRoom;
    public int phase = 1;
}
