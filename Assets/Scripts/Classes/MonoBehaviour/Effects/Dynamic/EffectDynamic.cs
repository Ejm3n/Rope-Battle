using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDynamic : PoolObject
{
    [SerializeField] private bool actionOnPool = true;
    [SerializeField] private bool autoPush = true;
    [SerializeField] private float timeToPush = 5f;
    private float nextPush;
    [SerializeField] private List<EffectDynamicAction> actions = new List<EffectDynamicAction>();
    
    public bool AutoPush { get => autoPush; set => autoPush = value; }
    public override void Pop()
    {
        if(actionOnPool)
        Execute();
        nextPush = timeToPush + Time.time;
    }

    public override void Push()
    {
        base.Push();
        Stop();
    }
    public void Execute()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].Execute();
        }
    }
    public void Stop()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].Stop();
        }
    }
    public virtual void Hide()
    {
        PoolManager.Default.Push(this);

    }
    private void LateUpdate()
    {
        if (autoPush)
        {
            if (Time.time >= nextPush)
                PoolManager.Default.Push(this);
        }
        else
            enabled = false;
    }
}
