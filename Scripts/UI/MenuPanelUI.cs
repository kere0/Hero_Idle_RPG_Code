using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuType
{
    Character,
    Skill,
    Equipment,
    Dungeon,
    Shop
}
public class MenuPanelUI : MonoBehaviour
{
    [SerializeField] private Button _characterButton;
    [SerializeField] private Button _skillButton;
    [SerializeField] private Button _equipmentButton;
    [SerializeField] private Button _dungeonButton;
    [SerializeField] private Button _shopButton;
    [SerializeField] private GameObject[] panels = new GameObject[5];
    private bool _isBossBattle = false;
    private void Awake()
    {
        _characterButton.onClick.AddListener(()=>OpenPanel(MenuType.Character));
        _skillButton.onClick.AddListener(()=>OpenPanel(MenuType.Skill));
        _equipmentButton.onClick.AddListener(()=>OpenPanel(MenuType.Equipment));
        _dungeonButton.onClick.AddListener(()=>OpenPanel(MenuType.Dungeon));
        _shopButton.onClick.AddListener(()=>OpenPanel(MenuType.Shop));
    }
    private void OpenPanel(MenuType menuType)
    {
        if (_isBossBattle == true)
        {
            if(menuType == MenuType.Dungeon) return;
        }
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(i == (int)menuType);
        }
    }
    public void StartBossBattle()
    {
        _isBossBattle = true;
        panels[(int)MenuType.Dungeon].SetActive(false);
        panels[(int)MenuType.Character].SetActive(true);
    }
    public void EndBossBattle()
    {
        _isBossBattle = false;
    }
}
