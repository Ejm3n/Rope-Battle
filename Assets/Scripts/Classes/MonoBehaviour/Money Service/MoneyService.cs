using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyService : MonoBehaviour
{
    #region Singleton
    private static MoneyService _default;
    public static MoneyService Default => _default;
    #endregion
    private ulong bank;
    public Action OnMoneyChanged;

    private void Awake()
    {
        _default = this;
    }
    private void Start()
    {
        BankrotCheck();
    }
    public static string AmountToString(ulong amount)
    {
        string number = amount.ToString();

        if (number.Length >= 19)
        {
            number = number.Remove(number.Length - 16) + "S";
        }
        else if (number.Length >= 16)
        {
            number = number.Remove(number.Length - 13) + "Q";
        }
        else if (number.Length >= 13)
        {
            number = number.Remove(number.Length - 10) + "T";
        }
        else if (number.Length >= 10)
        {
            number = number.Remove(number.Length - 7) + "B";
        }
        else if (number.Length >= 7)
        {
            number = number.Remove(number.Length - 4) + "M";
        }
        else if (number.Length >= 4)
        {
            number = number.Remove(number.Length - 1) + "K";
        }
        else
            return number;
        number = number.Insert(number.Length - 3, ",");
        if (number[number.Length - 2].Equals('0'))
            number = number.Remove(number.Length - 2, 1);
        if (number[number.Length - 2].Equals('0'))
            number = number.Remove(number.Length - 3, 2);

        return number;
    }
    public static string NumberToString(ulong amount)
    {
        string number = amount.ToString();

        if (number.Length >= 19)
        {
            number = number.Remove(number.Length - 18) + "S";
        }
        else if (number.Length >= 16)
        {
            number = number.Remove(number.Length - 15) + "Q";
        }
        else if (number.Length >= 13)
        {
            number = number.Remove(number.Length - 12) + "T";
        }
        else if (number.Length >= 10)
        {
            number = number.Remove(number.Length - 9) + "B";
        }
        else if (number.Length >= 7)
        {
            number = number.Remove(number.Length - 6) + "M";
        }
        else if (number.Length >= 4)
        {
            number = number.Remove(number.Length - 3) + "K";
        }
        else
            return number;
        return number;
    }
    public ulong GetMoney()
    {
        return ulong.Parse(PlayerPrefs.GetString("MoneyCount", "0"));
    }
    public void BankrotCheck()
    {
        if (GetMoney() <= 0UL)
            AddMoney(GameData.Default.startMoney);
    }
    public ulong GetBank()
    {
        return bank;
    }
    public void AddBank(ulong count)
    {
        bank += count;
        OnMoneyChanged?.Invoke();
    }
    public void ReleaseBank(ulong count)
    {
        if (bank >= count)
            bank -= count;
        else
            bank = 0UL;
        AddMoney(count);

    }
    public void ReleaseBank()
    {
        ulong bank = this.bank;
        this.bank = 0UL;
        AddMoney(bank);
    }
    public void RemoveBank()
    {
        bank = 0UL;
    }
    public void AddMoney(ulong count)
    {
        ulong gm = GetMoney();
        ulong money = gm + count;
        PlayerPrefs.SetString("MoneyCount", money.ToString());
        OnMoneyChanged?.Invoke();
    }
    public void SpendMoney(ulong count)
    {
        ulong gm = GetMoney();
        ulong money = 0UL;
        if (gm >= count)
            money = gm - count;
        PlayerPrefs.SetString("MoneyCount", money.ToString());
        OnMoneyChanged?.Invoke();
    }
}
