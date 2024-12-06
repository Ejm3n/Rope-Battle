using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Test : MonoBehaviour
{
    public Transform target0, target1;
    public float dist = 10;

    public void Start()
    {
        target0.DOMoveZ(dist,0.5f);
        target1.DOMoveZ(-dist, 0.5f);
    }
}
