using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum DungeonType
{
    GoldDungeon,
    ExpDungeon,
    EnhanceStoneDungeon
}
public class DungeonPanelUI : MonoBehaviour
{
    [SerializeField] private DungeonSlot[] _dungeonSlots = new DungeonSlot[3];
    private void Awake()
    {
        for (int i = 0; i < _dungeonSlots.Length; i++)
        {
            int dungeonLevel = Managers.PlayerManager.playerData.MaxDungeonLevel[i];
            _dungeonSlots[i].Init(this,i, dungeonLevel);
        }
    }
    public void DungeonButtonClick(int slotNum, int dungeonLevel)
    {
        switch (slotNum)
        {
            case (int)DungeonType.GoldDungeon:
                GameContainer.Instance.BattleManager.StartBossBattle(MonsterType.GoldDungeon, dungeonLevel);
                break;
            case (int)DungeonType.ExpDungeon:
                GameContainer.Instance.BattleManager.StartBossBattle(MonsterType.ExpDungeon, dungeonLevel);
                break;
            case (int)DungeonType.EnhanceStoneDungeon:
                GameContainer.Instance.BattleManager.StartBossBattle(MonsterType.EnhanceStoneDungeon, dungeonLevel);
                break;
        }
    }
}
