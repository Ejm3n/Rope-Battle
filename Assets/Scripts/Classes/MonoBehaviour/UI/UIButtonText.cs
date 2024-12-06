using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIButtonText : UIButton
{
    [SerializeField] private TextMeshProUGUI info;
    [SerializeField,TextArea] private string preText = "ADD ", afterText = " <sprite index=0>";
    public void UpdateText(string text)
    {
        info.SetText($"{preText}{text}{afterText}");
    }
}
