using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class RopeObject : MonoBehaviour
{
    [Serializable]
    protected class CharacterTarget
    {
        [SerializeField] private Character character;
        private Transform transform;
        private float distToPush;
        private Vector3 startPos;
        private Vector3 targetMove;
        private bool inMove;

        public CharacterTarget(Character character,float distToPush)
        {
            this.character = character;
            transform = character.transform;
            startPos = transform.position;
            this.distToPush = distToPush;
        }
        public Vector3 position => transform.position;
        public CharacterAnimation Animation => character.Animation;
        public float Power => character.Power;
        public bool Move(float shift, Vector3 direction)
        {
            if (!inMove)
            {
                targetMove = shift * direction + transform.position;
                inMove = true;
                //character.Animation.PlayPull();
                return false;
            }
            else
            {
                if((transform.position - targetMove).sqrMagnitude <= 0.01f)
                {
                    transform.position = targetMove;
                    inMove = false;
                    //character.Animation.PlayPullIdle();
                    return true;
                }else
                {
                    transform.position = Vector3.MoveTowards(transform.position,targetMove,GameData.Default.ropeMoveSpeed*Time.deltaTime);
                    return false;
                }
            }
        }
        public bool CheckPush()
        {
            return (startPos - transform.position).sqrMagnitude >= distToPush * distToPush;
        }

        public void MoveStartPos(Vector3 pos, bool withMove = true,float duration = 0.5f)
        {
            startPos = pos;
            if (withMove)
                transform.DOMove(pos, duration);
            else
                transform.position = pos;
        }

        public void Push(Vector3 point, Vector3 direction,float power,float duration)
        {
            character.Animation.PlayFall();
            var pos = direction * Vector3.Dot(point - transform.position, direction) + transform.position;
            pos.y += point.y;
            transform.DOJump(pos, power, 1, duration);
        }
    }
   
    
    [SerializeField] private RopeModel model;
    [SerializeField] private float ropeSetDuration = 0.5f;
    private float[] shifts = new float[2];
    private bool inMove;
    private int moveSide;

    private Vector2Int[] xCellRanges = new Vector2Int[2];
    private float maxMove,cellLength,shiftMove;
    private Vector2Int cellCount;
    private Vector3 ropeTargetMove;

    private bool isPushed;
    private List<CharacterTarget> leftCharacters = new List<CharacterTarget>();
    private List<CharacterTarget> rightCharacters = new List<CharacterTarget>();

    private float nextPause,nextJerk;


    public Action OnMoveEnd;
    public Action OnPush;


    public bool IsPushed => isPushed;
    public bool IsPushedRight => isPushed &&  rightCharacters.Count > leftCharacters.Count;
    public bool InMove => inMove;
    public float GetShift(int side) => shifts[side];
    public void SetXCellRange(int id, Vector2Int range)
    {
        xCellRanges[id] = range;
    }
    public Vector2Int GetXCellRange(int id) => xCellRanges[id];

    public float GetPower(int id)
    {
        float p = 0f;
        if (id == 0)
        {
            for (int i = 0; i < leftCharacters.Count; i++)
                p += leftCharacters[i].Power;
            return p;
        }
        else
        {
            for (int i = 0; i < rightCharacters.Count; i++)
                p += rightCharacters[i].Power;
            return p;
        }
    }
    public void Init(Vector2Int cellCount,float cellLength)
    {
        maxMove = cellCount.y*cellLength + cellLength*0.5f;
        this.cellCount = cellCount;
        this.cellLength = cellLength;
    }

    public void PlayPull(int side)
    {
        if(side == 0)
        {
            for (int i = 0; i < leftCharacters.Count; i++)
                leftCharacters[i].Animation.PlayPull();
            for (int i = 0; i < rightCharacters.Count; i++)
                rightCharacters[i].Animation.PlayBack();
        }
        else
        {
            for (int i = 0; i < rightCharacters.Count; i++)
                rightCharacters[i].Animation.PlayPull();
            for (int i = 0; i < leftCharacters.Count; i++)
                leftCharacters[i].Animation.PlayBack();


        }
    }

    public void AddCharacters(int side,Character character, Vector2Int cellId,Vector3 pos,float offset = 0f)
    {
        Transform t = transform;
        float sign = (side == 0 ? -1f : 1f);
        float distToPush = (cellCount.y - cellId.y) * cellLength - cellLength * 0.5f  + offset * sign;
        CharacterTarget target = new CharacterTarget(character, distToPush);
        var startPos = 
            t.right * 
            Vector3.Dot(t.position - pos, t.right) * 
            GameData.Default.characterRopeXOffset + 
            pos + 
            Vector3.forward* offset;
        target.MoveStartPos(startPos, true, ropeSetDuration);
        target.Animation.PlayPullIdle();
        if (side == 0)
            leftCharacters.Add(target);
        else
            rightCharacters.Add(target);

    }
    public void LiftUpRope()
    {
        model.LiftUp(ropeSetDuration);
        //model.SetMinStretch();
    }
    public void PrepareMove()
    {
        model.SetMinStretch();
    }
    public void StartMove(int side,float power)
    {
        if (!inMove)
        {
            inMove = true;
            moveSide = side;
            shiftMove = power * maxMove * (moveSide == 0 ? -1:1);
            //shifts[moveSide] = Mathf.Clamp(shifts[moveSide] + delta, -maxMove, maxMove);
            if (moveSide == 0)
            {
                model.SetTargetMove(1, shiftMove, transform.forward);
                HandleCharactersMove(rightCharacters, shiftMove, transform.forward);
                //for (int i = 0; i < leftCharacters.Count; i++)
                //    leftCharacters[i].Animation.PlayPull();
                //for (int i = 0; i < rightCharacters.Count; i++)
                //    rightCharacters[i].Animation.PlayBack();
            }
            else
            {
                model.SetTargetMove(0, shiftMove, transform.forward);
                HandleCharactersMove(leftCharacters, shiftMove, transform.forward);
                //for (int i = 0; i < rightCharacters.Count; i++)
                //    rightCharacters[i].Animation.PlayPull();
                //for (int i = 0; i < leftCharacters.Count; i++)
                //    leftCharacters[i].Animation.PlayBack();

            }      
            enabled = true;
        }
    }
    private bool HandleCharactersMove(List<CharacterTarget> targets, float shift,Vector3 direction) 
    {
        bool complete = true;

        if (inMove)
        {
            if (targets.Count <= 0)
            {
                return true;
            }
            else
            {
                for (int i = targets.Count - 1; i >= 0; i--)
                {
                    var target = targets[i];
                    if (!target.Move(shift, direction))
                        complete = false;
                    if (target.CheckPush())
                    {
                        target.Push(
                            GameData.Default.fallEndPos,
                            direction,
                            GameData.Default.characterJumpPower,
                            GameData.Default.characterJumpDuration);
                        OnPush?.Invoke();
                        targets.Remove(target);
                        if (targets.Count <= 0)
                        {
                            isPushed = true;
                           
                            //ResetModelPosition();
                        }
                    }


                }
            }
        }
        return complete;
    }

    private void LateUpdate()
    {
        if (inMove)
        {
            bool moveEnd = false;
            if (moveSide == 0)
            {
                model.Move(1,GameData.Default.ropeMoveSpeed * Time.deltaTime);
                moveEnd = HandleCharactersMove(rightCharacters, shiftMove, transform.forward);

            }
            else
            {
                model.Move(0, GameData.Default.ropeMoveSpeed*Time.deltaTime);
                moveEnd = HandleCharactersMove(leftCharacters, shiftMove, transform.forward);
            }
            if (moveEnd)
            {
                if (isPushed)
                {
                    if (moveSide == 0)
                    {
                        for (int i = 0; i < leftCharacters.Count; i++)
                            leftCharacters[i].Animation.PlayDance();
                        model.DisableAttach(1);
                    }
                    else
                    {
                        for (int i = 0; i < rightCharacters.Count; i++)
                            rightCharacters[i].Animation.PlayDance();
                        model.DisableAttach(0);
                    }
                    model.LiftDown(ropeSetDuration);
                }
                else
                {
                    for (int i = 0; i < leftCharacters.Count; i++)
                        leftCharacters[i].Animation.PlayPullIdle();
                    for (int i = 0; i < rightCharacters.Count; i++)
                        rightCharacters[i].Animation.PlayPullIdle();
                }
                model.SetMaxStretch();

                inMove = false;
                OnMoveEnd?.Invoke();
            }
        }
        else
            enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < leftCharacters.Count; i++)
            {
                var pos = leftCharacters[i].position;
                Gizmos.DrawWireSphere(pos, 0.1f);
                var pos2 = transform.right * Vector3.Dot(transform.position - pos, transform.right);
                Gizmos.DrawLine(pos, pos2 + pos);
            }
            Gizmos.color = Color.red;
            for (int i = 0; i < rightCharacters.Count; i++)
            {
                var pos = rightCharacters[i].position;
                Gizmos.DrawWireSphere(pos, 0.1f);
                var pos2 = transform.right * Vector3.Dot(transform.position - pos, transform.right);
                Gizmos.DrawLine(pos, pos2 + pos);
            }
        }
    }
}