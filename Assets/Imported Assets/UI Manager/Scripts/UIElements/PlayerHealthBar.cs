using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BG.UI.Main;

namespace BG.UI.Elements
{
    [RequireComponent(typeof(Slider))]
    public class PlayerHealthBar : MonoBehaviour
    {
        #region Singleton
        static private PlayerHealthBar _default;
        static public PlayerHealthBar Default => _default;
        #endregion

        private Slider _slider;

        private void Awake()
        {
            _default = this;
            _slider = GetComponent<Slider>();
            gameObject.SetActive(true);
        }

        private void Start()
        {
            if (UIManager.Default)
                UIManager.Default.OnStateChanged += HandleOnUIStateChanged;
        }

        private void OnDestroy()
        {
            if (UIManager.Default) 
                UIManager.Default.OnStateChanged -= HandleOnUIStateChanged;
        }

        public void Init(float value) 
        {
            _slider.maxValue = value;
            _slider.value = value;
        }

        public void ChangeValue(float value) 
        {
            this.DOKill();
            _slider.DOValue(value, 0.2f)
                .SetTarget(this);
        }

        private void HandleOnUIStateChanged(UIState o, UIState n) 
        {
            //this.DOKill();
            //if (n == UIState.Process)
            //    transform.DOScaleX(1f, 0.5f)
            //        .SetEase(Ease.OutBack)
            //        .SetTarget(this);
            //if (o == UIState.Process)
            //    transform.DOScaleX(0f, 0.5f)
            //        .SetEase(Ease.InBack)
            //        .SetTarget(this);
        }
    }
}