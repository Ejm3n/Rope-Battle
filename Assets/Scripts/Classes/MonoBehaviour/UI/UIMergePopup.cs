using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using YG;

public class UIMergePopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI nameInfo;
    [SerializeField] private TextMeshProUGUI powerInfo;
    [SerializeField] private Image background;
    [SerializeField] private Sprite def, boss;
    //[SerializeField] private Image iconInfo;
    //[SerializeField] private HorizontalLayoutGroup layout;
    ///private List<GameObject> stars = new List<GameObject>();
    [Space]
    [SerializeField] private Vector2 scaleRange;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [Space]
    [SerializeField] private float pause;
    private Sequence sequence;


    public System.Action OnHide;
    //private void Start()
    //{
    //    if (layout.transform.childCount > 0)
    //        stars.Add(layout.transform.GetChild(0).gameObject);
    //}

    public void SetInfo(Character character)
    {
        if (YandexGame.lang == "en")
            nameInfo.SetText(character.Name);
        else if (YandexGame.lang == "ru")
            nameInfo.SetText(character.NameRu);
        powerInfo.SetText(character.Power.ToString());
        if (character.IsBoss)
            background.sprite = boss;
        else
            background.sprite = def;
        // iconInfo.sprite = character.Icon;
        //int i;
        //for (i = 0; i < stars.Count && i < character.Tier; i++)
        //{
        //    stars[i].SetActive(true);
        //}
        //if (stars.Count < character.Tier)
        //{
        //    for (; i < character.Tier; i++)
        //    {
        //        stars.Add(Instantiate(stars[0], layout.transform));
        //    }
        //}
        //else
        //{
        //    for (; i < stars.Count; i++)
        //    {
        //        stars[i].SetActive(false);
        //    }
        //}
    }

    public void Show(bool anim = true)
    {
        Transform t = group.transform;
        group.DOKill();
        t.DOKill();
        sequence?.Kill();
        if (anim)
        {
            group.DOFade(1f, 0.25f);
            t.localScale = Vector3.one * scaleRange.x;
            group.alpha = 1f;
            group.alpha = 1f;
            sequence = DOTween.Sequence();
            sequence.Append(t.DOScale(scaleRange.y, duration).SetEase(ease));
            sequence.Append(DOTween.To(() => 0f, (v) => { }, 1f, pause));
            sequence.OnComplete(() => { Hide(); });
        }
        else
        {
            t.localScale = Vector3.one * scaleRange.y;
            group.alpha = 1f;
        }
    }
    public void Hide(bool anim = true)
    {
        group.DOKill();
        if (anim)
        {
            group.DOFade(0f, duration * 0.25f).OnComplete(() => { OnHide?.Invoke(); });
        }
        else
        {
            group.alpha = 0f;
            OnHide?.Invoke();
        }
    }

}
