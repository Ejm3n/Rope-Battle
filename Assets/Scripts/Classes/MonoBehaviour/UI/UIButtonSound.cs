using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonSound : MonoBehaviour
{
    private UIButton button;
    [SerializeField] private string sound = "UIClick";

    private void Awake()
    {
        button = GetComponent<UIButton>();
        button.OnClickAction += OnClickAction;
    }
    private void OnDestroy()
    {
        button.OnClickAction -= OnClickAction;
    }
    private void OnClickAction()
    {
        SoundHolder.Default.PlayFromSoundPack(sound);

    }
}
