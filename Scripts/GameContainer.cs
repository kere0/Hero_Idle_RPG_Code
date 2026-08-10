using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameContainer : MonoBehaviour
{
    public static GameContainer Instance { get; private set; }
    public PlayerController Player;
    public BattleManager BattleManager;
    public CameraShakeManager CameraShakeManager; 
    public CombatSystem CombatSystem;
    public MenuPanelUI MenuPanelUI;
    public Canvas_HUD HUD;
    public AcquireInfoUI AcquireInfoUI;
    public BuffPanelUI BuffPanelUI;
    public UIFeedbackManager UIFeedbackManager;
    public ViewPotionBuffPanel ViewPotionBuffPanel;
    public SleepModeManager SleepModeManager;
    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }
}
