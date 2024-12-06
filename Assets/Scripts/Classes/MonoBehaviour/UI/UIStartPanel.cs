using BG.UI.Elements;
using BG.UI.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStartPanel : Panel
{
    private bool isActive = true;
    [SerializeField] private UIButton startButton;
    public bool IsActive { get => isActive; set => isActive = value; }
    public UIButton StartButton => startButton;

    private void Awake()
    {
        LevelManager.Default.OnLevelLoaded += OnLevelLoaded;
    }
    protected override void Start()
    {
        base.Start();
        startButton.ReplaceListener(OnStartButtonClick);
    }

    private void OnLevelLoaded(LevelMaster levelMaster)
    {
    }

    public override void ShowPanel(bool animate = true)
    {
        base.ShowPanel(animate);
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }

    private void OnStartButtonClick()
    {
        LevelManager.Default.CurrentLevel.GameStart();
        startButton.Click();
    }
}
