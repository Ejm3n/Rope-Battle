using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GridMover))]
public class GridSoundMerge : MonoBehaviour
{
    private GridMover mover;
    [SerializeField] private string soundMerge = "Merge";
    private void Awake()
    {
        mover = GetComponent<GridMover>();
        mover.OnMergeComplete += OnMergeComplete;
    }
    private void OnDestroy()
    {
        mover.OnMergeComplete -= OnMergeComplete;
    }
    private void OnMergeComplete(Character character, GridArea.Cell cell)
    {
        SoundHolder.Default.PlayFromSoundPack(soundMerge);
    }
}
