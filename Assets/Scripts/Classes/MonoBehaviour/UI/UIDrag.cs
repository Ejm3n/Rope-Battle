using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{

    private bool isTouch;
    private Vector2 delta, startPosition, position,frameDelta;
    public bool IsTouch { get => isTouch; }
    public Vector2 Delta { get => delta; }
    public Vector2 FrameDelta 
    { 
        get 
        {
            Vector3 p = frameDelta;
            frameDelta = Vector3.zero; 
            return p;
        } 
    }
    public Vector2 Position { get => position; }

    public Action PointerDown;
    public Action PointerUp;

    private void OnDisable()
    {
        isTouch = false;
        frameDelta = startPosition = delta = position = Vector2.zero;
    }
    public void OnDrag(PointerEventData eventData)
    {
        frameDelta = eventData.position - position;
        frameDelta.x = frameDelta.x / Screen.width;
        frameDelta.y = frameDelta.y / Screen.height;
        position = eventData.position;
        delta = position - startPosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        frameDelta = startPosition = delta = position = Vector2.zero;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
        startPosition = position = eventData.position;
        PointerDown?.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        PointerUp?.Invoke();
        position = Vector2.zero;
    }
}
