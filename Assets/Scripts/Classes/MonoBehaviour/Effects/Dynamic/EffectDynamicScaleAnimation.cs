using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectDynamicScaleAnimation : EffectDynamicAnimationBase
{
    [SerializeField] private Ease hideEase = Ease.InBounce;
    [SerializeField] private Ease showEase = Ease.OutBounce;
    [SerializeField] private Vector2 scaleLimit = new Vector2(0.9f,1.1f);
    [SerializeField] private Vector2 durationLimit = new Vector2(1.0f, 1.5f);

    public override void PlayHide()
    {
        var duration = Random.Range(1.0f, 1.5f);
        transform.DOScale(Vector3.zero, duration).SetEase(hideEase).OnComplete(() => { finishAction?.Invoke(); });
    }

    public override void PlayShow()
    {
        var rand = Random.Range(scaleLimit.x, scaleLimit.y);
        var duration = Random.Range(durationLimit.x, durationLimit.y);
        transform.localScale = Vector3.one * 0.1f;
        transform.DOScale(new Vector3(rand, rand, rand), duration).SetEase(showEase);
    }
}
