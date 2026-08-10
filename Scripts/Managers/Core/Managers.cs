using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance;
    private readonly PoolManager poolManager = new PoolManager();
    private readonly ResourceManager resourceManager = new ResourceManager();
    private readonly DataManager dataManager = new DataManager();
    private readonly CSVLoader csvLoader = new CSVLoader();
    private readonly PlayerManager playerManager = new PlayerManager();

    public static PoolManager Pool => Instance.poolManager;
    public static ResourceManager Resource => Instance.resourceManager;
    public static DataManager Data => Instance.dataManager;
    public static PlayerManager PlayerManager => Instance.playerManager;
    public static CSVLoader CSVLoader => Instance.csvLoader;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Clear()
    {
        poolManager.Clear();
    }
}
