using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BG.UI.Main;
using BG.UI.Camera;

namespace BG.UI.Elements
{
    public class SwitchPanelButton : MonoBehaviour
    {
        enum LevelManagerAction { None, Start, Restart, Next }


        [SerializeField] private UIState _onClickState;
        [SerializeField] private CameraState _cameraState;
        [SerializeField] private LevelManagerAction _levelManagerAction;
        private Button _button;


        private void Awake()
        {
            _button = GetComponentInChildren<Button>();
            _button.onClick.AddListener(HandleOnButtonClicked);
        }

        private void HandleOnButtonClicked()
        {
            Action action = () =>
            {
                switch (_levelManagerAction)
                {
                    case LevelManagerAction.None:
                        break;
                    case LevelManagerAction.Start:
                        LevelManager.Default.StartLevel();
                        break;
                    case LevelManagerAction.Restart:
                        LevelManager.Default.RestartLevel();
                        break;
                    case LevelManagerAction.Next:
                        LevelManager.Default.NextLevel();
                        break;
                }
                UIManager.Default.CurentState = _onClickState;
                
                if (CameraSystem.Default)
                    CameraSystem.Default.CurentState = _cameraState;
            };

            if (_levelManagerAction == LevelManagerAction.Next || _levelManagerAction == LevelManagerAction.Restart)
            {
                LevelTransitionEffect.Default.DoTransition(action);
            }
            else
            {
                action.Invoke();
            }
        }
    }
}