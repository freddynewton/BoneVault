using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "AI/Settings")]
public class SettingsAI : ScriptableObject
{

    public List<actionSetting> actionSettingList;
    
    [Serializable]
    public struct actionSetting
    {
        public List<ActionAI> actionList;
        public List<setting> settingList;
    }

    [Serializable]
    public struct setting
    {
        public AnimationCurve curve;
        public InputAiEnum input;
    }


}
