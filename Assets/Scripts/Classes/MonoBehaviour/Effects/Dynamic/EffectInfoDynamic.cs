using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectInfoDynamic : EffectDynamic
{
  [SerializeField]  private TextMeshPro info;
    public void SetText(string text)
    {
        info.SetText(text);
    }
}
