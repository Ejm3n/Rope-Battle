using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITeamInfo : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI info;

    public void SetPower(float power)
    {
        info.SetText(power.ToString());
    }
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
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
}