using UnityEngine;

public abstract class ActionAI : ScriptableObject
{
    public abstract void use(UtilityAIHandler controller);
}