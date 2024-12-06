using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridArea : MonoBehaviour
{
    [System.Serializable]
    public class Cell
    {
        public int x, y;
        public Vector3 position;
        public Quaternion rotation;
        public Character character;
        public bool hasCharacter;

        public Cell(int x, int y, Vector3 position, Quaternion rotation)
        {
            this.x = x;
            this.y = y;
            this.position = position;
            this.rotation = rotation;
        }
        public void Clear()
        {
            hasCharacter = false;
            character = null;
        }
        public void Set(Character character)
        {
            hasCharacter = true;
            this.character = character;
        }
    }

    public Color gizmosColor = Color.red;
    [Header("Grid")]
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private Vector3 cellSize;
    private List<Character> characters = new List<Character>(16);
    private Cell[] cells = new Cell[0];
    [SerializeField] private GridAutoSpawn autoSpawer;
    public System.Action<Character> OnAdd;
    public System.Action<Character> OnRemove;
    public System.Action<Character> OnSpawn;
    public System.Action OnInit;
    public int CharactersCount => characters.Count;
    public int CellCount => cells.Length;
    public Vector2Int GridSize => gridSize;
    public Vector3 CellSize => cellSize;
    public bool ContainsPoint(Vector3 point)
    {
        Vector3 leftDown = cells[0].position;
        leftDown.x -= cellSize.x;
        leftDown.z -= cellSize.z;
        Vector3 rightUp = cells[cells.Length - 1].position;
        rightUp.x += cellSize.x;
        rightUp.z += cellSize.z;
        return point.x > leftDown.x && point.x < rightUp.x && point.z > leftDown.z && point.z < rightUp.z;
    }
    public bool HasEmptyCells()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (!cells[i].hasCharacter)
                return true;
        }
        return false;
    }
    public Character GetCharacter(int id)
    {
        return characters[id];
    }
    public Cell GetCell(int id)
    {
        return cells[id];
    }
    public bool GetBoss(out Character boss)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            boss = characters[i];
            if (boss.IsBoss)
                return true;
        }
        boss = null;
        return false;
    }
    public bool GetBoss(string specifier, out Character boss)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            boss = characters[i];
            if (boss.IsBoss && boss.Specifier.Equals(specifier))
                return true;
        }
        boss = null;
        return false;
    }
    public void Init()
    {
        CreateGrid();

        if (transform.childCount > 0)
        {
            var spawners = GetComponentsInChildren<CharacterSpawner>();
            for (int i = 0; i < spawners.Length; i++)
            {
                Cell cell = GetClosestCell(spawners[i].transform.position);
                if (!cell.hasCharacter)
                    AddCharacter(spawners[i].Spawn(), cell);
            }
        }
        if (autoSpawer != null)
        {
            autoSpawer.Spawn();
        }
        OnInit?.Invoke();
        float totalPower = 0;
        foreach (var character in characters)
        {
            totalPower += character.Power;
        }
        if (totalPower > SaveManager.LoadTeamPower())
            SaveManager.SaveTeamPower(Mathf.FloorToInt(totalPower));
    }
    private void CreateGrid()
    {
        cells = new Cell[gridSize.x * gridSize.y];
        float xStep = cellSize.x;
        float zStep = cellSize.z;
        float xStart = -cellSize.x * gridSize.x * 0.5f + xStep * 0.5f;
        float zStart = -cellSize.z * gridSize.y * 0.5f + zStep * 0.5f;

        for (int z = 0; z < gridSize.y; z++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                cells[x + gridSize.x * z] = new Cell(x, z, transform.position + transform.rotation * new Vector3(xStart + xStep * x, 0.0f, zStart + zStep * z), transform.rotation);
            }
        }
    }
    //private void AlignTransformChildren()
    //{
    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        Transform child = transform.GetChild(i);
    //        child.position = GetClosestCell(child.position).position;
    //    }
    //}
    public bool SpawnCharacter(Character prefab)
    {
        Cell cell;
        if (GetEmptyCell(out cell))
        {
            Character character = PoolManager.Default.Pop(prefab, cell.position, cell.rotation) as Character;
            AddCharacter(character, cell);
            character.Animation.PlayAppear();
            OnSpawn?.Invoke(character);
            return true;
        }
        return false;
    }

    public bool GetWeakestCell(out Cell cell)
    {
        if (cells.Length > 0)
        {
            cell = cells[0];
            for (int i = 1; i < cells.Length; i++)
            {
                if ((cell.hasCharacter && cells[i].hasCharacter &&
                   cell.character.Power > cells[i].character.Power) ||
                   (!cell.hasCharacter && cells[i].hasCharacter))
                {
                    if (CharacterHolder.Default.CheckCharacterHasChild(cells[i].character) ||
                        cells[i].character.Specifier.Equals(CharacterHolder.Default[0].Specifier))
                        cell = cells[i];
                }
            }
            return cell.hasCharacter;

        }

        cell = null;
        return false;
    }
    public void ForceSpawnCharacterInCell(Character prefab, Cell cell)
    {
        Character character = PoolManager.Default.Pop(prefab, cell.position, cell.rotation) as Character;
        AddCharacter(character, cell);
    }
    public void ForceAddCharacterWithReplace(Character prefab)
    {
        Cell cell;
        if (GetEmptyCell(out cell))
        {
            Character character = PoolManager.Default.Pop(prefab, cell.position, cell.rotation) as Character;
            AddCharacter(character, cell);
        }
        else if (GetWeakestCell(out cell))
        {
            Character c = cell.character;
            RemoveCharacter(c, cell);
            PoolManager.Default.Push(c);
            ForceSpawnCharacterInCell(prefab, cell);
        }
    }
    public void AddCharacter(Character character, Cell cell)
    {
        if (!characters.Contains(character))
        {
            character.transform.position = cell.position;
            character.transform.rotation = transform.rotation;
            if (cell.x % 2 == 0)
                character.SetModelScale(new Vector3(-1f, 1f, 1f));
            cell.Set(character);
            characters.Add(character);
            OnAdd?.Invoke(character);

        }
    }
    public void RemoveCharacter(Character character, Cell cell)
    {
        if (characters.Remove(character))
        {
            cell.Clear();
            OnRemove?.Invoke(character);
        }
    }

    public bool GetEmptyCell(out Cell cell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (!cells[i].hasCharacter)
            {
                cell = cells[i];
                return true;
            }
        }
        cell = default;
        return false;
    }
    public Cell GetClosestCell(Vector3 point)
    {
        int id = 0;
        Vector3 closest = cells[0].position;
        for (int i = 1; i < cells.Length; i++)
        {
            if ((point - closest).sqrMagnitude >= (point - cells[i].position).sqrMagnitude)
            {
                closest = cells[i].position;
                id = i;
            }
        }
        return cells[id];
    }
    private void OnDrawGizmos()
    {
        var matrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = gizmosColor;

        float xStep = cellSize.x;
        float zStep = cellSize.z;
        float xStart = -cellSize.x * gridSize.x * 0.5f + xStep * 0.5f;
        float zStart = cellSize.z * gridSize.y * 0.5f - zStep * 0.5f;

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int z = 0; z < gridSize.y; z++)
            {
                Vector3 pos = new Vector3(xStart + xStep * x, 0.0f, zStart - zStep * z);
                Gizmos.DrawWireCube(pos, cellSize);
                Gizmos.DrawSphere(pos, 0.05f);
            }
        }
        Gizmos.matrix = matrix;
        Gizmos.color = Color.yellow;
        if (Application.isPlaying)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].hasCharacter)
                    Gizmos.DrawWireCube(cells[i].position, Vector3.one * 0.25f);
            }
        }
    }
}

