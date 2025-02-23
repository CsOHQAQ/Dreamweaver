using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Mathematics;

public class CameraZoom : MonoBehaviour
{
    [SerializeField][Range(0f, 10f)] private float defaultDistance = 6f;
    [SerializeField][Range(0f, 10f)] private float minimumDistance = 1f;
    [SerializeField][Range(0f, 10f)] private float maximumDistance = 6f;
    [SerializeField][Range(0f, 100f)] private float smoothing = 4f;
    [SerializeField][Range(0f, 10f)] private float zoomSensitivity = 1f;

    private CinemachineFramingTransposer framingTransposer;
    private CinemachineInputProvider provider;
    private float currentTargetDistance;

    private void Awake()
    {
        framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        provider = GetComponent<CinemachineInputProvider>();
        currentTargetDistance = defaultDistance;
    }

    private void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        float zoomValue = provider.GetAxisValue(2) * zoomSensitivity;
        currentTargetDistance = math.clamp(currentTargetDistance + zoomValue, minimumDistance, maximumDistance);
        float currentDistance = framingTransposer.m_CameraDistance;
        if(currentDistance == currentTargetDistance) return;

        float lerpZoomValue = math.lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
        framingTransposer.m_CameraDistance = lerpZoomValue;
    }

}
