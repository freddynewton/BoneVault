using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public enum InputAiEnum
{
    Health,
    RangeToTargetNormalized,
    TargetHealth
}

public class UtilityAIHandler : MonoBehaviour
{
    [Header("Settings")]
    public SettingsAI settings;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public EnemyUnit unit;
    [HideInInspector] public NavMeshAgent navAgent;


    private List<float> utilitiesArr = new List<float>();
    private int currentAction;

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponent<EnemyUnit>();
        rb = GetComponent<Rigidbody2D>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    private void Awake()
    {
        StartCoroutine(setNavAgent());
    }

    private void calcUtility()
    {
        utilitiesArr.Clear();

        foreach (var action in settings.actionSettingList)
        {
            utilitiesArr.Add(0);

            float ut = 1;

            foreach (var setting in action.settingList)
            {
                ut *= setting.curve.Evaluate(getEnumInputValue(setting.input));
            }

            utilitiesArr[settings.actionSettingList.IndexOf(action)] = ut;
        }

        chooseHighestScoreUtility();
    }

    private IEnumerator setNavAgent()
    {
        yield return new WaitForEndOfFrame();
        navAgent.speed = unit.stats.moveSpeed;
        navAgent.stoppingDistance = unit.stats.stoppingDistance;
    }

    private void chooseHighestScoreUtility()
    {
        int index = 0;
        float highestScore = 0;

        foreach (float ut in utilitiesArr)
        {
            if (ut > highestScore)
            {
                highestScore = ut;
                index = utilitiesArr.IndexOf(ut);
            }
        }

        currentAction = index;
    }

    private float getEnumInputValue(InputAiEnum input)
    {
        switch (input)
        {
            case InputAiEnum.Health:
                return unit.currentHealth / unit.stats.health;
            case InputAiEnum.RangeToTargetNormalized:
                {
                    float tmp = Vector2.Distance(gameObject.transform.position, PlayerController.Instance.transform.position) / (unit.stats.moveSpeed * unit.stats.maxRange);
                    return tmp > 1 ? 1 : tmp;
                }
            case InputAiEnum.TargetHealth:
                return PlayerController.Instance.unit.currentHealth / PlayerController.Instance.unit.stats.health;

        }

        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ActionAI action in settings.actionSettingList[currentAction].actionList)
        {
            if (action != null)
            {
                action.use(this);
            }
        }
    }

    private void LateUpdate()
    {
        calcUtility();
    }
}
