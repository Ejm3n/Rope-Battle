using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : PoolObject
{
    public Color gizmosColor = Color.green;
    [SerializeField] private bool isBoss;
    [SerializeField] private string _name;
    [SerializeField] private float power = 10f;
    [SerializeField] Vector3 renderPos = Vector3.zero;
    [SerializeField] Quaternion renderRot = Quaternion.identity;

    //[SerializeField] private int tier;
    [SerializeField] private Transform model;
    [SerializeField] private CharacterAnimation anim;
    
    public bool IsBoss => isBoss;
    public float Power => power;
    public CharacterAnimation Animation => anim;
   // public int Tier => tier;
    public string Name => _name;

    public Vector3 RenderPos => renderPos;
    public Quaternion RenderRot => renderRot;

    public void SetModelScale(Vector3 vector3)
    {
        model.localScale = vector3;
    }
    public override void Pop()
    {
        base.Pop();
        SetLayer(gameObject, 0);
    }
    public override void Push()
    {
        base.Push();
        SetLayer(gameObject,0);
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
