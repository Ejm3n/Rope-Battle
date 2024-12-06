using BG.UI.Main;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorialSubBuy : UITutorialBase
{
    private Sequence anim;
    private Vector2 buttonPosition;
    private Vector2 cursorOffset;
    private UIGridPanel panel;
    private GridArea area;

    public override void Init(UITutorialCursor cursor)
    {
        base.Init(cursor);
        panel = UIManager.Default[UIState.Grid] as UIGridPanel;
        buttonPosition = panel.AddButton.transform.position;
        cursorOffset = buttonPosition + new Vector2(0, Screen.height / 5f);
        area = LevelManager.Default.CurrentLevel.GetArea(0);
        area.OnAdd += OnAdd;
        isFinished = area.CharactersCount > 0;
    }
    private void OnDestroy()
    {
        if (area != null)
            area.OnAdd -= OnAdd;
    }
    public override bool ShowCondition()
    {
        return true;
    }
    public override void Show()
    {
        base.Show();
        anim?.Kill();
        anim = DOTween.Sequence();

        anim.Append(cursor.Transform.DOScale(cursor.Scale, 0f));
        anim.Append(cursor.Transform.DOMove(cursorOffset, 0f));
        anim.Append(cursor.PlayFade(1f, 0.2f));
        anim.Append(cursor.Transform.DOMove(buttonPosition, 0.5f));
        anim.Append(cursor.PlayClick(0.5f));
        anim.Append(cursor.PlayFade(0f, 0.2f));
        anim.Append(DOTween.To(() => 0f, (v) => { }, 0f, 0.5f));
        anim.SetLoops(-1, LoopType.Restart);

    }
    public override void Hide()
    {
        base.Hide();
        anim?.Kill();
        cursor.PlayFade(0f, 0.2f);
    }
    private void OnAdd(Character character)
    {
        if (enabled)
        {
            Hide();
            enabled = false;
            isFinished = true;
            Deselect();
        }
    }

    public override void Select()
    {
        panel.StartButton.Interactable = false;
        panel.StartButton.Lock = true;
    }

    public override void Deselect()
    {
        panel.StartButton.Lock = false;
        panel.StartButton.Interactable = true;
    }
}

