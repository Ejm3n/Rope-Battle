using BG.UI.Main;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITutorialSubMerge : UITutorialBase
{
    private Sequence anim;
    [SerializeField] private Vector3 characterOffset;
    private Vector3 cursorOffset;
    private Vector3[] points = new Vector3[2];
    private GridArea area;
    private GridMover mover;
    private UIGridPanel panel;

    public override void Init(UITutorialCursor cursor)
    {
        base.Init(cursor);
        InputController.Default.PointerDown += OnPointerDown;
        cursorOffset = new Vector2(0f, Screen.height / 8f);
        panel = UIManager.Default[UIState.Grid] as UIGridPanel;
        area = LevelManager.Default.CurrentLevel.GetArea(0);
        mover = LevelManager.Default.CurrentLevel.Mover;
        mover.OnMergeComplete += OnMerge;
    }
    private void OnDestroy()
    {
        if (mover != null)
            mover.OnMergeComplete -= OnMerge;
        InputController.Default.PointerDown -= OnPointerDown;
    }
    public override bool ShowCondition()
    {
        return area.CharactersCount >= 2;
    }
    private void OnPointerDown()
    {
        if (enabled)
            Hide();
    }

    public override void Show()
    {
        base.Show();
        anim?.Kill();
        anim = DOTween.Sequence();

        anim.Append(cursor.Transform.DOScale(cursor.Scale, 0f));
        anim.Append(cursor.Transform.DOMove(cursorOffset + points[1], 0f));
        anim.Append(cursor.PlayFade(1f, 0.2f));
        anim.Append(cursor.Transform.DOMove(points[1], 0.25f));
        anim.Append(cursor.PlayDown(0.1f));
        anim.Append(DOTween.To(() => 0f, (v) => { }, 0f, 0.15f));
        anim.Append(cursor.Transform.DOMove(points[0], 0.5f));
        anim.Append(DOTween.To(() => 0f, (v) => { }, 0f, 0.15f));
        anim.Append(cursor.PlayUp(0.1f));
        anim.Append(cursor.PlayFade(0f, 0.2f));
        anim.Append(DOTween.To(() => 0f, (v) => { }, 0f, 0.5f));
        anim.SetLoops(-1, LoopType.Restart);

    }
    public override void Hide()
    {
        base.Hide();
        anim?.Kill();
        cursor.PlayFade(0f,0.2f);
    }
    private void OnMerge(Character character, GridArea.Cell cell)
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
        panel.AddButton.Interactable = false;
        panel.AddButton.Lock = true;
        panel.StartButton.Interactable = false;
        panel.StartButton.Lock = true;

        var cam = CinemachineBrain.Default.Camera;

        for (int i = 0; i < 2; i++)
            points[i] = cam.WorldToScreenPoint(area.GetCharacter(i).transform.position + characterOffset);

    }

    public override void Deselect()
    {
        panel.AddButton.Lock = false;
        panel.AddButton.Interactable = true;
        panel.StartButton.Lock = false;
        panel.StartButton.Interactable = true;
    }
}
