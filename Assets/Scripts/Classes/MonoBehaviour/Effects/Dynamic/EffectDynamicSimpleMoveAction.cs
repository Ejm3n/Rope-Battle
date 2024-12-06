using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDynamicSimpleMoveAction : EffectDynamicAction
{
    private float speed = 1.0f;
    public override void Execute()
    {
        enabled = true;
    }

    public override void Stop()
    {
        enabled = false;
    }
    private void LateUpdate()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
