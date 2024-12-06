using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int fallCount =5;
    protected static int fallCurrent = 0;
    private Tween tween;
    private Vector3 startPos;

    public System.Action OnPull;
    public System.Action OnFall;

    private void OnDisable()
    {
        transform?.DOKill();
        tween?.Kill();
        transform.localRotation = Quaternion.identity;
        transform.localPosition = startPos;
    }
    private void Awake()
    {
        startPos = transform.localPosition;
    }
    public void PlayAnimation(string name,float normalDuration = 0.25f,int layer = 0)
    {
        animator.CrossFade(name, normalDuration, layer);
    }


    public void PlayAppear()
    {
        var ts = transform.localScale;
        var ss = ts * 0.1f;
        transform.localScale = ss;

        tween = DOTween.To(() => 0f,
            (v) =>
            {
                transform.localScale = Vector3.Lerp(ss, ts, v);
                transform.localRotation = Quaternion.AngleAxis(Mathf.Lerp(-360, 0, v), Vector3.up);
            },
            1f, 0.5f);

        PlayAnimation("Appear");
    }
    public void PlayPull()
    {
        PlayAnimation("Pull", 0.1f);
        OnPull?.Invoke();
    }
    public void PlayPullIdle()
    { 
        PlayAnimation("PullIdle");
    }
    public void PlaySelect()
    {
        transform.DOLocalMoveY(startPos.y + 1.0f,0.25f);
        PlayAnimation("Select");
    }
    public void PlayDeselect()
    {
        transform.DOLocalMoveY(startPos.y, 0.25f);
        PlayAnimation("Idle",0.1f);
    }
    public void PlayIdle()
    {
        PlayAnimation("Idle");
    }
    public void PlayDance()
    {
        PlayAnimation("Dance");
    }
    public void PlayBack()
    {
        PlayAnimation("Back",0.1f);
    }
    public void PlayFall()
    {
        PlayAnimation("Fall"+(fallCurrent));
        fallCurrent = (fallCurrent + 1 + fallCount) % fallCount;
    }
}
