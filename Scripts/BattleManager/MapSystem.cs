using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapType
{
    StageMap,
    StageBossMap,
    DungeonMap
}
public class MapSystem
{
    private MapScroller[] _maps = new MapScroller[2];
    private MapScroller[] _dungeonMaps = new MapScroller[2];
    private MapScroller[] _stageBossMaps = new MapScroller[2];
    private MapType _mapType = MapType.StageMap;
    public void MapCreate()
    {
        for (int i = 0; i < 2; i++)
        {
            MapScroller map = Managers.Resource.Instantiate("Stage1Background").GetComponent<MapScroller>();
            MapScroller stageBossMaps = Managers.Resource.Instantiate("StageBossMap").GetComponent<MapScroller>();
            MapScroller dungeonMap = Managers.Resource.Instantiate("DungeonMap").GetComponent<MapScroller>();
            dungeonMap.gameObject.SetActive(false);
            stageBossMaps.gameObject.SetActive(false);
            _maps[i] = map;
            _maps[i].SetPos(i);
            _stageBossMaps[i] = stageBossMaps;
            _dungeonMaps[i] = dungeonMap;
        }
    }
    public void Reset()
    {
        MapScroller[] currentMaps = new  MapScroller[2];
        switch (_mapType)
        {
            case MapType.StageMap:
                currentMaps = _maps;
                break;
            case  MapType.StageBossMap:
                currentMaps = _stageBossMaps;
                break;
            case MapType.DungeonMap:
                currentMaps = _dungeonMaps;
                break;
        }
        for (int i = 0; i < 2; i++)
        {
            currentMaps[i].SetPos(i);
        }
    }
    public void CheckChangeMap(MapType mapType)
    {
        if (_mapType == mapType)
        {
            Reset();
            return;
        }
        switch (mapType)
        {
            case MapType.StageMap:
                for (int i = 0; i < 2; i++)
                {
                    _dungeonMaps[i].gameObject.SetActive(false);
                    _stageBossMaps[i].gameObject.SetActive(false);
                    _maps[i].gameObject.SetActive(true);
                    _maps[i].SetPos(i);
                }
                SoundManager.Instance.PlayBGM("StageBGM", 0.5f);
                break;
            case MapType.StageBossMap:
                for (int i = 0; i < 2; i++)
                {
                    _maps[i].gameObject.SetActive(false);
                    _dungeonMaps[i].gameObject.SetActive(false);
                    _stageBossMaps[i].gameObject.SetActive(true);
                    _stageBossMaps[i].SetPos(i);
                }
                SoundManager.Instance.PlayBGM("StageBossBGM", 0.5f);
                break;
            case MapType.DungeonMap:
                for (int i = 0; i < 2; i++)
                {
                    _maps[i].gameObject.SetActive(false);
                    _stageBossMaps[i].gameObject.SetActive(false);
                    _dungeonMaps[i].gameObject.SetActive(true);
                    _dungeonMaps[i].SetPos(i);
                }
                SoundManager.Instance.PlayBGM("DungeonBossBGM", 0.25f);
                break; 
        }
        _mapType = mapType;
    }
}
