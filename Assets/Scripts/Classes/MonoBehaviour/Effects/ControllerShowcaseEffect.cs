using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerShowcaseEffect : MonoBehaviour
{
    [SerializeField] private ControllerShowcase controller;
    [SerializeField] private ParticleSystem effect;
    private void Awake()
    {
        controller.OnAllyBoss += OnAllyBoss;
    }
    private void OnDestroy()
    {
        controller.OnAllyBoss -= OnAllyBoss;
    }
    private void OnAllyBoss(Character character)
    {
        effect.transform.position = character.transform.position;
        effect.Play();
        //PoolManager.Default.Pop(effectPrefab, character.transform.position, Quaternion.identity);
    }
}