using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMoneyPopupSound : MonoBehaviour
{
    [SerializeField] private string sound;
    [SerializeField] private UIMoneyPopup popup;

    private void Awake()
    {
        popup.OnPlay += OnPlay;
    }
    private void OnDestroy()
    {
        popup.OnPlay -= OnPlay;
    }
    private void OnPlay()
    {
        SoundHolder.Default.PlayFromSoundPack(sound);
    }
}
