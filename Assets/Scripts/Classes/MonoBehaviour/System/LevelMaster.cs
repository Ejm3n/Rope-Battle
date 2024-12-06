using BG.UI.Camera;
using BG.UI.Main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaster : MonoBehaviour
{
    public enum LevelState
    {
        none,
        start,
        process,
        win,
        over
    }
    private LevelState state;
    [SerializeField] private ControllerStateMachine controllers;
    [Space]
    //[SerializeField] private float moveDistance = 4f;
    [Header("Grid")]
    [SerializeField] private GridArea[] areas;
    [SerializeField] private GridMover mover;
    [SerializeField] private RopeHolder ropeHolder;
    public LevelState State => state;
    public ControllerStateMachine ControllerMachine => controllers;
    public GridMover Mover => mover;
    public RopeHolder RopeHolder => ropeHolder;

    public System.Action OnStart;
    public System.Action OnFinish;
    //public float MoveDistance => moveDistance;
    public GridArea GetArea(int id)
    {
        return areas[id];
    }
    public void Init()
    {
        for (int i = 0; i < areas.Length; i++)
            areas[i].Init();
        PartyManager.Default.LoadParty(areas[0]);
        PartyManager.Default.SaveParty(areas[0]);
        ropeHolder.Init(areas);
        mover.SetArea(areas[0], ropeHolder);
        controllers.Init(this);
        controllers.OnFinish += GameFinish;
        //state = LevelState.process;
        //LevelManager.Default.StartLevel();
        controllers.Execute();
    }
    private void OnDestroy()
    {
        controllers.OnFinish -= GameFinish;
    }
    public void GameStart()
    {
        state = LevelState.start;
        CameraSystem.Default.CurentState = CameraState.Process;
        enabled = true;
        OnStart?.Invoke();
    }

    public void GameFinish(bool isWin)
    {
        if (isWin)
        {
            SaveManager.SaveMaxLevel(SaveManager.LoadMaxLevel() + 1);
            GameWin();
        }
        else
            GameOver();
        OnFinish?.Invoke();
    }

    [ContextMenu("Win")]
    private void GameWin()
    {
        state = LevelState.win;
        controllers.Stop();
        UIManager.Default.CurentState = UIState.Win;
        CameraSystem.Default.CurentState = CameraState.Win;
        Character boss;
        if (areas[1].GetBoss(out boss))
            PartyManager.Default.SaveBoss(boss);
    }
    private void GameOver()
    {
        state = LevelState.over;
        controllers.Stop();
        UIManager.Default.CurentState = UIState.Fail;
        CameraSystem.Default.CurentState = CameraState.Fail;
    }
    private bool CheckStart()
    {
        Vector3 dist =
            CameraSystem.Default[CameraState.Process].
            transform.position -
            CinemachineBrain.Default.transform.position;
        if (dist.sqrMagnitude <= 0.1f)
        {
            state = LevelState.process;
            LevelManager.Default.StartLevel();
            //scontrollers.Execute();
            return true;
        }
        return false;
    }
    private void LateUpdate()
    {
        if (state != LevelState.start || CheckStart())
            enabled = false;
    }
    //private void OnDrawGizmos()
    //{
    //    if(areas != null)
    //    {
    //        for (int i = 0; i < length; i++)
    //        {

    //        }
    //    }
    //}
}
