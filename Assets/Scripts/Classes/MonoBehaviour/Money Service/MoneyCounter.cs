using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private bool _coinRight;

    private TextMeshProUGUI _tmp;


    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        HandleOnMoneyUpdated();
        MoneyService.Default.OnMoneyChanged += HandleOnMoneyUpdated;
    }

    private void OnDisable()
    {
        MoneyService.Default.OnMoneyChanged -= HandleOnMoneyUpdated;
    }


    private void HandleOnMoneyUpdated() 
    {
        string text = $"{MoneyService.Default.GetMoney()}";
        if (_coinRight) text = text + "<sprite index=0>";
        else text = "<sprite index=0>" + text;

        _tmp.text = text;
    }
}
