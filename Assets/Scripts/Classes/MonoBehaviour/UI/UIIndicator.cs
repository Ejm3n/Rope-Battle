using TMPro;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIIndicator : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI info,addInfo;
    [SerializeField] private RectTransform indicator;
    [SerializeField] private Vector2 angelRange;

    
    public void Show(bool anim = true)
    {
        group.DOKill();
        if (anim)
            group.DOFade(1f, 0.25f);
        else
            group.alpha = 1f;
    }
    public void Hide(bool anim = true)
    {
        group.DOKill();
        if (anim)
            group.DOFade(0f, 0.25f);
        else
            group.alpha = 0f;

    }
    public void SetInicatorInfo(IndicatorRange range,float maxValue)
    {
        var r = range.Evaluate();
        info.SetText(Mathf.FloorToInt(r.Value* maxValue).ToString());
        info.color = r.Color;
        indicator.localRotation =
            Quaternion.AngleAxis(
                Mathf.LerpAngle(angelRange.x, angelRange.y, range.Current),
                Vector3.forward);
    }
    public void ShowAddInfo()
    {
        addInfo.enabled = true;
    }
    public void HideAddInfo()
    {
        addInfo.enabled = false;
    }
}
