using BG.UI.Camera;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ControllerShowcase : ControllerBase
{
    [SerializeField] private Vector3 cameraPosOffset;
    [SerializeField] private Vector3 bossOffset = Vector3.up;
    [SerializeField] private float showTime = 1f;
    [SerializeField] private float showAllyTime = 2f;
    private Vector3 startPos;


    private float nextTime;
    private CameraObject cameraObject;
    private Character boss;
    private bool hasBoss;
    private bool isAlly;
    private bool onPos;

    public System.Action<Character> OnEnemyBoss;
    public System.Action<Character> OnAllyBoss;


    public override void Init(LevelMaster levelMaster)
    {
        if (!LevelManager.Default.IsRestart)
        {
            string specifer;
            cameraObject = CameraSystem.Default[CameraState.Start];
            if (PartyManager.Default.GetLastBossSpecifer(out specifer) && levelMaster.GetArea(0).GetBoss(specifer, out boss))

            {
                hasBoss = true;
                isAlly = true;
                startPos = boss.transform.position;
            }
            else if (levelMaster.GetArea(1).GetBoss(out boss))
            {
                hasBoss = true;
            }
        }
    }
    protected override bool HandleApear()
    {
        if (base.HandleApear())
        {
            if (hasBoss)
                ShowBoss(boss);
            return true;
        }
        if (!hasBoss)
            ForceFinish();
        return false;
    }
    private void ShowBoss(Character character)
    {
        var chrT = character.transform;
        var camT = cameraObject.transform;
        camT.position = chrT.position + cameraPosOffset;
        if (isAlly)
        {
            camT.position = camT.position + bossOffset;
            DOTween.To(
    () => 0f,
    (v) =>
    {
        chrT.rotation = Quaternion.AngleAxis(Mathf.LerpAngle(0f, 180f, v), Vector3.up);
        chrT.position = Vector3.Lerp(startPos, startPos + bossOffset, v);
    },
    1f, 0.35f);
        }

        cameraObject.Select();
    }
    protected override bool CheckFinish()
    {
        return false;
    }

    protected override void HandleProcess()
    {
        if (hasBoss)
        {
            Vector3 dist =
                cameraObject.transform.position -
                CinemachineBrain.Default.transform.position;
            if (dist.sqrMagnitude <= 0.1f)
            {
                if (onPos)
                {
                    if(Time.time >= nextTime)
                    {
                        if (isAlly)
                        {
                            var chrT = boss.transform;
                            DOTween.To(
                    () => 0f,
                    (v) =>
                    {
                        chrT.rotation = Quaternion.AngleAxis(Mathf.LerpAngle(180f, 0f, v), Vector3.up);
                        chrT.position = Vector3.Lerp(startPos + bossOffset, startPos, v);
                    },
                    1f, 0.35f);
                        }
                        ForceFinish();

                    }

                }
                else
                {
                    onPos = true;
                    nextTime = Time.time + (isAlly? showAllyTime: showTime);
                    if (isAlly)
                        OnAllyBoss?.Invoke(boss);
                    else
                        OnEnemyBoss?.Invoke(boss);
                }
            }
        }
        else
            ForceFinish();
    }
}
