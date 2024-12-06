using BG.UI.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRope : ControllerBase
{
    public enum ControllerRopeState
    {
        none,
        inInput,
        inAuto,
        inMove,
        inSwitch,
        inPrepare,
        inFinish
    }
    public enum ControllerRopeSide
    {
        player = 0,
        enemy = 1,
    }
    private LevelMaster levelMaster;
    private RopeHolder ropeHolder;
    [SerializeField] private IndicatorRange range = new IndicatorRange();
    private ControllerRopeState ropeState;
    private ControllerRopeSide ropeSide;
    //[SerializeField] private CameraObject[] cameras;
    private float autoDelay;
    private float nextPrepare;
    private float powerPercent;

    public System.Action OnSwitchToPlayer;
    public System.Action OnSwitchToEnemy;
    public System.Action OnSwitchStart;
    public System.Action<float, IndicatorRange.Range> OnAction;
    public ControllerRopeState RopeState => ropeState;
    public ControllerRopeSide RopeSide => ropeSide;
    public IndicatorRange Indicator => range;

    public float GetMaxPower(ControllerRopeSide side)
    {
        return ropeHolder.GetMaxPower((int)side);
    }

    public override void Init(LevelMaster levelMaster)
    {
        this.levelMaster = levelMaster;
        ropeHolder = levelMaster.RopeHolder;
        //range = new IndicatorRange();
    }
    public GridArea GetSideArea()
    {
        if (ropeSide == ControllerRopeSide.player)
            return levelMaster.GetArea(0);
        else
            return levelMaster.GetArea(1);
    }
    protected override bool StartAppear()
    {
        if (base.StartAppear())
        {
            ropeHolder.DistributeCharactersFromAreas();
            InitSwitchToPlayer();
            return true;
        }
        return false;
    }
    protected override bool HandleApear()
    {
        if (HandleSideChange() && CheckDelay())
        {
            return base.HandleApear();
        }
        return false;
    }
    protected override bool CheckFinish()
    {
        for (int i = 0; i < ropeHolder.Count; i++)
        {
            if (ropeHolder[i].IsPushedRight)
                return false;
        }
        return true;
    }
    private bool HandleSideChange()
    {
        if (ropeState == ControllerRopeState.inSwitch)
        {
            //var cam = cameras[0];
            Vector3 dist =
                CameraSystem.Default[CameraState.Player].transform.position -
                CinemachineBrain.Default.transform.position;
            if (dist.sqrMagnitude <= 0.1f)
            {
                if (ropeSide == ControllerRopeSide.player)
                {
                    ropeState = ControllerRopeState.inInput;
                    OnSwitchToPlayer?.Invoke();
                }
                else
                {
                    ropeState = ControllerRopeState.inAuto;
                    OnSwitchToEnemy?.Invoke();
                }
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }

    }
    public override void OnPointerDown()
    {
        base.OnPointerDown();
    }
    private void InitSwitchToPlayer()
    {
        ropeState = ControllerRopeState.inSwitch;
        ropeSide = ControllerRopeSide.player;
        CameraSystem.Default.CurentState = CameraState.Player;
        //cameras[0].Select();
        OnSwitchStart?.Invoke();
    }
    private void InitSwitchToEnemy()
    {
        ropeState = ControllerRopeState.inSwitch;
        ropeSide = ControllerRopeSide.enemy;
        CameraSystem.Default.CurentState = CameraState.Player;
        range.SetRandomStart();
        OnSwitchStart?.Invoke();
        var r = GameData.Default.enemyTimeToActionRange;
        autoDelay = Time.time + Random.Range(r.x, r.y);
    }
    protected override void HandleProcess()
    {

        switch (ropeState)
        {
            case ControllerRopeState.inInput:
                {

                    if (hasInput)
                    {
                        var r = range.Evaluate();
                        powerPercent = r.Value;
                        int side = (int)ControllerRopeSide.player;
                        nextPrepare = Time.time + GameData.Default.ropePrepareTime;
                        ropeState = ControllerRopeState.inPrepare;

                        for (int i = 0; i < ropeHolder.Count; i++)
                        {
                            var rope = ropeHolder[i];
                            if (!rope.IsPushed)
                            {
                                rope.PrepareMove();
                                rope.PlayPull(side);
                            }
                        }
                        //int opSide = (int)ControllerRopeSide.enemy;
                        //for (int i = 0; i < ropeHolder.Count; i++)
                        //{
                        //    var rope = ropeHolder[i];
                        //    if(!rope.IsPushed)
                        //    {
                        //        float power = powerPercent * (rope.GetPower(side) / rope.GetPower(opSide)) * GameData.Default.powerScale;
                        //        Debug.Log("Player Power " + power);
                        //        rope.StartMove(side ,power );
                        //    }
                        //}
                        OnAction?.Invoke(powerPercent * ropeHolder.GetMaxPower(side), r);
                        if (SaveManager.LoadBestScore() < powerPercent * ropeHolder.GetMaxPower(side))
                        {
                            SaveManager.SaveBestScore(powerPercent * ropeHolder.GetMaxPower(side));
                        }
                        range.Reset();
                        hasInput = false;
                        //ropeState = ControllerRopeState.inMove;
                        //Debug.Break();
                    }
                    else
                    {
                        range.Update();
                    }
                }
                break;
            case ControllerRopeState.inAuto:
                {
                    if (Time.time >= autoDelay)
                    {
                        var r = range.Evaluate();
                        powerPercent = r.Value;
                        int side = (int)ControllerRopeSide.enemy;
                        nextPrepare = Time.time + GameData.Default.ropePrepareTime;
                        ropeState = ControllerRopeState.inPrepare;
                        for (int i = 0; i < ropeHolder.Count; i++)
                        {
                            var rope = ropeHolder[i];
                            if (!rope.IsPushed)
                            {
                                rope.PrepareMove();
                                rope.PlayPull(side);
                            }
                        }
                        //for (int i = 0; i < ropeHolder.Count; i++)
                        //{
                        //    var rope = ropeHolder[i];
                        //    if (!rope.IsPushed)
                        //    {
                        //        int opSide = (int)ControllerRopeSide.player;
                        //        float power = powerPercent * (rope.GetPower(side) / rope.GetPower(opSide)) * GameData.Default.powerScale;
                        //        Debug.Log("Enemy Power " + power);
                        //        rope.StartMove(side, power);
                        //    }
                        //}
                        OnAction?.Invoke(powerPercent * ropeHolder.GetMaxPower(side), r);
                        autoDelay = 0f;
                        range.Reset();
                        //ropeState = ControllerRopeState.inMove;
                    }
                    else
                    {
                        range.Update();
                    }
                }
                break;
            case ControllerRopeState.inFinish:
                break;
            case ControllerRopeState.inMove:
                // Debug.Log(ropeHolder.InMove);
                if (!ropeHolder.InMove)
                {
                    if (ropeHolder.CheckFinish() || ropeHolder.CheckEnemyFinish())
                    {
                        finishState = CheckFinish();
                        StartHide();
                    }
                    else
                    {
                        if (ropeSide == ControllerRopeSide.player)
                            InitSwitchToEnemy();
                        else
                            InitSwitchToPlayer();
                    }
                }
                break;
            case ControllerRopeState.inSwitch:
                HandleSideChange();
                break;
            case ControllerRopeState.inPrepare:
                if (Time.time >= nextPrepare)
                {
                    int side = (int)ropeSide;
                    if (side == 0)
                    {
                        int opSide = 1;
                        for (int i = 0; i < ropeHolder.Count; i++)
                        {
                            var rope = ropeHolder[i];
                            if (!rope.IsPushed)
                            {
                                float power = powerPercent * (rope.GetPower(side) / rope.GetPower(opSide)) * GameData.Default.powerScale;
                                Debug.Log("Player Power " + power);
                                rope.StartMove(side, power);
                            }
                        }
                    }
                    else
                    {
                        int opSide = 0;
                        for (int i = 0; i < ropeHolder.Count; i++)
                        {
                            var rope = ropeHolder[i];
                            if (!rope.IsPushed)
                            {
                                float power = powerPercent * (rope.GetPower(side) / rope.GetPower(opSide)) * GameData.Default.powerScale;
                                Debug.Log("Enemy Power " + power);
                                rope.StartMove(side, power);
                            }
                        }
                    }
                    ropeState = ControllerRopeState.inMove;
                }
                break;
            default:
                break;
        }

    }
}
[System.Serializable]
public class IndicatorRange
{
    private float current;
    private bool dir;
    [SerializeField] private Range[] ranges;
    public float Current => current;
    [System.Serializable]
    public class Range
    {
        [SerializeField] private float from, to;
        [SerializeField] private float value;
        [SerializeField] private Sprite[] labels;
        [SerializeField] private Color color;
        public Color Color { get => color; }
        public float Value { get => value; }

        //public Range(float from, float to, float value,int rating, Color color)
        //{
        //    this.from = from;
        //    this.to = to;
        //    this.value = value;
        //    this.color = color;
        //}
        public bool Check(float percent)
        {
            return percent >= from && percent < to;
        }
        public Sprite GetLabel() => labels[Random.Range(0, labels.Length)];
    }
    //public IndicatorRange()
    //{
    //    ranges = new Range[5];
    //    ranges[0] = new Range(-0.1f, 0.2f, 0.25f,0,Color.red);
    //    ranges[1] = new Range(0.2f, 0.4f, 0.5f, 1, Color.yellow);
    //    ranges[2] = new Range(0.4f, 0.6f, 1.0f, 2, Color.green);
    //    ranges[3] = new Range(0.6f, 0.8f, 0.5f, 1, Color.yellow);
    //    ranges[4] = new Range(0.8f, 1.1f, 0.25f, 0, Color.red);
    //}
    public void Update()
    {
        if (dir)
        {
            current -= Time.deltaTime * GameData.Default.indicatorSpeed;
            if (current <= 0.0f)
            {
                current = 0f;
                dir = false;
            }
        }
        else
        {
            current += Time.deltaTime * GameData.Default.indicatorSpeed;
            if (current >= 1.0f)
            {
                current = 1f;
                dir = true;
            }
        }
    }
    public Range Evaluate()
    {
        for (int i = 0; i < ranges.Length; i++)
        {
            if (ranges[i].Check(current))
            {
                return ranges[i];
            }
        }
        return new Range();
    }
    public Range Evaluate(float percent)
    {
        for (int i = 0; i < ranges.Length; i++)
        {
            if (ranges[i].Check(percent))
            {
                return ranges[i];
            }
        }
        return new Range();
    }
    public void Reset()
    {
        dir = false;
        current = 0f;
    }
    public void SetRandomStart()
    {
        dir = (int)Random.value == 1;
        current = Random.value;
    }
}