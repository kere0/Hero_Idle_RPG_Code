using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public Dictionary<int, MonsterData> MonsterDatas = new();
    public Dictionary<int, StageData> StageDatas = new();
  
    public SwordData[] SwordSlotData = new SwordData[16];
    public RingData[] RingSlotData = new RingData[16];
    public DungeonBossTableSO DungeonBossTableSo;
    public void StartSetResourceData()
    {
        LoadPrefabResource();
        LoadMonsterPrefabResource();
        LoadTextAssetResource();
        LoadSOResource();
        LoadAudioClipResource();
    }
    private void LoadMonsterPrefabResource()
    {
        Managers.Resource.LoadAllAsync<GameObject>("MonsterPrefab", (key, count, totalCount) => 
        {
            Debug.Log($"{key}, {count}, {totalCount}");
            if (count == totalCount)
            {
                GameManager.Instance.isMonsterDataLoaded = true;
                GameManager.Instance.LoadedStartData();
                Debug.Log("프리팹리소스 로드 완료");
            }
        });
    }
    private void LoadPrefabResource()
    {
        Managers.Resource.LoadAllAsync<GameObject>("Prefab", (key, count, totalCount) => 
        {
            Debug.Log($"{key}, {count}, {totalCount}");
            if (count == totalCount)
            {
                GameManager.Instance.isLoadedPrefab = true;
                GameManager.Instance.LoadedStartData();
                Debug.Log("프리팹리소스 로드 완료");
            }
        });
    }
    private void LoadSOResource()
    {
        Managers.Resource.LoadAllAsync<ScriptableObject>("SO", (key, count, totalCount) => 
        {
            Debug.Log($"{key}, {count}, {totalCount}");
            if (count == totalCount)
            {
                GameManager.Instance.isLoadedSO = true;
                DungeonBossTableSo = Managers.Resource.Load<DungeonBossTableSO>("DungeonBossTableSO");
                GameManager.Instance.LoadedStartData();
                Debug.Log("SO 로드 완료");
            }
        });
    }
    private void LoadTextAssetResource()
    {
        Managers.Resource.LoadAllAsync<TextAsset>("TextAsset", (key, count, totalCount) => 
        { 
            Debug.Log($"{key}, {count}, {totalCount}");
            if (count == totalCount)
            {
                Debug.Log("TextAsset 로드 완료");
                WeaponDataCSVLoad();
                RingDataCSVLoad();
                // ItemDataCSVLoad();
                // DialogueDataCSVLoad();
                MonsterDataCSVLoad();
                StageDataCSVLoad();
                GameManager.Instance.isLoadedData = true;
                GameManager.Instance.LoadedStartData();
            }
        });
    }
    private void LoadAudioClipResource()
    {
        Managers.Resource.LoadAllAsync<AudioClip>("AudioClip", (key, count, totalCount) => 
        {
            Debug.Log($"{key}, {count}, {totalCount}");
            if (count == totalCount)
            {
                GameManager.Instance.isAudioClip = true;
                GameManager.Instance.LoadedStartData();
                Debug.Log("AudioClip 로드 완료");
            }
        });
    }
    private void WeaponDataCSVLoad()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("SwordData");
        Dictionary<int, SwordData> dict = Managers.CSVLoader.LoadCSV<SwordData>(textAsset);
        foreach (SwordData itemData in dict.Values)
        {
            SwordSlotData[itemData.ItemId] = itemData;
        }
        Debug.Log("ItemData 로드 완료");
    }
    private void RingDataCSVLoad()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("RingData");
        Dictionary<int, RingData> dict = Managers.CSVLoader.LoadCSV<RingData>(textAsset);
        foreach (RingData itemData in dict.Values)
        {
            RingSlotData[itemData.ItemId] = itemData;
        }
        Debug.Log("ItemData 로드 완료");
    }
    private void MonsterDataCSVLoad()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("MonsterData"); // CSV 파일명
        MonsterDatas = Managers.CSVLoader.LoadCSV<MonsterData>(textAsset);
    }
    private void StageDataCSVLoad()
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>("StageData"); // CSV 파일명
        StageDatas = Managers.CSVLoader.LoadCSV<StageData>(textAsset);
    }
}