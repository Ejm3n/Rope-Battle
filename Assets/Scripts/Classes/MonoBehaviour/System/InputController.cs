using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class InputController : UIDrag
{
    #region Singleton
    private static InputController _default;
    public static InputController Default => _default;
    #endregion
    private void Awake()
    {
        _default = this;
    }
}