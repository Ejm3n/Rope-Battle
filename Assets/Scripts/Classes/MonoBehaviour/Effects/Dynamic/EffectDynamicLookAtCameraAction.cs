using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDynamicLookAtCameraAction : EffectDynamicAction
{
    public override void Execute()
    {
        Transform cam = CinemachineBrain.Default.transform;
        transform.LookAt(cam, cam.up);
    }

    public override void Stop()
    {
       
    }
}
