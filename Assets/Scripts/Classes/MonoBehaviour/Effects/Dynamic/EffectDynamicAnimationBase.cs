using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffectDynamicAnimationBase : MonoBehaviour
{
    protected System.Action finishAction;
    public void SetFinishAction(System.Action action)
    {
        finishAction = action;
    }
    public abstract void PlayShow();
    public abstract void PlayHide();
}
