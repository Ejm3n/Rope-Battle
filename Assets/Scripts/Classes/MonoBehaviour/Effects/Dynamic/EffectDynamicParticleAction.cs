using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDynamicParticleAction : EffectDynamicAction
{
    [SerializeField] private ParticleSystem particle;
    public override void Execute()
    {
        particle.Play(true);
    }

    public override void Stop()
    {
        particle.Clear(true);
    }
}
