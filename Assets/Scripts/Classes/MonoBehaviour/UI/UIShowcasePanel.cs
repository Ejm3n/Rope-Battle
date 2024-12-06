using BG.UI.Camera;
using BG.UI.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShowcasePanel : Panel
{
    private ControllerShowcase controllerShowcase;
    [SerializeField] private UIMergePopup mergePopup;
    [SerializeField] private UIRenderSpace renderSpace;
    private bool hasController;
    private void Awake()
    {
        LevelManager.Default.OnLevelLoaded += OnLevelLoaded;
        LevelManager.Default.OnLevelPreLoad += OnLevelPreLoad;
    }
    protected override void Start()
    {
        mergePopup.Hide(false);
        mergePopup.OnHide += renderSpace.Stop;
        base.Start();
    }
    private void OnDestroy()
    {
        LevelManager.Default.OnLevelLoaded -= OnLevelLoaded;
        LevelManager.Default.OnLevelPreLoad -= OnLevelPreLoad;
        mergePopup.OnHide -= renderSpace.Stop;
    }
    public override void ShowPanel(bool animate = true)
    {
        base.ShowPanel(animate);
    }
    public override void HidePanel()
    {
        base.HidePanel();
    }
    private void OnLevelLoaded(LevelMaster levelMaster)
    {
        var cm = levelMaster.ControllerMachine;
        if (hasController = cm.TryGetController(out controllerShowcase))
        {
            controllerShowcase.OnFinish += ControllerFinish;
            controllerShowcase.OnAllyBoss += OnAllyBoss;
        }
    }
    private void OnLevelPreLoad(LevelMaster levelMaster)
    {
        if (LevelManager.Default.HasCurrent && hasController)
        {
            controllerShowcase.OnFinish -= ControllerFinish;
            controllerShowcase.OnAllyBoss -= OnAllyBoss;
        }
    }
    private void ControllerFinish(bool win)
    {
        UIManager.Default.CurentState = UIState.Grid;
        if (LevelManager.Default.HasCurrent)
        {
            LevelManager.Default.CurrentLevel.GameStart();
            Debug.Log(gameObject.name);
            CameraSystem.Default[CameraState.Process].Select();

        }
    }
    private void OnAllyBoss(Character character)
    {
        renderSpace.RenderCharacter(character);
        mergePopup.SetInfo(character);
        mergePopup.Show();
    }
}
