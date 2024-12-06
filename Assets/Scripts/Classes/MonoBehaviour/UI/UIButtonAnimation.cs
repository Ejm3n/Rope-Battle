using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIButtonAnimation : MonoBehaviour
{
    private UIButton button;
    private Tween tween;
    [SerializeField] private Transform target;
    [Header("Animation")]
    [Header("Idle")]
    [SerializeField] private float idleScale = 1.2f;
    [SerializeField] private float idleDuration = 1.0f;
    [SerializeField] private Ease idleEase = Ease.InOutBack;
    [SerializeField] private bool playOnce = false;
    [Header("Click")]
    [SerializeField] private float clickScale = 0.5f;
    [SerializeField] private float clickDuration = 0.75f;
    [SerializeField] private Ease clickEase = Ease.OutElastic;

    private void Awake()
    {
        button = GetComponent<UIButton>();
        button.OnInteractableChange += OnInteractableChange;
        button.OnClickAction += PlayClick;

    }
    private void OnDestroy()
    {
        button.OnInteractableChange -= OnInteractableChange;
        button.OnClickAction -= PlayClick;

    }
    private void Start()
    {
        PlayIdle(button.Interactable);
    }
    private void OnInteractableChange(bool interactable)
    {
        if (!playOnce)
            PlayIdle(interactable);
    }

    private void PlayIdle(bool play)
    {
        tween?.Kill();
        if (play)
        {
            target.localScale = Vector3.one;
            tween = target.DOScale(idleScale, idleDuration).SetEase(idleEase).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            tween = target.DOScale(Vector3.one, 0.2f);
        }
    }
    private void PlayClick()
    {
        tween?.Kill();
        tween = DOTween.To(() => 0f,
            (v) =>
            {
                target.localScale = Vector3.LerpUnclamped(Vector3.one * clickScale, Vector3.one, v);
            },
            1f, clickDuration).SetEase(clickEase,0.25f).OnComplete(() => { if (!playOnce) PlayIdle(button.Interactable); });
    }
}
