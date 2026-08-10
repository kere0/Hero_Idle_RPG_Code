using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EquipmentGachaManager : MonoBehaviour
{
    public static readonly Color NormalFrameColor = new Color32(182, 182, 182, 255);
    public static readonly Color NormalBackgroundColor = new Color32(255, 255, 255, 255);
    public static readonly Color RareFrameColor = new Color32(45, 134, 255, 255);
    public static readonly Color RareBackgroundColor = new Color32(100, 165, 255, 255);
    public static readonly Color UniqueFrameColor = new Color32(132, 70, 183, 255);
    public static readonly Color UniqueBackgroundColor = new Color32(161, 132, 185, 255);
    public static readonly Color LegendFrameColor = new Color32(211, 165, 55, 255);
    public static readonly Color LegendBackgroundColor = new Color32(233, 191, 89, 255);

    [SerializeField] private EquipmentRarityTableSO equipmentRarityTableSo;
    [SerializeField] private EquipmentStarTableSO equipmentStarTableSo;

    [SerializeField] private SummonViewUI _summonViewPanel;
    private List<BaseSummonSlot> _summonSlots = new List<BaseSummonSlot>();

    public Action OnSummonStart;
    public Action OnSummonFinished;
    
    private Coroutine _summonCoroutine;
    private void PushInventory(EquipmentType equipmentType, int num)
    {
        switch (equipmentType)
        {
            case EquipmentType.Sword:
                if (Managers.PlayerManager.playerData.SwordInstances[num].IsUnlocked == false)
                {
                    Managers.PlayerManager.playerData.SwordInstances[num].IsUnlocked = true;
                }
                Managers.PlayerManager.playerData.SwordInstances[num].Count++;
                break;
            case EquipmentType.Ring:
                if (Managers.PlayerManager.playerData.RingInstances[num].IsUnlocked == false)
                {
                    Managers.PlayerManager.playerData.RingInstances[num].IsUnlocked = true;
                }
                Managers.PlayerManager.playerData.RingInstances[num].Count++;
                break;
        }
    }
    public void RequestSummon(EquipmentType equipmentType, bool isTen)
    {
        _summonViewPanel.SetSummonViewPanleType(equipmentType);
        if (_summonSlots.Count != 0)
        {
            int count = _summonSlots.Count;
            for(int i = count - 1; i >= 0; i--)
            {
                Managers.Pool.ObjPush(_summonSlots[i].gameObject);
                _summonSlots.RemoveAt(i);
            }
        }
        _summonViewPanel.gameObject.SetActive(true);
        OnSummonStart?.Invoke();

        if (isTen == true)
        {
            Managers.PlayerManager.SummonSystem.RefreshSummonGauge(equipmentType, 10);
            Managers.PlayerManager.PlayerInfoSystem.UseDiamond(SummonSystem.TenSummonDiamondCost);
            if (_summonCoroutine != null)
            {
                StopCoroutine(_summonCoroutine);
                _summonCoroutine = null;
            }
            _summonCoroutine = StartCoroutine(RequestSummonCoroutine(equipmentType));
        }
        else
        {
            Managers.PlayerManager.SummonSystem.RefreshSummonGauge(equipmentType, 1);
            Managers.PlayerManager.PlayerInfoSystem.UseDiamond(SummonSystem.OneSummonDiamondCost);
            SummonEquipment(equipmentType);
            if (equipmentType == EquipmentType.Sword)
            {
                Managers.PlayerManager.MissionSystem.IncreaseSwordSummon();
            }
            else
            {
                Managers.PlayerManager.MissionSystem.IncreaseRingSummon();
            }
            Managers.PlayerManager.SummonSystem.SetSummonCount(1);
            DOVirtual.DelayedCall(0.8f, () => OnSummonFinished?.Invoke());
        }
    }
    private IEnumerator RequestSummonCoroutine(EquipmentType equipmentType)
    {
        int summonCount = 0;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while (summonCount < 10)
        {
            summonCount++;
            SummonEquipment(equipmentType);
            if (equipmentType == EquipmentType.Sword)
            {
                Managers.PlayerManager.MissionSystem.IncreaseSwordSummon();
            }
            else
            {
                Managers.PlayerManager.MissionSystem.IncreaseRingSummon();
            }
            yield return wait;
        }
        _summonCoroutine = null;
        Managers.PlayerManager.SummonSystem.SetSummonCount(10);
        yield return new WaitForSeconds(0.8f);
        OnSummonFinished?.Invoke();
    }
    public void CloseSummonViewPanel()
    {
        if (_summonSlots.Count != 0)
        {
            int count = _summonSlots.Count;
            for(int i = count - 1; i >= 0; i--)
            {
                Managers.Pool.ObjPush(_summonSlots[i].gameObject);
                _summonSlots.RemoveAt(i);
            }
        }
        _summonViewPanel.gameObject.SetActive(false);
    }
    private void SummonEquipment(EquipmentType equipmentType)
    {
        EquipmentRarity randomRarity = GetRandomEquipmentRarity();
        int randomStarGrade = GetRandomEquipmentStar();
        BaseSummonSlot equipmentSummonSlot;
        BaseEquipmentData equipmentData;

        if (equipmentType == EquipmentType.Sword)
        {
            equipmentSummonSlot = Managers.Resource.Instantiate("SwordSummonSlot", pooling : true).GetComponent<BaseSummonSlot>();
            equipmentData = Managers.PlayerManager.EquipmentSystem.GetWeaponInfo(randomRarity, randomStarGrade).EquipmentData;
        }
        else
        {
            equipmentSummonSlot = Managers.Resource.Instantiate("RingSummonSlot", pooling : true).GetComponent<BaseSummonSlot>();
            equipmentData = Managers.PlayerManager.EquipmentSystem.GetRingInfo(randomRarity, randomStarGrade).EquipmentData;
        }
        equipmentSummonSlot.transform.SetParent(_summonViewPanel.transform);
        equipmentSummonSlot.transform.localScale = Vector3.one;
        equipmentSummonSlot.SetInfo(equipmentData.ItemId, equipmentData.StarGrade);
        _summonSlots.Add(equipmentSummonSlot);
        PushInventory(equipmentType, (int)randomRarity * 4 + randomStarGrade -1);
    }
    private EquipmentRarity GetRandomEquipmentRarity()
    {
        EquipmentRarityTableSO.RarityProbability[] rarities = equipmentRarityTableSo.rarityProbabilities;
        float total = 0;
        foreach (EquipmentRarityTableSO.RarityProbability grade in rarities)
        {
            total += grade.probability;
        }
        float random = Random.Range(0, total);
        float cumulative = 0;
        foreach (EquipmentRarityTableSO.RarityProbability grade in rarities)
        {
            cumulative += grade.probability;
            if (random < cumulative)
            {
                return grade.rarity;
            }
        }
        return rarities[0].rarity;
    }
    private int GetRandomEquipmentStar()
    {
        EquipmentStarTableSO.StarProbability[] rarities = equipmentStarTableSo.starProbabilities;
        float total = 0;
        foreach (EquipmentStarTableSO.StarProbability starGrade in rarities)
        {
            total += starGrade.probability;
        }
        float random = Random.Range(0, total);
        float cumulative = 0;
        foreach (EquipmentStarTableSO.StarProbability starGrade in rarities)
        {
            cumulative += starGrade.probability;
            if (random < cumulative)
            {
                return starGrade.starGrade;
            }
        }
        return rarities[0].starGrade;
    }
}
