using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    public static void SaveMaxLevel(int level)
    {
        PlayerPrefs.SetInt("MaxLevel", level);
    }

    public static int LoadMaxLevel()
    {
        return PlayerPrefs.GetInt("MaxLevel", 1);
    }

    public static void SaveBestScore(float score)
    {
        PlayerPrefs.SetFloat("BestScore", score);
    }

    public static int LoadBestScore()
    {
        return Mathf.FloorToInt(PlayerPrefs.GetFloat("BestScore", 0));
    }

    public static int LoadBattlersBought()
    {
        return PlayerPrefs.GetInt("BattlersBought", 0);
    }

    public static void AddBattlersBought(int battlers)
    {
        PlayerPrefs.SetInt("BattlersBought", LoadBattlersBought() + battlers);
    }

    public static void SaveTeamPower(int power)
    {
        PlayerPrefs.SetInt("TeamPower", power);
    }

    public static int LoadTeamPower()
    {
        return PlayerPrefs.GetInt("TeamPower", 0);
    }
}
