using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMover : MonoBehaviour
{
   private GridArea area;

    private bool inMove, inMerge, inStop;


    private bool hasSelected,selectedHasChild,hasTo;
    private Character selected;
    private GridArea.Cell from, to;
    private Vector3 movePoint;
    private Character spawnMerged;
    private RopeHolder ropeHolder;

    public event System.Action<Character, GridArea.Cell> OnMoveComplete;
    public event System.Action<Character, GridArea.Cell> OnMergeComplete;
    public event System.Action<Character, GridArea.Cell> OnSelect;

    public bool HasSelected => hasSelected;
    public bool InWork => inMove || inMerge || inStop;
    public bool InMove => inMove;
    public bool InMerge => inMerge;
    public bool InStop => inStop;

    public bool GetToPos(out Vector3 pos)
    {
        pos = Vector3.zero;
        if (hasTo)
        {
            pos = to.position;
            return true;
        }
        return false;
    }
    public void SetArea(GridArea area,RopeHolder ropeHolder)
    {
        this.area = area;
        this.ropeHolder = ropeHolder;
    }

    public bool TrySelect(RaycastHit hit)
    {
        if (!hasSelected && area.ContainsPoint(hit.point))
        {

            to = from = area.GetClosestCell(hit.point);
            hasTo = true;
            return from.hasCharacter;

        }
        return false;
    }
    public void ConfirmSelect()
    {
        if (hasTo && from.hasCharacter)
        {
            hasSelected = true;
            selected = from.character;
            selectedHasChild = CharacterHolder.Default.TryFindCharcterByChild(selected, out spawnMerged);
            from.Clear();
            selected.Animation.PlaySelect();
            OnSelect?.Invoke(selected, from);
        }
    }


    public void SetMovePoint(RaycastHit hit)
    {
        if (hasSelected)
        {
            var cell = area.GetClosestCell(hit.point);
            if (CanMove(cell))
            {
                to = cell;
            }

        
            hasTo = true;
            enabled = inMove = true;
            if (area.ContainsPoint(hit.point))
                movePoint = hit.point;
            else
            {
                movePoint = to.position;
            }
        }
    }
    public void StopMove()
    {
        if (inMerge)
        {
            inMove = false;
            inStop = false;
            inMerge = false;
        }
        else if(hasSelected)
        {
            inStop = true;
        }
        if (hasTo)
        movePoint = to.position;
    }

    private void MoveSelected()
    {
        float speed = GameData.Default.characterGridMove * Time.deltaTime;

        if ((selected.transform.position - movePoint).sqrMagnitude > 0.01f)
        {
            selected.SetPosition(Vector3.MoveTowards(selected.transform.position, movePoint, speed));
        }
        else if (inStop)
        {
            selected.SetPosition(movePoint);
            HandleSelectedOnPosition();
        }
    }
    private void HandleSelectedOnPosition()
    {
        if (CanMergeSelected())
        {
            MergeSelected();
        }
        else
        {
            to.Set(selected);
            inStop = inMove = inMerge = false;
            hasSelected = false;
            selected.Animation.PlayDeselect();
            selected.SetModelScale(new Vector3(to.x % 2 == 0 ? -1f:1f, 1f, 1f));
            OnMoveComplete?.Invoke(selected,to);
        }
    }

    private bool CanMergeSelected()
    {
        return
            selectedHasChild &&
            to.hasCharacter &&
            selected != to.character &&
            selected.Specifier.Equals(to.character.Specifier) &&
            
            (area.CharactersCount > ropeHolder.Count || MoneyService.Default.GetMoney() >= MoneyForCharacter() && area.CharactersCount == ropeHolder.Count);
    }
    private ulong MoneyForCharacter()
    {
        return
            (ulong)(
            GameData.Default.moneyForCharacter *
            Mathf.Pow(GameData.Default.moneyForCharacterMultiplier, PartyManager.Default.GetBuyCount()));
    }
    private bool CanMove(GridArea.Cell cell)
    {
        return
            (cell.hasCharacter && selectedHasChild &&
             cell.character.Specifier.Equals(selected.Specifier) &&
             (area.CharactersCount > ropeHolder.Count || (MoneyService.Default.GetMoney() >= MoneyForCharacter() && area.CharactersCount == ropeHolder.Count)))
                ||
              !cell.hasCharacter;
    }
    private void MergeSelected()
    {
        var char1 = selected;
        var char2 = to.character;
        selected.Animation.PlayDeselect();
        area.RemoveCharacter(char1, to);
        area.RemoveCharacter(char2, to);
        PoolManager.Default.Push(char1);
        PoolManager.Default.Push(char2);
        Character character = PoolManager.Default.Pop(spawnMerged, to.position, to.rotation) as Character;
        area.AddCharacter(character, to);
        character.Animation.PlayAppear();
        inMerge = true;
        hasSelected = false;       
        OnMergeComplete?.Invoke(character, to);
    }


    private void LateUpdate()
    {
        if (hasSelected)
            MoveSelected();
        else
            enabled = false;
    }
}
