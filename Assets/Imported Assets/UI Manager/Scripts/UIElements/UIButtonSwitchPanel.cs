using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BG.UI.Main;
using BG.UI.Camera;
using UnityEngine.Events;
using DG.Tweening;

namespace BG.UI.Elements
{
    public class UIButtonSwitchPanel : UIButton
    {
        enum LevelManagerAction { None, Start, Restart, Next, Prepare }


        [SerializeField] private UIState _onClickState;
        [SerializeField] private CameraState _cameraState;
        [SerializeField] private LevelManagerAction _levelManagerAction;
        private bool inTransition;

        public override void Click()
        {
            if (!inTransition)
            {
                inTransition = true;
                Action action = () =>
                {
                    switch (_levelManagerAction)
                    {
                        case LevelManagerAction.None:
                            break;
                        case LevelManagerAction.Start:
                            //LevelManager.Default.StartLevel();
                            break;
                        case LevelManagerAction.Restart:
                            LevelManager.Default.RestartLevel();
                            break;
                        case LevelManagerAction.Next:
                            LevelManager.Default.NextLevel();
                            break;
                        case LevelManagerAction.Prepare:
                            LevelManager.Default.PrepareLevel();
                            break;
                    }
                    UIManager.Default.CurentState = _onClickState;

                    if (CameraSystem.Default)
                    {
                        CameraSystem.Default.CurentState = _cameraState;
                    }
                    inTransition = false;
                };

                if (_levelManagerAction == LevelManagerAction.Next || _levelManagerAction == LevelManagerAction.Restart)
                {
                    UIManager.Default[UIManager.Default.CurentState].HidePanel();
                    LevelTransitionEffect.Default.DoTransition(action);
                }
                else
                {
                    action.Invoke();
                }
                OnClickAction?.Invoke();
            }
        }
    }
}