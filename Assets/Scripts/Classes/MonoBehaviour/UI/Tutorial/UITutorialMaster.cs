using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITutorialMaster : MonoBehaviour
{
    [SerializeField] private int maxLevel = 2;
    [SerializeField] private UITutorialCursor cursor;
    [SerializeField] private UITutorialBase[] tutorials = new UITutorialBase[0];
    private int activeTutorial = 0;
    public UITutorialBase Active => tutorials[activeTutorial];

    private void Start()
    {
        if (LevelManager.Default.CurrentLevelCount <= maxLevel &&
                activeTutorial < tutorials.Length &&
                !LevelManager.Default.IsRestart &&
                PlayerPrefs.GetInt("TutorialPassCount",0) < maxLevel &&
                GameData.Default.enableTutorial)
        {
            cursor.Hide();
            for (int i = 0; i < tutorials.Length; i++)
            {
                tutorials[i].enabled = false;
                tutorials[i].Init(cursor);

            }
            PlayerPrefs.SetInt("TutorialPassCount",PlayerPrefs.GetInt("TutorialPassCount",0)+1);
            LevelManager.Default.OnLevelStarted += OnLevelStarted;
        }
        else
        {
            gameObject.SetActive(false);
        }
        enabled = false;
    }
    private void OnDestroy()
    {
        LevelManager.Default.OnLevelStarted -= OnLevelStarted;
    }
    private void OnLevelStarted()
    {
        enabled = true;
        Active.Select();
        Active.enabled = true;
        Active.Show();
    }

    private void Next()
    {
        Active.enabled = false;    
        Active.Hide();
        activeTutorial++;
        Active.enabled = true;
        Active.Select();
        Active.Show();
    }

    private void LateUpdate()
    {
        if (activeTutorial +1 < tutorials.Length)
        {
            if (tutorials[activeTutorial].IsFinished && tutorials[activeTutorial + 1].ShowCondition())
                Next();
        }
        else
        {
            enabled = false;
        }
    }
}
