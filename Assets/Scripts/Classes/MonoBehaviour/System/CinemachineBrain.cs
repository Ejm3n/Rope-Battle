using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineBrain : MonoBehaviour
{
    #region Singleton

    private static CinemachineBrain _default;
    public static CinemachineBrain Default => _default;
    #endregion
    private float defaultBlendTime;
    [SerializeField] private Camera cam;
    [SerializeField] private Cinemachine.CinemachineBrain brain;
    private int priority = 1;
    private void Awake()
    {
        _default = this;
        defaultBlendTime = brain.m_DefaultBlend.m_Time;
    }
    public Cinemachine.CinemachineBrain Brain => brain;
    public Camera Camera => cam;

    public void SetBlendTime(float time)
    {
        brain.m_DefaultBlend.m_Time = time;
    }
    public void DefaultBlendTime()
    {
        brain.m_DefaultBlend.m_Time = defaultBlendTime;
    }
    public int GetPriority()
    {
        priority++;
        return priority;
    }

    public void Teleport(Transform transform)
    {
        this.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
