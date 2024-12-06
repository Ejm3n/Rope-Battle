using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RopeModel : MonoBehaviour
{
    [SerializeField] private Obi.ObiParticleAttachment[] controles;
    [SerializeField] private Obi.ObiRope obiRope;
    [SerializeField] private float stretchDuration = 0.5f;
    [SerializeField] private Vector2 stretchRange;
    [SerializeField] private Vector2 liftRange;
    private Vector3 targetMove;
    private Tween tweenStretch;

    public void SetTargetMove(int id, float shift, Vector3 direction)
    {
        targetMove = shift * direction + controles[id].target.position;
    }
    public void SetMinStretch()
    {
        float start = obiRope.stretchingScale;
        tweenStretch?.Kill();
        tweenStretch = DOTween.To(() => start,
            (v) =>
            {
                obiRope.stretchingScale = v;
            },
             stretchRange.x,
            stretchDuration);
    }
    public void SetMaxStretch()
    {
        float start = obiRope.stretchingScale;
        tweenStretch?.Kill();
        tweenStretch = DOTween.To(() => start,
            (v) =>
            {
                obiRope.stretchingScale = v;
            },
             stretchRange.y,
            stretchDuration);
    }
    public void DisableAttach(int id)
    {
        controles[id].enabled = false;
        controles[id].target.gameObject.SetActive(false);
    }
    public void LiftUp(float duration)
    {
        for (int i = 0; i < controles.Length; i++)
        {
            var target = controles[i].target;
            target?.DOKill();
            target.DOMoveY(liftRange.y, duration);
        }
    }
    public void LiftDown(float duration)
    {
        for (int i = 0; i < controles.Length; i++)
        {
            var target = controles[i].target;
            target?.DOKill();
            target.DOMoveY(liftRange.x, duration);
        }
    }
    public void Move(int id,float speed)
    {
        if ((controles[id].target.position - targetMove).sqrMagnitude <= 0.01f)
        {
            controles[id].target.position = targetMove;
        }
        else
        {
            controles[id].target.position = Vector3.MoveTowards(controles[id].target.position, targetMove, speed);
        }
        //controles[id].target.position = controles[id].target.position +(speed * direction * Time.deltaTime);
    }
}
