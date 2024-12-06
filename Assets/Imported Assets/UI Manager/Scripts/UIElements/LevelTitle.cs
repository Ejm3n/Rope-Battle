using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BG.UI.Main;

namespace BG.UI.Elements
{
    public class LevelTitle : MonoBehaviour
    {
        [SerializeField] private string _titleTextBeforeNum;
        [SerializeField] private string _titleTextAfterNum;

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
            _titleUI.text = $"{_titleTextBeforeNum} {LevelManager.Default.CurrentLevelCount} {_titleTextAfterNum}";
        }

        private void HandleOnPanelHide() 
        {

        }
    }
}