using UnityEngine;
using DG.Tweening;

public class UIPanelPopup : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f; // Длительность анимации
    [SerializeField] private Vector2 startPivot = new Vector2(0.5f, 0.5f); // Точка всплытия
    private RectTransform rectTransform;

    private void OnEnable()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        // Устанавливаем начальную точку (pivot)
        rectTransform.pivot = startPivot;

        // Сбрасываем размеры панели до 0
        rectTransform.localScale = Vector3.zero;

        // Анимация увеличения до полного экрана
        rectTransform.DOScale(Vector3.one, animationDuration)
            .SetEase(Ease.OutBack); // Добавляем плавный эффект
    }

    public void ShrinkAndDisable()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        // Анимация сжатия до точки
        rectTransform.DOScale(Vector3.zero, animationDuration)
            .SetEase(Ease.InBack) // Добавляем плавный эффект сжатия
            .OnComplete(() =>
            {
                gameObject.SetActive(false); // Отключаем объект после анимации
            });
    }
}
