using UnityEngine;

[RequireComponent(typeof(GridMover))]
public class GridEffectMerge : MonoBehaviour
{
    private GridMover mover;
    [SerializeField] private EffectDynamic effectPrefab;
    private void Awake()
    {
        mover = GetComponent<GridMover>();
        mover.OnMergeComplete += OnCharacterMerge;
    }
    private void OnDestroy()
    {
        mover.OnMergeComplete -= OnCharacterMerge;
    }
    private void OnCharacterMerge(Character character, GridArea.Cell cell)
    {
        PoolManager.Default.Pop(effectPrefab, character.transform.position, Quaternion.identity);
    }
}
