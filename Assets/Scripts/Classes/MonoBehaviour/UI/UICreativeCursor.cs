using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICreativeCursor : MonoBehaviour//, IPointerClickHandler,IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UITutorialCursor cursor;
    private float startClick;
    private void Start()
    {
        gameObject.SetActive(GameData.Default.enableCursor);
        cursor.transform.position = Input.mousePosition;

    }

    private void LateUpdate()
    {
        if (GameData.Default.enableLazyCursor)
        {
            if (GameData.Default.lazyCursorUseLerp)
                cursor.transform.position = Vector3.Lerp(cursor.transform.position, Input.mousePosition, GameData.Default.lazyCursorSpeed * Time.deltaTime);
            else
            cursor.transform.position = Vector3.MoveTowards(cursor.transform.position,Input.mousePosition,GameData.Default.lazyCursorSpeed*200f*Time.deltaTime);

        }
        else
        {
            cursor.transform.position = Input.mousePosition;
        }
        if (Input.GetMouseButtonDown(0))
        {
            cursor.PlayDown(0.1f);

            startClick = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - startClick < 0.2f)
                cursor.PlayClick(0.5f*GameData.Default.cursorAnimDurationScale);
            else
                cursor.PlayUp(0.1f);
        }
    }
}
