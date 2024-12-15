using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BG.UI.Main;
using YG;

namespace BG.UI.Elements
{
    public class LevelTitle : MonoBehaviour
    {
        [SerializeField] private string _titleTextBeforeNum;
        [SerializeField] private string _titleTextAfterNum;
        [SerializeField] private string _titleTextBeforeNumRu;
        [SerializeField] private string _titleTextAfterNumRu;


        private Panel _panel;
        private TextMeshProUGUI _titleUI;

        private void Awake()
        {
            _panel = GetComponentInParent<Panel>();
            _titleUI = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _panel.onPanelShow += HandleOnPanelShow;
            HandleOnPanelShow();
        }

        private void OnDestroy()
        {
            _panel.onPanelShow += HandleOnPanelHide;
        }


        private void HandleOnPanelShow()
        {
            if (YandexGame.lang == "en")
                _titleUI.text = $"{_titleTextBeforeNum} {LevelManager.Default.CurrentLevelCount} {_titleTextAfterNum}";
            else if (YandexGame.lang == "ru")
                _titleUI.text = $"{_titleTextBeforeNumRu} {LevelManager.Default.CurrentLevelCount} {_titleTextAfterNumRu}";
        }

        private void HandleOnPanelHide()
        {

        }
    }
}