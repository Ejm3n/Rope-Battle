using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    #region Singleton

    private static PartyManager _default;
    public static PartyManager Default => _default;

    #endregion

    // private bool levelLoaded;
    private const string PREFS_PARTY = "PartyStruct";
    private const string PREFS_PARTY_ADD = "PartyAddStruct";
    private const string PREFS_BUY_COUNT = "UnitBuyCount";
    private const string PREFS_CHAR_POPUP = "CharPopup";
    private const string PREFS_BOSS = "BossState";
    private string lastBossSpecifer;
    private int buyCount;
    //private GridArea area;
    private void Awake()
    {
        _default = this;
        //LevelManager.Default.OnLevelLoaded += OnLevelLoaded;
        //LevelManager.Default.OnLevelStarted += OnLevelStarted;
        buyCount = GetBuyCount();
    }
    //private void OnDestroy()
    //{
    //    LevelManager.Default.OnLevelLoaded -= OnLevelLoaded;
    //    //LevelManager.Default.OnLevelStarted += OnLevelStarted;
    //}
    //private void OnLevelLoaded(LevelMaster levelMaster)
    //{
    //    area = levelMaster.GetArea(0);
    //    LoadParty(area);
    //}
    //private void OnLevelStarted()
    //{
    //    SaveParty(area);
    //}
    public void IncreaseBuyCount(int value = 1)
    {
        buyCount = GetBuyCount() + value;
        PlayerPrefs.SetInt(PREFS_BUY_COUNT, buyCount);
    }
    public void DecreaseBuyCount()
    {
        buyCount = Mathf.Max(0, GetBuyCount() - 1);
        PlayerPrefs.SetInt(PREFS_BUY_COUNT, buyCount);
    }
    public int GetBuyCount()
    {
        return PlayerPrefs.GetInt(PREFS_BUY_COUNT, 0);
    }
    public void LoadParty(GridArea area)
    {
        string party = PlayerPrefs.GetString(PREFS_PARTY, string.Empty);
        Debug.Log(party);
        if (!string.IsNullOrEmpty(party))
        {
            string[] characters = party.Split(',');
            for (int i = 0; i < characters.Length - 1; i++)
            {
                string[] cell = characters[i].Split('.');
                int id = int.Parse(cell[0]);
                string specifier = cell[1];

                if (id < area.CellCount)
                {
                    Character character = default;
                    if (CharacterHolder.Default.TryFindCharcterBySpecifier(specifier, out character))
                        area.ForceSpawnCharacterInCell(character, area.GetCell(id));
                }
            }
        }
        string add = PlayerPrefs.GetString(PREFS_PARTY_ADD, string.Empty);
        if (!string.IsNullOrEmpty(add))
        {
            string[] characters = add.Split(',');
            for (int i = 0; i < characters.Length - 1; i++)
            {
                Debug.Log("1 " + characters[i]);

                Character character = default;
                if (CharacterHolder.Default.TryFindCharcterBySpecifier(characters[i], out character))
                    area.ForceAddCharacterWithReplace(character);
            }
            PlayerPrefs.SetString(PREFS_PARTY_ADD, string.Empty);
        }
    }

    public void SaveBoss(Character boss)
    {
        string pref = PREFS_BOSS + boss.Specifier;
        lastBossSpecifer = boss.Specifier.Split('_')[0];
        if (PlayerPrefs.GetInt(pref,0) == 0)
        {
            PlayerPrefs.SetInt(pref, 1);
            PlayerPrefs.SetString(
                PREFS_PARTY_ADD,
                PlayerPrefs.GetString(PREFS_PARTY_ADD, string.Empty) + 
                $"{lastBossSpecifer},");
        }
    }
    public bool GetLastBossSpecifer(out string boss)
    {
        boss = lastBossSpecifer;
        lastBossSpecifer = string.Empty;
        return !string.IsNullOrEmpty(boss);
    }

    public void SaveParty(GridArea area)
    {
        StringBuilder strB = new StringBuilder();
        for (int i = 0; i < area.CellCount; i++)
        {
            var cell = area.GetCell(i);
            if (cell.hasCharacter)
            {
                strB.Append(i).Append('.').Append(cell.character.Specifier).Append(",");
            }
        }
        //Debug.Log(strB.ToString());
        PlayerPrefs.SetString(PREFS_PARTY, strB.ToString());
    }

    public bool CheckPopup(Character character)
    {
       return PlayerPrefs.GetInt(PREFS_CHAR_POPUP + character.Specifier, 0) == 1;
    }
    public void SetPopup(Character character)
    {
        PlayerPrefs.SetInt(PREFS_CHAR_POPUP + character.Specifier, 1);
    }
}
