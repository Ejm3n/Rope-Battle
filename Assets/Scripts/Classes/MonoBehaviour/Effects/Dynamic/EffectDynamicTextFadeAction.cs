using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EffectDynamicTextFadeAction : EffectDynamicAction
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private AnimationCurve fadeCurve;
    private float startTime;
    public override void Execute()
    {
        enabled = true;
        text.alpha = fadeCurve.Evaluate(0.0f);
        startTime = Time.time;
    }

    public override void Stop()
    {
        text.alpha = 1.0f;
    }
    private void LateUpdate()
    {
        text.alpha = fadeCurve.Evaluate(Time.time - startTime);

    }
}
