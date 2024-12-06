using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectDynamicAction : MonoBehaviour
{
    protected EffectDynamic dymanic;
    public void SetDynamic(EffectDynamic dymanic)
    {
        this.dymanic = dymanic;
    }
    public abstract void Execute();
    public abstract void Stop();
}
