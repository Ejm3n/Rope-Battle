using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeHolder : MonoBehaviour
{
    private GridArea[] areas;
    [SerializeField] private RopeObject[] ropes;

    public bool InMove
    {
        get
        {
            for (int i = 0; i < ropes.Length; i++)
            {
                if (ropes[i].InMove)
                    return true;
            }
            return false;
        }
    }
    public int Count => ropes.Length;
    public RopeObject this[int i]
    {
        get => ropes[i];
    }
    public float GetMaxPower(int side)
    {
        float p = 0f;
        for (int i = 0; i < ropes.Length; i++)
            p += ropes[i].GetPower(side);
        return p;
    }
    public void Init(GridArea[] areas)
    {
        if (ropes.Length > 0)
        {
            this.areas = areas;

            for (int j = 0; j < areas.Length; j++)
            {
                var area = areas[j];
                var size = area.GridSize;
                var step = size.x / ropes.Length;
                for (int i = 0; i < ropes.Length; i++)
                {
                    var rope = ropes[i];
                    rope.Init(area.GridSize, area.CellSize.z);
                    int x = 0;
                    int x2 = 0;
                    if (j == 0)
                    {
                        x = Mathf.Clamp(i * step, 0, size.x - 1);
                        x2 = Mathf.Clamp((i + 1) * step-1, 0, size.x-1);
                    }                  
                    else
                    {
                        x = Mathf.Clamp(size.x-1 - ((i + 1) * step - 1), 0, size.x - 1);
                        x2 = Mathf.Clamp(size.x -1- i * step, 0, size.x - 1);
                    }
                    rope.SetXCellRange(j, new Vector2Int(x, x2));


                }
            }
        }
    }
    public bool CheckEnoughCharactersOnRopes()
    {
        var size = areas[0].GridSize;
        for (int i = 0; i < ropes.Length; i++)
        {
            bool hasOne = false;
            var range = ropes[i].GetXCellRange(0);
            for (int z = 0; z < size.y; z++)
                for (int x = range.x; x <= range.y; x++)
                {
                    var cell = areas[0].GetCell(x + size.x * z);
                    if (cell.hasCharacter)
                        hasOne = true;
                }
            if (hasOne == false)
                return false;
        }
        return true;
    }
    public bool CheckFinish()
    {
        for (int i = 0; i < ropes.Length; i++)
        {
            if (!ropes[i].IsPushed)
                return false;
        }
        return true;
    }
    public bool CheckEnemyFinish()
    {
        for (int i = 0; i < ropes.Length; i++)
        {
            if (ropes[i].IsPushedRight)
                return true;
        }
        return false;
    }
    public void DistributeCharactersFromAreas()
    {

        for (int j = 0; j < areas.Length; j++)
        {
            var area = areas[j];
            var size = area.GridSize;
            var length = area.CellSize.z;
            int yCount = 0;
            int xCount = 0;
            for (int i = 0; i < ropes.Length; i++)
            {
                ropes[i].LiftUpRope();
                var range = ropes[i].GetXCellRange(j);
                yCount = 0;
                xCount = 0;
                int rangeX = range.y - range.x;
                for (int z = 0; z < size.y; z++)
                {
                    if (xCount != 0)
                        yCount++;
                    xCount = 0;
                    
                    for (int x = range.x; x <= range.y; x++)
                    {
                        var cell = area.GetCell(x + size.x * z);
                        if (cell.hasCharacter)
                        {
                            ropes[i].AddCharacters(j, 
                                cell.character, 
                                new Vector2Int(xCount, yCount),
                                area.GetCell(x + size.x * yCount).position,
                                //length * (xCount- rangeX/2) * 0.25f);
                            xCount % 2 == 0 ? -length * 0.25f : length * 0.25f);
                            xCount++;
                        }
                    }
                }
            }
        }
    }
}