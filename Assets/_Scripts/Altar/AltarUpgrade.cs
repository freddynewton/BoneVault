using UnityEngine;

public abstract class AltarUpgrade : ScriptableObject
{
    [Header("Altar Default Settings")]
    public int BoneCost = 5;

    public Sprite Icon;
    public Color lightColor;

    public string uiText = "This is a Text";

    public abstract void use();
}