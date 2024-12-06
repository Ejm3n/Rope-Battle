using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FPSCounter : MonoBehaviour
{
    private int _frames;
    private TextMeshProUGUI _text;


    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(ComputeFrames());
    }

    private void Update()
    {
        _frames++;
    }


    private IEnumerator ComputeFrames() 
    {
        yield return new WaitForSeconds(1f);
        _text.text = "" + _frames;
        _frames = 0;
        StartCoroutine(ComputeFrames());
    }
}
