using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private Character prefab;

    public Character Spawn()
    {
        Character character = PoolManager.Default.Pop(prefab, transform.position, transform.rotation) as Character;
        character.SetPosition(transform.position);
        character.SetRotation(transform.rotation);
        return character;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            return;
        if (prefab != null)
            Gizmos.color = prefab.gizmosColor;
        else
            Gizmos.color = Color.black;
        var matrix = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.up, new Vector3(1.0f, 2.0f, 1.0f));
        Gizmos.DrawCube(Vector3.up, new Vector3(0.25f, 2.0f, 0.25f));
        Gizmos.DrawCube(Vector3.up + Vector3.forward * 0.5f, new Vector3(0.25f, 0.25f, 0.25f));
        Gizmos.matrix = matrix;

    }
}
