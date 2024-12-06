using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrid : ControllerBase
{
    private GridArea area;
    private GridMover mover;
    private RaycastHit lastHit;
    private Vector2 lastPositionInput;
    private bool hasSuccsCast;
    private float lastSuccsCastTime;
    [SerializeField] private SpriteRenderer selectIndicator;
    [SerializeField] private Vector3 selectIndicatorOffset;
    [SerializeField] private LayerMask moveLayer, selectLayer;
    public override void Init(LevelMaster levelMaster)
    {
        area = levelMaster.GetArea(0);
        mover = levelMaster.Mover;
    }
    public override void ForceFinish()
    {
        PartyManager.Default.SaveParty(area);
        base.ForceFinish();
    }
    protected override bool CheckFinish()
    {
        return false;
    }

    protected override void HandleProcess()
    {

        if (hasInput && !mover.InMerge)
        {
            RaycastHit hit;
            if (mover.HasSelected)
            {
                if (Raycast(out hit, moveLayer))
                    mover.SetMovePoint(hit);
                else
                    mover.SetMovePoint(lastHit);
                MoveSelectIndicator(Color.green);
            }
            else
            {
                if (Raycast(out hit, selectLayer))
                {
                    lastPositionInput = Vector2.left * 1000;
                    if (mover.TrySelect(hit))
                    {
                        //if (hasSuccsCast &&
                        //Time.time - lastSuccsCastTime >= GameData.Default.timeToSelect)
                        //{
                        if (hasSuccsCast)
                        {
                            MoveSelectIndicator(Color.green);
                            mover.ConfirmSelect();
                        }
                        else
                        {
                            MoveSelectIndicator(Color.yellow);
                        }
                    }
                    else
                    {
                        MoveSelectIndicator(Color.yellow);

                    }
                }
                else
                {
                    StopSelectIndicator();
                }
            }
        }
        else
        {
            mover.StopMove();
            lastPositionInput = Vector2.left * 1000;
            StopSelectIndicator();
            hasInput = false;

        }
    }
    private bool Raycast(out RaycastHit hit,LayerMask layer)
    {
        Vector3 delta = InputController.Default.Position - lastPositionInput;
        delta.x /= Screen.width;
        delta.y /= Screen.height;
        if (delta.sqrMagnitude >= GameData.Default.distanceToRecast * GameData.Default.distanceToRecast)
        {
            lastPositionInput = InputController.Default.Position;
            var ray = CinemachineBrain.Default.Camera.ScreenPointToRay(lastPositionInput);
            bool hasHit = Physics.SphereCast(ray, 0.25f, out hit, 1000, layer);
            if (hasHit)
            {
                
                lastHit = hit;
                if (!hasSuccsCast)
                {
                    lastSuccsCastTime = Time.time;
                    hasSuccsCast = true;
                }
            }
            else
                hasSuccsCast = false;
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.25f);
            return hasHit;
        }
        hit = default;
        return false;
    }
    public void MoveSelectIndicator(Color color)
    {
        Vector3 position;
        if (mover.GetToPos(out position))
        {
            selectIndicator.color = color;
            selectIndicator.transform.position = position + selectIndicatorOffset;
            selectIndicator.gameObject.SetActive(true);
        }


    }
    public void StopSelectIndicator()
    {
        selectIndicator.transform.position = area.transform.position - Vector3.up;
        selectIndicator.gameObject.SetActive(false);
    }
    public override void OnPointerUp()
    {
        base.OnPointerUp();
        StopSelectIndicator();
        hasSuccsCast = false;
    }
}
