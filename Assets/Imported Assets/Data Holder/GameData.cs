using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/GameData")]
public class GameData : DataHolder
{
    #region Singleton

    private static GameData _default;
    public static GameData Default => _default;

    #endregion
   
    [Header("Input")]
    public float distanceToRecast = 0.0001f;
    public float characterGridMove = 20f;
    public float timeToSelect = 0.01f;
    public float timeWhenEndTouchToClick = 0.2f;
    [Header("GamePlay")]   
    public ulong startMoney = 100;
    [Space]
    public ulong moneyForCharacter = 100;
    public float moneyForCharacterMultiplier = 1.1f;
    [Space]
    public ulong moneyForLevel = 1000;
    public float moneyForLevelMultiplier = 1.1f;
    public int equilMoneyForLevelAfterLevel = 20;
    [Space]
    public float indicatorSpeed = 1f;
    public float powerScale = 1f;
    public Vector2 enemyTimeToActionRange = new Vector2(0.5f,1f);
    [Space]
    public float ropeMoveSpeed = 1f;
    public float ropePrepareTime = 1f;
    public float characterRopeXOffset = 0.3f;
    [Space]
    public float characterJumpDuration = 1f;
    public float characterJumpPower = 10f;
    public Vector3 fallEndPos;
	[Header("Other")]
    public bool enableTutorial = true;
    public bool enableCursor = false;
    public float cursorAnimDurationScale = 1f;
    public bool enableLazyCursor = false;
    public bool lazyCursorUseLerp = true;
    public float lazyCursorSpeed = 5f;
    [Header("UI")]
    public Color UIColor;
    public Color UITitleColor;
    public Vector3 teamPowerUIOffset;
    public Vector3 teamPowerUIScale = new Vector3(35f, 35f, 35f);

    public override void Init()
    {
        _default = this;
    }
}
