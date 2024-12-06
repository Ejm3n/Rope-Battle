using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRopeSound : MonoBehaviour
{
    [SerializeField] private string sound;
    [SerializeField] private ControllerRope controller;

    private void Awake()
    {
        controller.OnAction += OnAction;
    }
    private void OnDestroy()
    {
        controller.OnAction -= OnAction;
    }
    private void OnAction(float p , IndicatorRange.Range r)
    {
        SoundHolder.Default.PlayFromSoundPack(sound);
    }
}
