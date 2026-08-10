using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class BattleManager : MonoBehaviour
{
    private Vector3 startPos = new Vector3(-2.75f, 1.05f, 0);
    public List<BaseMonster> targetList = new List<BaseMonster>();
    public Action<BaseMonster> OnMonsterDead;
    public Action<float> OnPlayerDead;
    private int _currentProgress = 0;
    public Action<int> OnMonsterGroupClear;
    private bool _isStageClear = false;
    
    // System
    private MapSystem _mapSystem;
    private SpawnSystem _spawnSystem;
    public RewardSystem RewardSystem;
    
    private Sequence _seq;
    private bool _isBossBattle = false;
    private readonly int _bossBattleTimeLimit = 30;
    private float _bossBattleTimer = 0;
    public bool isBattleStart = false;

    private StageData _currentStageData;
    private Dictionary<int, MonsterData> _monsterDatas = new();

    private void Awake()
    {
        GameManager.Instance.OnGameStart += Init;
        OnMonsterDead += MonsterDead;
        OnPlayerDead += BossBattleEnd;
        // System
        _mapSystem = new MapSystem();
        _spawnSystem =  new SpawnSystem(this);
        RewardSystem =  new RewardSystem(this);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(isBattleStart + "isBattleStart");
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Managers.PlayerManager.PlayerInfoSystem.LevelUp();
        }
        UpdateBossBattleTime();
    }
    private void Init()
    {
        _mapSystem.MapCreate();
        var vcam = FindObjectOfType<CinemachineVirtualCamera>();
        PlayerController go = Managers.Resource.Instantiate("Player").GetComponent<PlayerController>();
        GameContainer.Instance.Player = go;
        go.transform.position = startPos;
        vcam.Follow = go.transform;
        // 스테이지 정보
        StageInfoRefresh();
        // 몬스터 정보
        _monsterDatas = Managers.Data.MonsterDatas;
        _spawnSystem.SpawnMonsterGroup(_monsterDatas[_currentStageData.NormalMonsterId], 5);
        // PoolInit();
        _seq = DOTween.Sequence();
        _seq.AppendInterval(0.5f);
        _seq.Append(FadeManager.Instance.FadeOut(1f));
        isBattleStart = true; 
    }
    private void StageInfoRefresh()
    {
        _currentStageData = Managers.Data.StageDatas[(Managers.PlayerManager.playerData.CurrentStage-1) % 5 + 1];
    }
    public void StartBossBattle(MonsterType monsterType, int dungeonLevel = 0)
    {
        isBattleStart = false;
        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(ClearBattleField(0, 1,FadeType.Boss));
        // 필드 초기화 되면 보스 생성
        _seq.AppendCallback(() =>
        {
            GameContainer.Instance.MenuPanelUI.StartBossBattle();
            float offsetX = GameContainer.Instance.Player.transform.position.x;
            Vector3 pos = new Vector3(offsetX + 27f, 1.05f, 0);
            _bossBattleTimer = _bossBattleTimeLimit;
            GameContainer.Instance.HUD.RefreshTimeProgressBar(_bossBattleTimer, _bossBattleTimeLimit);
            if (monsterType == MonsterType.StageBoss)
            {
                GameContainer.Instance.HUD.StartBossBattle(false);
                _mapSystem.CheckChangeMap(MapType.StageBossMap);
                MonsterData monsterData = _monsterDatas[_currentStageData.BossMonsterId];
                _spawnSystem.SpawnStageBoss(monsterData, pos);
            }
            else
            {
                GameContainer.Instance.HUD.StartBossBattle(true);
                _mapSystem.CheckChangeMap(MapType.DungeonMap);
                MonsterData monsterData = _monsterDatas[Managers.Data.DungeonBossTableSo.DungeonBossData[(int)monsterType - 3].monsterId];
                Debug.Log(monsterData.Name + "보스이름");
                Debug.Log("보스이름");
                _spawnSystem.SpawnDungeonBoss(monsterData, dungeonLevel, pos);
            }
        });
        _seq.AppendInterval(0.5f);
        _seq.Append(FadeManager.Instance.FadeOut(1.5f));
        _seq.AppendCallback(()=>
        {
            _isBossBattle = true;
            isBattleStart = true;
        });
    }
    private void UpdateBossBattleTime()
    {
        if (_isBossBattle == true)
        {
            _bossBattleTimer -= Time.deltaTime; 
            GameContainer.Instance.HUD.RefreshTimeProgressBar(_bossBattleTimer, _bossBattleTimeLimit);
            if (_bossBattleTimer <= 0)
            {
                GameContainer.Instance.Player.ForceKill();
                BossBattleEnd(2f);
            }
        }
    }
    public void ReStart(float delayTime, float fadeInTime, Action action = null)
    {
        isBattleStart = false;
        _seq?.Kill();
        _seq = DOTween.Sequence();
        _seq.Append(ClearBattleField(delayTime, fadeInTime, FadeType.StageClear));
        // 필드 초기화 되면 몬스터 생성
        _seq.AppendInterval(0.7f);
        _seq.AppendCallback(() =>
        {
            action?.Invoke();
            _mapSystem.CheckChangeMap(MapType.StageMap);
            GameContainer.Instance.HUD.RefreshStageLevel();
            _spawnSystem.SpawnMonsterGroup(_monsterDatas[_currentStageData.NormalMonsterId], 5);
        });
        _seq.Append(FadeManager.Instance.FadeOut(1f));
        _seq.AppendCallback(() => isBattleStart = true);
    }
    // 필드 정리
    private Tween ClearBattleField(float delayTime, float fadeInTime, FadeType fadeType)
    {
        Sequence clearSeq = DOTween.Sequence();
        clearSeq.AppendInterval(delayTime);
        clearSeq.Append(FadeManager.Instance.FadeIn(fadeInTime, fadeType));
        // Fade 끝나면 필드 초기화
        clearSeq.AppendCallback(InitObject);
        return clearSeq;
    }
    private void InitObject()
    {
        // 오브젝트 제거
        if (targetList.Count != 0)
        {
            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                Managers.Pool.ObjPush(targetList[i].gameObject);
                targetList.RemoveAt(i);
            }
        }
        _isStageClear = false;
        OnMonsterGroupClear.Invoke(0);
        GameContainer.Instance.Player.transform.position = startPos;
        GameContainer.Instance.Player.Reset();
        Managers.PlayerManager.BuffSystem.ResetBuffs();
        GameContainer.Instance.BuffPanelUI.BuffSlotsReset();
        Managers.PlayerManager.SkillSystem.Reset();
        _currentProgress = 0;
    }
    private void MonsterDead(BaseMonster monster)
    {
        if (GameContainer.Instance.Player.CurrentTarget == monster)
        {
            GameContainer.Instance.Player.CurrentTarget = null;
        }
        targetList.Remove(monster);
        // 보상
        int rewardValue = RewardSystem.MonsterReward(monster);
        Managers.PlayerManager.MissionSystem.IncreaseMonsterKill();
        if (monster.MonsterType == MonsterType.Normal)
        {
            StageClearCheck();
        }
        else if (monster.MonsterType == MonsterType.TreasureChest)
        {
            ReStart(2f, 1f);
        }
        else if (monster.MonsterType == MonsterType.StageBoss)
        {
            
            Managers.PlayerManager.PlayerInfoSystem.StageUp();
            StageInfoRefresh();
            BossBattleEnd(2f);
        }
        // 던전 보스
        else
        {
            BossBattleEnd(4f);
            GameContainer.Instance.HUD.ViewDungeonRewardPanel(monster.MonsterType, rewardValue);
        }
    }

    public void RunBossBattle()
    {
        _isBossBattle = false;
        ReStart(0f, 0.2f, ()=>
        {
            GameContainer.Instance.MenuPanelUI.EndBossBattle();
            GameContainer.Instance.HUD.EndBossBattle();
        });
    }
    private void BossBattleEnd(float delayTime)
    {
        _isBossBattle = false;
        ReStart(delayTime, 1f, ()=>
        {
            GameContainer.Instance.MenuPanelUI.EndBossBattle();
            GameContainer.Instance.HUD.EndBossBattle();
        });
    }
    private void StageClearCheck()
    {
        // 리스트가 0이되면 다음 몬스터 그룹생성
        if (targetList.Count == 0)
        {
            // 진행도
            _currentProgress += 50;
            OnMonsterGroupClear.Invoke(_currentProgress);
            if (_currentProgress < 100)
            {
                _spawnSystem.SpawnMonsterGroup(_monsterDatas[_currentStageData.NormalMonsterId], 5);
            }
            else
            {
                if (_isStageClear == false)
                {
                    _isStageClear = true;
                    Vector3 createPos = new Vector3(GameContainer.Instance.Player.transform.position.x + 20, 1.05f, 0f);
                    _spawnSystem.SpawnTreasureChest("TreasureChest", createPos);
                }
            }
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnGameStart -= Init;
        OnMonsterDead -= MonsterDead;
        OnPlayerDead -= BossBattleEnd;
    }
}
