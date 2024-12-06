using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UITutorialCursor : MonoBehaviour
{
    private CanvasGroup cursorImage;
    private Transform cursorTransform;
    //private bool pressed;
    private Vector3 scale;
    [Space]
    [SerializeField] private Ease clickEase = Ease.OutElastic;
    private Tween tween;
    public Transform Transform => cursorTransform;
    public Vector3 Scale => scale;
    private void Awake()
    {
        cursorImage = GetComponent<CanvasGroup>();
        cursorTransform = transform;
        scale = cursorTransform.localScale;
    }
    public void Hide()
    {
        cursorImage.alpha = 0;
    }
    public Tween PlayFade(float fade, float fadeTime)
    {
       return cursorImage.DOFade(fade, fadeTime);
    }
    public Tween PlayClick(float clickTime)
    {
        this.tween?.Kill();
        Sequence tween = DOTween.Sequence();

        tween.Append(DOTween.To(() => 0f,
            (v) =>
            {
                cursorTransform.localScale = Vector3.LerpUnclamped(scale * 0.65f, scale, v);
            },
            1f, clickTime).SetEase(clickEase));
        tween.Append(cursorTransform.DOScale(scale, 0.0f));
        this.tween = tween;
        return tween;
    }
    public Tween PlayDown(float time)
    {
        tween?.Kill();

        tween = DOTween.To(() => 0f,
                (v) =>
                {
                    cursorTransform.localScale = Vector3.LerpUnclamped(scale, scale * 0.65f, v);
                },
                1f, time).SetEase(Ease.Linear);
        return tween;
    }
    public Tween PlayUp(float time)
    {
        tween?.Kill();
        tween = DOTween.To(() => 0f,
                (v) =>
                {
                    cursorTransform.localScale = Vector3.LerpUnclamped(scale * 0.65f, scale, v);
                },
                1f, time).SetEase(Ease.Linear);
        return tween;
    }
}
