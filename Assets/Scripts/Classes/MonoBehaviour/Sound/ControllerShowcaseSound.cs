using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerShowcaseSound : MonoBehaviour
{
    [SerializeField] private ControllerShowcase controller;
    [SerializeField] private string sound = "Showcase";
    private void Awake()
    {
        controller.OnAllyBoss += OnAllyBoss;
    }
    private void OnDestroy()
    {
        controller.OnAllyBoss -= OnAllyBoss;
    }
    private void OnAllyBoss(Character character)
    {
        SoundHolder.Default.PlayFromSoundPack(sound);
    }
}