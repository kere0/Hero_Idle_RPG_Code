using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem
{
    private BattleManager _battleManager;

    public SpawnSystem(BattleManager battleManager)
    {
        _battleManager = battleManager;
    }
    public void SpawnMonsterGroup(MonsterData monsterData, int monsterCount)
    {
        int hp = ValueCalculator.GetMonsterHp(Managers.PlayerManager.playerData.CurrentStage);
        _battleManager.targetList.AddRange(MonsterGroupCreate(monsterData, hp, monsterCount));
    }
    public void SpawnTreasureChest(string monsterName, Vector3 pos)
    {
        int hp = ValueCalculator.GetMonsterHp(Managers.PlayerManager.playerData.CurrentStage * 2);
        BaseMonster monster = Managers.Resource.Instantiate(monsterName, pooling : true).GetComponent<BaseMonster>();
        monster.transform.position = pos;
        monster.Init(hp, pos, 3.5f, monster.MonsterType);
        _battleManager.targetList.Add(monster);
    }

    public void SpawnDungeonBoss(MonsterData monsterData, int dungeonLevel, Vector3 pos)
    {
        int hp = ValueCalculator.GetDungeonBossHp(dungeonLevel);
        int attack = ValueCalculator.GetDungeonBossAttack(dungeonLevel);
        BaseMonster monster = CreateMonster(monsterData, hp, pos);
        monster.SetAttack(attack);
        _battleManager.targetList.Add(monster);
    }
    public void SpawnStageBoss(MonsterData monsterData, Vector3 pos)
    {
        int hp = ValueCalculator.GetStageBossHp(Managers.PlayerManager.playerData.CurrentStage);
        int attack = ValueCalculator.GetStageBossAttack(Managers.PlayerManager.playerData.CurrentStage);
        BaseMonster monster = CreateMonster(monsterData, hp, pos);
        monster.SetAttack(attack);
        _battleManager.targetList.Add(monster);
    }
    // 몬스터 그룹생성
    private List<BaseMonster> MonsterGroupCreate(MonsterData monsterData, int hp, int monsterCount)
    {
        List<BaseMonster> monsterGroup = new List<BaseMonster>();
        float offsetX = GameContainer.Instance.Player.transform.position.x;
        for (int i = 0; i < monsterCount; i++)
        {
            BaseMonster monster = Managers.Resource.Instantiate(monsterData.Name, pooling : true).GetComponent<BaseMonster>();
            
            Vector3 pos = new Vector3(offsetX + 20 + ((monsterData.Size + 1) * i), 1.05f, 0f);
            monster.transform.position = pos;
            monster.Init(hp, pos, monsterData.Size, monsterData.MonsterType);
            monsterGroup.Add(monster);
        }
        return monsterGroup;
    }
    private BaseMonster CreateMonster(MonsterData monsterData, int hp, Vector3 pos)
    {
        BaseMonster monster = Managers.Resource.Instantiate(monsterData.Name, pooling : true).GetComponent<BaseMonster>();
        monster.transform.position = pos;
        monster.Init(hp, pos, monsterData.Size, monsterData.MonsterType);
        return monster;
    }
}
