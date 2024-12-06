using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BG.UI.Main
{
    public enum UIState 
    {
        Showcase,
        Grid,
        Rope,
        Win, 
        Fail, 
    }

    public class UIManager : MonoBehaviour
    {
        #region Singleton
        private static UIManager _default;
        public static UIManager Default => _default;
        #endregion
        [SerializeField] private Panel _showcasePanel;
        [SerializeField] private Panel _gridPanel;
        [SerializeField] private Panel _ropePanel;
        [SerializeField] private Panel _winPanel;
        [SerializeField] private Panel _failPanel;

        private Dictionary<UIState, Panel> _stateToPanel;
        private UIState _curentState;
        public Action<UIState, UIState> OnStateChanged;
        public UIState CurentState
        {
            get => _curentState;
            set
            {
                if (_curentState != value)
                {
                    _stateToPanel[value].ShowPanel();
                    _stateToPanel[_curentState].HidePanel();
                    OnStateChanged?.Invoke(_curentState, value);
                    _curentState = value;
                }
            }
        }
        public Panel this[UIState state] { get => _stateToPanel[state]; }
        private void Awake()
        {
            _default = this;

            _stateToPanel = new Dictionary<UIState, Panel>();
            _stateToPanel.Add(UIState.Showcase, _showcasePanel);
            if (!_showcasePanel.HideOnStart)
                _curentState = UIState.Showcase;
            _stateToPanel.Add(UIState.Grid, _gridPanel);
            if (!_gridPanel.HideOnStart)
                _curentState = UIState.Grid;
            _stateToPanel.Add(UIState.Rope, _ropePanel);
            if (!_ropePanel.HideOnStart)
                _curentState = UIState.Rope;
            _stateToPanel.Add(UIState.Win, _winPanel);
            if (!_winPanel.HideOnStart)
                _curentState = UIState.Win;
            _stateToPanel.Add(UIState.Fail, _failPanel);
            if (!_failPanel.HideOnStart)
                _curentState = UIState.Fail;

        }
    }
}