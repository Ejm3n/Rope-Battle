using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatisticsPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text levelsCompleted;
    [SerializeField] private TMP_Text bestScore;
    [SerializeField] private TMP_Text battlersPurchased;
    [SerializeField] private TMP_Text teamPower;

    private void OnEnable()
    {
        levelsCompleted.text = "Levels completed: " + SaveManager.LoadMaxLevel().ToString();
        bestScore.text = "Best score: " + SaveManager.LoadBestScore().ToString();
        battlersPurchased.text = "Battlers purchased: " + SaveManager.LoadBattlersBought().ToString();
        teamPower.text = "Team power: " + SaveManager.LoadTeamPower().ToString();
    }
}
