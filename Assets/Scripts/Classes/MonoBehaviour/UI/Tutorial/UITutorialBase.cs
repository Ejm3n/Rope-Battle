using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UITutorialBase : MonoBehaviour
{
    
    private bool isActive;
    protected bool isFinished;
    protected UITutorialCursor cursor;
    [SerializeField] private bool canRepeat = true;
    [SerializeField] private float timeToRepeat = 5f;
    private float nextRepeat;
    public bool IsActive => isActive;
    public bool IsFinished => isFinished;
    public virtual void Init(UITutorialCursor cursor)
    {
        this.cursor = cursor;
    }
    public abstract bool ShowCondition();
    public abstract void Select();
    public abstract void Deselect();
    public virtual void Show()
    {
        isActive = true;
    }
    public virtual void Hide()
    {
        isActive = false;
        nextRepeat = timeToRepeat + Time.time;
    }
    public virtual void Repeat()
    {
        if (!isActive && Time.time >= nextRepeat && canRepeat)
        {
            Show();
        }
    }
    private void LateUpdate()
    {
        Repeat();
    }
}
