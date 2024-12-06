using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControllerBase : MonoBehaviour
{
    public enum ControllerState
    {
        none,
        inAppear,
        inProcess,
        inHide,
        finished
    }
    protected ControllerState state;
    [SerializeField] protected float appearDelay = 0.0f,hideDelay = 1.0f;
    private float nextDelay;
    protected bool hasInput;
    protected bool finishState = true;


    public System.Action<bool> OnFinish;
    public System.Action OnAppear;
    public System.Action OnHide;
    public ControllerState State => state;
    public bool IsWinFinish => finishState;
    public abstract void Init(LevelMaster levelMaster);
    public void Select()
    {
        StartAppear();
    }
    public void Deselect()
    {
        StartHide();
    }
    protected virtual bool StartAppear()
    {
        if (state != ControllerState.inAppear)
        {
            state = ControllerState.inAppear;
            nextDelay = Time.time + appearDelay;
            OnAppear?.Invoke();
            enabled = true;
            return true;
        }
        return false;
    }
    protected virtual bool StartHide()
    {
        if (state != ControllerState.inHide)
        {
            state = ControllerState.inHide;
            nextDelay = Time.time + hideDelay;
            OnHide?.Invoke();
            enabled = true;
            return true;
        }
        return false;

    }
    protected virtual bool HandleApear()
    {
        if (CheckDelay())
        {
            state = ControllerState.inProcess;
            OnAppear?.Invoke();
            return true;
        }
        return false;
    }
    protected virtual void HandleHide()
    {
        if (CheckDelay())
        {
            state = ControllerState.finished;
            enabled = false;
            OnFinish?.Invoke(finishState);      
        }
    }
    protected bool CheckDelay() => Time.time >= nextDelay;

    protected abstract void HandleProcess();
    protected abstract bool CheckFinish();
    [ContextMenu("Force Finish")]
    public virtual void ForceFinish()
    {
        finishState = true;
        StartHide();
    }
    public virtual void Execute()
    {
        if (enabled)
        {
            switch (state)
            {
                case ControllerState.inAppear:
                    HandleApear();
                    break;
                case ControllerState.inProcess:
                    HandleProcess();
                    break;
                case ControllerState.inHide:
                    HandleHide();
                    break;
                default:
                    break;
            }
        }
    }
    public virtual void OnPointerDown()
    {
        hasInput = true;
    }
    public virtual void OnPointerUp()
    {
        hasInput = false;
    }
}
