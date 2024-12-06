using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

namespace BG.UI.Camera
{
    public enum CameraState { Start, Process, Player, Win, Fail }

    public class CameraSystem : MonoBehaviour
    {
        #region Singleton
        private static CameraSystem _default;
        public static CameraSystem Default => _default;
        #endregion

        [Header("Cameras")]
        [SerializeField] private CameraObject _startCamera;
        [SerializeField] private CameraObject _processCamera;
        [SerializeField] private CameraObject playerCamera;
        [SerializeField] private CameraObject _winCamera;
        [SerializeField] private CameraObject _failCamera;

        private Dictionary<CameraState, CameraObject> _stateToCamera;
        private CameraState _curentState;

        public Action<CameraState, CameraState> OnStateChanged;
        public CameraState CurentState
        {
            get => _curentState;
            set
            {
                if (_curentState != value)
                {
                    _stateToCamera[value].Select();
                    OnStateChanged?.Invoke(_curentState, value);
                    _curentState = value;
                }
            }
        }
        public CameraObject this[CameraState state]
        {
            get => _stateToCamera[state];
        }
        private void Awake()
        {
            _default = this;

            _stateToCamera = new Dictionary<CameraState, CameraObject>();
            _stateToCamera.Add(CameraState.Start, _startCamera);
            _stateToCamera.Add(CameraState.Process, _processCamera);
            _stateToCamera.Add(CameraState.Player, playerCamera);
            _stateToCamera.Add(CameraState.Win, _winCamera);
            _stateToCamera.Add(CameraState.Fail, _failCamera);
        }
    }
}