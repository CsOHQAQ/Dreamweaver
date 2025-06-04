using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VirtualCameraConfig", menuName = "Camera/VirtualCameraConfig", order = 0)]
public class VirtualCameraConfig : ScriptableObject
{
    public float fieldOfView;
    public float cameraDistance;
    public Quaternion rotation;
    public int priority;
    
}

