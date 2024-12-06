using BG.UI.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIRopePanel : Panel
{
    private ControllerRope controllerRope;
    private bool hasController;
    [SerializeField] private UIIndicator indicator;
    [SerializeField] private UIPowerInfo power;
    [SerializeField] private UITeamInfo team;
    private void Awake()
    {
        LevelManager.Default.OnLevelLoaded += OnLevelLoaded;
        LevelManager.Default.OnLevelPreLoad += OnLevelPreLoad;
    }
    protected override void Start()
    {
        team.transform.localScale = GameData.Default.teamPowerUIScale;
        base.Start();

    }
    private void OnDestroy()
    {
        LevelManager.Default.OnLevelLoaded -= OnLevelLoaded;
        LevelManager.Default.OnLevelPreLoad -= OnLevelPreLoad;
    }
    public override void ShowPanel(bool animate = true)
    {
        base.ShowPanel(animate);
        indicator.Hide(false);
        power.Hide(false);
        //team.Hide(false);
    }
    public override void HidePanel()
    {
        base.HidePanel();
        indicator.Hide();
    }
    private void OnLevelLoaded(LevelMaster levelMaster)
    {
        var cm = levelMaster.ControllerMachine;
        if (hasController = cm.TryGetController(out controllerRope))
        {
            controllerRope.OnFinish += ControllerFinish;
            controllerRope.OnSwitchToEnemy += OnSwitchToEnemy;
            controllerRope.OnSwitchToPlayer += OnSwitchToPlayer;
            controllerRope.OnAction += OnAction;
        }
    }
    private void OnLevelPreLoad(LevelMaster levelMaster)
    {
        if (LevelManager.Default.HasCurrent && hasController)
        {
            controllerRope.OnFinish -= ControllerFinish;
            controllerRope.OnSwitchToEnemy -= OnSwitchToEnemy;
            controllerRope.OnSwitchToPlayer -= OnSwitchToPlayer;
            controllerRope.OnAction -= OnAction;
        }
    }
    private void ControllerFinish(bool win)
    {
    }
    private void OnSwitchToEnemy()
    {
        indicator.Show();
        indicator.ShowAddInfo();
        team.SetPower(controllerRope.GetMaxPower(controllerRope.RopeSide));
        //team.Show();
    }
    private void OnSwitchToPlayer()
    {
        indicator.Show();
        indicator.HideAddInfo();
        team.SetPower(controllerRope.GetMaxPower(controllerRope.RopeSide));
        //team.Show();
    }
    private void OnAction(float power, IndicatorRange.Range range)
    {
        indicator.Hide();
        indicator.HideAddInfo();
        //team.Hide();
        this.power.Show(Mathf.FloorToInt(power), range, controllerRope.RopeSide == ControllerRope.ControllerRopeSide.player);
    }
    private void LateUpdate()
    {
        if (hasController)
        {
            switch (controllerRope.RopeState)
            {
                case ControllerRope.ControllerRopeState.inInput:
                case ControllerRope.ControllerRopeState.inAuto:
                    indicator.SetInicatorInfo(controllerRope.Indicator, controllerRope.GetMaxPower(controllerRope.RopeSide));
                    //Vector3 pos = controllerRope.GetSideArea().transform.position ;
                    //pos.y = pos.y + +GameData.Default.teamPowerUIOffset.y;
                    //pos = pos+ team.transform.right *GameData.Default.teamPowerUIOffset.x;
                    //pos = pos + team.transform.forward * GameData.Default.teamPowerUIOffset.z;

                    //team.SetPosition(pos);

                    //team.SetPosition(CinemachineBrain.Default.Camera.WorldToScreenPoint(pos));
                    break;
                default:
                    break;
            }
        }
    }
}
