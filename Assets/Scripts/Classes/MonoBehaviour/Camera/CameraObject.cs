using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObject : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineVirtualCamera cam;
    public void Select()
    {
        cam.Priority = CinemachineBrain.Default.GetPriority();
    }
}
