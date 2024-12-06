using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    protected Button button;
    protected bool locked;
    public virtual bool Interactable 
    {
        get => button.interactable; 
        set 
        {
            if (button.interactable != value && !locked)
            {
                button.interactable = value;
                OnInteractableChange?.Invoke(value);
            }
        } 
    }
    public bool Lock { get => locked; set { locked = value; } }
    public System.Action<bool> OnInteractableChange;
    public System.Action OnClickAction;

    protected virtual void Awake()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(Click);
    }
    public void AddListener(UnityAction action)
    {
        button.onClick.AddListener(action);
    }
    public void RemoveListener(UnityAction action)
    {
        button.onClick.RemoveListener(action);
    }
    public void ReplaceListener(UnityAction action)
    {
        button.onClick.RemoveListener(Click);
        button.onClick.AddListener(action);
    }
    [ContextMenu("Click")]
    public virtual void Click()
    {
        OnClickAction?.Invoke();
    }
}