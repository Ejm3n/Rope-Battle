using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRenderSpace : MonoBehaviour
{
    [SerializeField] private int renderLayer;
    [SerializeField] private Transform spawnPoint;
    private Character character;
    private bool hasCharacter;
    [SerializeField] private Camera renderCamera;
    

    public void RenderCharacter(Character prefab)
    {     
        renderCamera.enabled = true;
        if (hasCharacter)
        {
            SetLayer(character.gameObject, 0);
            PoolManager.Default.Push(character);           
        }
        character = PoolManager.Default.Pop(prefab, spawnPoint.position, spawnPoint.rotation) as Character;
        SetLayer(character.gameObject, renderLayer);
        character.SetParent(spawnPoint);
        character.transform.localPosition = character.RenderPos;
        character.transform.localRotation = character.RenderRot;
        hasCharacter = true;
    }
    public void Stop()
    {
        renderCamera.enabled = false;
        if (hasCharacter)
        {
            SetLayer(character.gameObject, 0);
            PoolManager.Default.Push(character);
            hasCharacter = false;
        }
    }
    public void SetLayer(GameObject go, int layerNumber)
    {
        go.layer = layerNumber;
        foreach (Transform child in go.transform)
        {
            child.gameObject.layer = layerNumber;
            if (child.childCount > 0)
            {
                SetLayer(child.gameObject, layerNumber);
            }
        }
    }
}
