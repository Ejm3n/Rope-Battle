using BG.UI.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWinPanel : Panel
{
    [SerializeField,Range(0f,1f)] private float moneySclae = 1f;
    [SerializeField] private UIMoneyPopup moneyPopup;
    public override void ShowPanel(bool animate = true)
    {
        base.ShowPanel(animate);
        moneyPopup.SetMoney
            (MoneyForLevel());
        moneyPopup.Show();
    }
    public override void HidePanel()
    {
        base.HidePanel();
        moneyPopup.Hide();
    }
    private ulong MoneyForLevel()
    {
        return
            (ulong)(
            GameData.Default.moneyForLevel *
            Mathf.Pow(GameData.Default.moneyForLevelMultiplier, LevelManager.Default.CurrentLevelCount) *
            (LevelManager.Default.CurrentLevelCount >= GameData.Default.equilMoneyForLevelAfterLevel ? 1f : moneySclae));
    }

}
