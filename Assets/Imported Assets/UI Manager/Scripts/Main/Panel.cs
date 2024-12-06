using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace BG.UI.Main
{
    [RequireComponent(typeof(CanvasGroup))]
    public class Panel : MonoBehaviour
    {
        [SerializeField] protected bool _hideOnStart;
        [SerializeField] protected float _animDuration = 0.25f;

        private CanvasGroup _group;
        
        public Action onPanelShow = () => { };
        public Action onPanelHide = () => { };

        public bool HideOnStart => _hideOnStart;
        protected virtual void Start()
        {
            _group = GetComponent<CanvasGroup>();
            gameObject.SetActive(!_hideOnStart);
            _group.blocksRaycasts = !_hideOnStart;
            if (!_hideOnStart)
                ShowPanel(false);
        }

        public virtual void ShowPanel(bool animate = true)
        {
            gameObject.SetActive(true);
            onPanelShow.Invoke();

            _group.blocksRaycasts = true;
            if (animate)
            {
                _group.alpha = 0f;
                transform.DOScale(1f, _animDuration);
                DOTween.To(
                    () => 0f,
                    (v) => _group.alpha = v,
                    1f, _animDuration);
            }
            else
            {
                transform.localScale = Vector3.one;
                _group.alpha = 1.0f;
            }
        }

        public virtual void HidePanel()
        {
            onPanelHide.Invoke();
            transform.localScale = Vector3.one;
            _group.blocksRaycasts = false;
            //transform.DOScale(1.2f, _animDuration);
            DOTween.To(
                () => 1f,
                (v) => _group.alpha = v,
                0f, _animDuration)
                    .OnComplete(() => gameObject.SetActive(false));
        }
    }
}