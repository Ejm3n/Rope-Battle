using BG.UI.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIProcessPanel : Panel
{
    private void Awake()
    {
        LevelManager.Default.OnLevelLoaded += OnLevelLoaded;
        LevelManager.Default.OnLevelPreLoad += OnLevelPreLoad;
    }
    private void OnDestroy()
    {
        LevelManager.Default.OnLevelLoaded -= OnLevelLoaded;
        LevelManager.Default.OnLevelPreLoad -= OnLevelPreLoad;
    }
    public override void ShowPanel(bool animate = true)
    {
        base.ShowPanel(animate);
    }
    private void OnLevelLoaded(LevelMaster levelMaster)
    {
    }
    private void OnLevelPreLoad(LevelMaster levelMaster)
    {
        if (LevelManager.Default.HasCurrent)
        {


        }
    }
}
