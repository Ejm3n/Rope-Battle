using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDynamicAnimated : EffectDynamic
{
    [SerializeField] private EffectDynamicAnimationBase anim;

    //private void Start()
    //{
    //    anim.SetFinishAction(base.Hide);
    //}
    public override void Pop()
    {
        base.Pop();
        anim.PlayShow();
    }
    public override void Hide()
    {
        anim.PlayHide();
    }
}
