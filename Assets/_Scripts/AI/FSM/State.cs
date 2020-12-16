using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "FSM/State")]
public class State : ScriptableObject
{
    [Serializable] public struct transStruct
    {
        public State toState;
        public bool isFalseTrue;
        public Transition transition;
    }

    public transStruct[] Transitions;
    public StateAction[] Actions;
}
