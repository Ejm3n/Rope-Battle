using BG.UI.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerStateMachine : MonoBehaviour
{
   [SerializeField] private ControllerBase[] controllers;
    private int current = -1;

    public System.Action OnPreChange;
    public System.Action<ControllerBase> OnChange;
    public System.Action<bool> OnFinish;
    public ControllerBase Current => controllers[current];
    private void Awake()
    {
        for (int i = 0; i < controllers.Length; i++)
            controllers[i].enabled = false;
    }
    private void Start()
    {

        InputController.Default.PointerDown += OnPointerDown;
        InputController.Default.PointerUp += OnPointerUp;
    }
    private void OnDestroy()    
    {
        for (int i = 0; i < controllers.Length; i++)
            UnsetController(controllers[i]);
        InputController.Default.PointerDown += OnPointerDown;
        InputController.Default.PointerUp += OnPointerUp;

    }
    public bool TryGetController<T>(out T controller ) where T : ControllerBase
    {
        controller = null;
        for (int i = 0; i < controllers.Length; i++)
        {
            if(controllers[i] is T)
            {
                controller = controllers[i] as T;
                return true;
            }
        }
        return false;
    }
    public void Execute()
    {

        current = 0;
        enabled = true;
        SetController(Current);
        Current.Select();
    }
    public void Stop()
    {
        for (int i = 0; i < controllers.Length; i++)
            UnsetController(controllers[i]);
    }
    public void Init(LevelMaster levelMaster)
    {
        for (int i = 0; i < controllers.Length; i++)
            controllers[i].Init(levelMaster);
    }
    private void SetController(ControllerBase controller)
    {
        controller.OnAppear += OnControllerAppear;
        controller.OnFinish += OnControllerFinish;
        controller.OnHide += OnControllerHide;  
        controller.enabled = true;
        OnChange?.Invoke(controller);
    }
    private void UnsetController(ControllerBase controller)
    {
        controller.OnAppear -= OnControllerAppear;
        controller.OnFinish -= OnControllerFinish;
        controller.OnHide -= OnControllerHide;
        controller.enabled = false;

    }
    private bool NextController()
    {
        UnsetController(Current);
        current++;
        if(current >= controllers.Length)
            return false;
        else
        {
            SetController(Current);
            return true;
        }
    }
    private void OnControllerAppear()
    {

    }
    private void OnControllerFinish(bool isWin)
    {
        if (NextController())
            Current.Select();
        else
            OnFinish?.Invoke(isWin);
    }
    private void OnControllerHide()
    {
        OnPreChange?.Invoke();
    }
    private void Update()
    {
        if (current >= 0 && current < controllers.Length)
        {
            Current.Execute();
        }
    }

    private void OnPointerDown()
    {
        if (current >= 0 && current < controllers.Length)
            Current.OnPointerDown();
    }
    private void OnPointerUp()
    {
        if (current >= 0 && current < controllers.Length && Current.enabled)
            Current.OnPointerUp();

    }
    [ContextMenu("Find All Controllers")]
    public void FindAllControllers()
    {
        controllers = GetComponentsInChildren<ControllerBase>();
    }
}
