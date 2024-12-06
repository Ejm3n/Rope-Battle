using UnityEngine;

[RequireComponent(typeof(GridArea))]
public class GridEffectSpawn : MonoBehaviour
{
    private GridArea area;
    [SerializeField] private EffectDynamic effectPrefab;
    private void Awake()
    {
        area = GetComponent<GridArea>();
        area.OnSpawn += OnCharacterSpawn;
    }
    private void OnDestroy()
    {
        area.OnSpawn -= OnCharacterSpawn;
    }
    private void OnCharacterSpawn(Character character)
    {
        PoolManager.Default.Pop(effectPrefab, character.transform.position, Quaternion.identity);
    }
}
