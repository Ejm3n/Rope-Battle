using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIMoneyPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI info;
    [SerializeField] private Transform origin;
    [SerializeField] private MoneyAnimation moneyAnimation;
    [SerializeField, TextArea] private string preText = "+", afterText = "<sprite index=0>";
    [SerializeField] private Vector2 scaleRange;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    private bool blocked;

    public System.Action OnPlay;

    public void SetMoney(ulong money)
    {
        MoneyService.Default.AddBank(money);
        UpdateText();
    }
    private void UpdateText()
    {
        info.SetText($"{preText}{MoneyService.Default.GetBank()}{afterText}");

    }
    public void Show(bool anim = true)
    {
        blocked = false;
        Transform t = info.transform;
        group.DOKill();
        t.DOKill();
        if (anim)
        {
            group.DOFade(1f, 0.25f);
            t.localScale = Vector3.one * scaleRange.x;
            //info.color = color;
            group.alpha = 1f;
            t.DOScale(scaleRange.y, duration).SetEase(ease).OnComplete(() => { if (gameObject.activeSelf && !blocked) StartCoroutine(MoneyAnimCoroutine()); });
        }
        else
        {
            t.localScale = Vector3.one * scaleRange.y;
            group.alpha = 1f;
            if (gameObject.activeSelf && !blocked) 
                StartCoroutine(MoneyAnimCoroutine());
        }
    }
    public void Hide(bool anim = true)
    {
        group.DOKill();
        if (anim)
        {
            group.DOFade(0f, duration * 0.25f);
        }
        else
        {
            group.alpha = 0f;
        }
        ForceAddMoney();
    }

    private IEnumerator MoneyAnimCoroutine()
    {
        OnPlay?.Invoke();
        ulong animatedMoney = 0UL;
        if (MoneyService.Default.GetBank() > 25UL)
            animatedMoney = 25UL;
        else
            animatedMoney = MoneyService.Default.GetBank();

        ulong nonAnimatedMoney = MoneyService.Default.GetBank() - animatedMoney;
        for (ulong i = 0UL; i < animatedMoney; i++)
        {
            moneyAnimation.PlayMoneyUpAnim(origin.position, Random.Range(0.25f, 1.0f), 
                () => 
                { 
                    if (!blocked)
                    {
                        MoneyService.Default.ReleaseBank(1UL);
                        UpdateText();
                    }
                }, false);
            yield return new WaitForSeconds(1f / 50f);

        }
        MoneyService.Default.ReleaseBank(nonAnimatedMoney);
    }
    private void ForceAddMoney()
    {
        blocked = true;
        MoneyService.Default.ReleaseBank();
        UpdateText();
        StopCoroutine(MoneyAnimCoroutine());
    }
}
