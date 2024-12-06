using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class UIPowerInfo : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI info;
    [SerializeField] private Image label;
    [SerializeField] private Vector2 scaleRange;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    private Sequence sequence;
    public Tween Show(float value, IndicatorRange.Range range, bool showLabel)
    {
        Transform t = info.transform;

        info.SetText(value.ToString());
        t.localScale = Vector3.one * scaleRange.x;
        group.DOKill();
        t.DOKill();
        //info.color = color;
        group.alpha = 1f;
        sequence?.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(t.DOScale(scaleRange.y, duration).SetEase(ease));
        sequence.Append(DOTween.To(() => 0f, (v) => { }, 1f, duration*0.25f));
        sequence.Append(Hide());
        if (showLabel)
        {
            label.enabled = true;
            label.sprite = range.GetLabel();
        }else
            label.enabled = false;
        return sequence;
    }

    public Tween Hide()
    {
        group.DOKill();
        return group.DOFade(0, duration * 0.25f);
    }
    public void Hide(bool direct)
    {
        group.alpha = 0f;
    }
}
