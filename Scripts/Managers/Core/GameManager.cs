using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title,
    Battle
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action OnResourceLoaded;
    public event Action OnGameStart;
    public bool isGameStarted = false;
    // 시작 데이터
    public bool isLoadedPrefab = false;
    public bool isMonsterDataLoaded = false;
    public bool isLoadedData = false;
    public bool isLoadedSO = false;
    public bool isAudioClip = false;
    public bool isAllStartDataLoaded = false;
    
    public GameState gameState = GameState.Title;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isGameStarted == true)
        {
            Managers.PlayerManager.PlayerTimeSystem.Update();
        }
    }
    public void LoadedStartData()
    {
        if (isLoadedPrefab && isMonsterDataLoaded && isLoadedData && isLoadedSO && isAudioClip)
        {
            isAllStartDataLoaded = true;
            OnResourceLoaded?.Invoke();
            Debug.Log("데이터 로드완료 : 게임 시작");
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            SoundManager.Instance.PlayBGM("StageBGM");
            Managers.PlayerManager.Init();
        }
    }
    public void GameStart()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        isGameStarted = true;
        OnGameStart?.Invoke();
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}