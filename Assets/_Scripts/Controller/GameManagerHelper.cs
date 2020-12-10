using UnityEngine;

public class GameManagerHelper : MonoBehaviour
{
    public static GameManagerHelper Instance { get; private set; }

    public void resetAll()
    {
        Inventory.Instance.setBones(0);

        PlayerController.Instance.unit.upgradeHandler.resetUpgrades();
        PlayerController.Instance.unit.setHealthPlayer(PlayerController.Instance.unit.baseStats.maxHealth);

        WorldGeneratorManager.Instance.clearWorld();
        WorldGeneratorManager.Instance.miniMapGenerator.clearMinimap();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Disables all Logs in Builds to save performance
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }
}