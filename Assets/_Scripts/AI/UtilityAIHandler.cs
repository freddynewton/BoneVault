using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum InputAiEnum
{
    Health,
    RangeToTargetNormalized,
    TargetHealth,
    FireBallCount,
    LivingSpawnedMinions
}

public class UtilityAIHandler : MonoBehaviour
{
    [Header("Settings")]
    public SettingsAI settings;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public EnemyUnit unit;
    [HideInInspector] public BossUdokEnemyUnit bossUdokEnemyUnit;
    [HideInInspector] public WrathEnemy wrathEnemyUnit;

    private List<float> utilitiesArr = new List<float>();
    private int currentAction;

    // Start is called before the first frame update
    private void Start()
    {
        unit = GetComponent<EnemyUnit>();
        bossUdokEnemyUnit = GetComponent<BossUdokEnemyUnit>();
        wrathEnemyUnit = GetComponent<WrathEnemy>();
        rb = GetComponent<Rigidbody2D>();
        // navAgent = GetComponent<NavMeshAgent>();
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
                return unit.currentHealth / unit.baseStats.maxHealth;

            case InputAiEnum.RangeToTargetNormalized:
                {
                    float tmp = Vector2.Distance(gameObject.transform.position, PlayerController.Instance.transform.position) / (unit.baseStats.moveSpeed * unit.enemyStats.maxRange);
                    return tmp > 1 ? 1 : tmp;
                }
            case InputAiEnum.TargetHealth:
                return PlayerController.Instance.unit.currentHealth / PlayerController.Instance.unit.baseStats.maxHealth;

            case InputAiEnum.FireBallCount:
                return bossUdokEnemyUnit.fireBalls.Count / bossUdokEnemyUnit.maxFireBalls;

            case InputAiEnum.LivingSpawnedMinions:
                if (bossUdokEnemyUnit.spawnedMinions.Count <= 0) return 0;

                return bossUdokEnemyUnit.returnLivingMinions() / bossUdokEnemyUnit.minionsLivingCount;
        }
        return 0;
    }

    // Update is called once per frame
    private void Update()
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