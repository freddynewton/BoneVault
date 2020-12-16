using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : ScriptableObject
{
    public abstract bool CheckTransition(StateMachine sm);
}
