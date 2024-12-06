using System;
using System.Collections;
using System.Collections.Generic;
using BG.UI.Camera;
using DG.Tweening;
using UnityEngine;

public class LevelTransitionEffect : MonoBehaviour
{
    #region Singleton

    private static LevelTransitionEffect _default;
    public static LevelTransitionEffect Default => _default;

    #endregion

    [SerializeField] private SpriteRenderer _back;
    [SerializeField] private SpriteMask _hole;
    [SerializeField] private SpriteRenderer _front;

    private void Awake()
    {
        _default = this;
    }

    private void Start()
    {
        transform.SetParent(Camera.main.transform);
        transform.localPosition = Vector3.forward * (Camera.main.nearClipPlane + 0.01f);
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        float size = (Mathf.Tan(0.5f * Camera.main.fieldOfView) * 0.01f * 4f) * 2f;
        transform.localScale = Vector3.one * size;
    }

    public void DoTransition(Action onComplete)
    {
        // Создаём последовательность анимаций
        Sequence transitionSequence = DOTween.Sequence();

        // Анимация сжатия (_hole уменьшается до 0)
        transitionSequence.Append(_hole.transform.DOScale(0f, 1f)
            .SetEase(Ease.OutQuad));

        // Включаем _front после завершения сжатия
        transitionSequence.AppendCallback(() => _front.gameObject.SetActive(true));

        // Добавляем задержку (оставаться на месте)
        transitionSequence.AppendInterval(2f);

        // Вызываем onComplete после задержки
        transitionSequence.AppendCallback(() => onComplete?.Invoke());

        // Анимация возвращения (_hole увеличивается до 1)
        transitionSequence.Append(_hole.transform.DOScale(1f, 0.5f)
            .SetEase(Ease.InQuad));

        // После завершения всей последовательности скрываем _front
        transitionSequence.OnComplete(() => _front.gameObject.SetActive(false));
    }



}
