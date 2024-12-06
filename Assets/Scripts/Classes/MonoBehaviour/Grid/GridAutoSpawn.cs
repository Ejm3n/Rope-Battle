using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAutoSpawn : MonoBehaviour
{
    [SerializeField] private GridArea area;
    [SerializeField] private int countOffset;
    public void Spawn()
    {
        int length = area.CellCount;
        int level = SumNumber(LevelManager.Default.CurrentLevelCount - LevelManager.Default.Levels.Count);
        int count = countOffset + level;

        int tier = count / length;
        int off = count - tier * length;
        CharacterHolder.CharacterInheritance[] toSpawn = new CharacterHolder.CharacterInheritance[length];
        Debug.Log($"length: {length}, count: {count}, tier: {tier}, off: {off}");
        if (tier != 0)
        {
            var character = CharacterHolder.Default.GetInchByTier(tier - 1);
            for (int i = 0; i < length; i++)
                toSpawn[i] = character;
            character = CharacterHolder.Default.GetInchByTier(tier);
            for (int i = 0; i < off; i++)
                toSpawn[i] = character;

        }
        else
        {
            var character = CharacterHolder.Default.Get(0);
            for (int i = 0; i < off; i++)
                toSpawn[i] = character;
        }

        for (int i = 0; i < length; i++)
        {
            if (toSpawn[i] == null)
                break;
            area.ForceSpawnCharacterInCell(toSpawn[i].enemy, area.GetCell(i));
        }
    }
    private int SumNumber(int number)
    {
        int r = 0;
        for (int i = 1; i <= number; i++)
        {
            r = r+i;
        }
        return r;
    }
}
