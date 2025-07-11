using System.Collections;
using UnityEngine;
using Cinemachine;
using System;
using System.Diagnostics;

public enum CameraType
{
    Topdown,
    Oblique45,
    testCamera
}

public class CameraManager : BaseManager<CameraManager>
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public CinemachineVirtualCamera currentCamera;
    public VirtualCameraConfig topdownCameraConfig;
    public VirtualCameraConfig oblique45CameraConfig;
    public VirtualCameraConfig testCameraConfig; // Add this if you want to use a test camera config
    private VirtualCameraConfig currentCameraConfig;

    [Tooltip("The Camera Transition speed")]
    [SerializeField][Range(0f, 10f)] private float moveSpeed = 1.0f;

    void Start()
    {
        currentCamera = camera1;
        currentCameraConfig = topdownCameraConfig; // Default camera config
        ApplyCameraConfig(camera1, topdownCameraConfig); // Set initial camera type
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCamera(CameraType.Topdown);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchCamera(CameraType.Oblique45);
        }

        if( Input.GetKeyDown(KeyCode.B))
        {
            SwitchCamera(CameraType.testCamera);
        }
    }

    public void SwitchCamera(CameraType cameraType)
    {
        if (currentCamera == null)
        {
            UnityEngine.Debug.LogWarning("Current camera is not set.");
            return;
        }

        if (camera1 == null || camera2 == null)
        {
            UnityEngine.Debug.LogWarning("Camera1 or Camera2 is not assigned.");
            return;
        }
        
        CinemachineVirtualCamera targetCamera = null;
        if (currentCamera == camera1)
        {
            targetCamera = camera2;
        }
        else if (currentCamera == camera2)
        {
            targetCamera = camera1;
        }

        switch (cameraType)
        {
            case CameraType.Topdown:
                if(currentCameraConfig == topdownCameraConfig)
                {
                    UnityEngine.Debug.Log("Already in Topdown camera mode.");
                    return; // Already in the desired camera mode
                }
                ApplyCameraConfig(targetCamera, topdownCameraConfig);
                break;
            case CameraType.Oblique45:
                if(currentCameraConfig == oblique45CameraConfig)
                {
                    UnityEngine.Debug.Log("Already in Oblique 45 camera mode.");
                    return; // Already in the desired camera mode
                }
                ApplyCameraConfig(targetCamera, oblique45CameraConfig);
                break;
            case CameraType.testCamera:
                if(currentCameraConfig == testCameraConfig)
                {
                    UnityEngine.Debug.Log("Already in Test camera mode.");
                    return; // Already in the desired camera mode
                }
                ApplyCameraConfig(targetCamera, testCameraConfig);
                break;
            default:
                UnityEngine.Debug.LogWarning("Unknown camera type: " + cameraType);
                return;
        }

        currentCamera.Priority = 1;
        currentCamera = targetCamera;
    }

    private void ApplyCameraConfig(CinemachineVirtualCamera camera, VirtualCameraConfig config)
    {
        if (camera != null && config != null)
        {
            camera.m_Lens.FieldOfView = config.fieldOfView;
            camera.transform.position = config.cameraDistance * Vector3.forward + config.rotation * Vector3.up;
            camera.transform.rotation = config.rotation;
            camera.Priority = config.priority;
            currentCameraConfig = config;
        }
    }

    // public void ActiveDummy()
    // {
    //     currLookAt.SetActive(true);
    //     currLookAt.transform.position = currentCamera.Follow.position;
    // }

    // public void DeactiveDummy()
    // {
    //     currLookAt.SetActive(false);
    // }

    // /// <summary>
    // /// Smoothly blend the current and the idle camera to new controllable
    // /// </summary>
    // /// <param name="newTarget">The new controllable look at point</param>
    // public void SmoothBlendTo(Transform newTarget)
    // {
    //     ActiveDummy();
    //     if (blendingCoroutine != null)
    //     {
    //         StopCoroutine(blendingCoroutine);
    //         blendingCoroutine = null;
    //         Debug.Log("ha");
    //     }
    //     blendingCoroutine = StartCoroutine(SmoothBlend(newTarget));
    //     blendingCoroutine = null;
    // }

    // /// <summary>
    // /// Corresponding coroutine for the blending
    // /// </summary>
    // /// <param name="target">The new controllable look at point</param>
    // /// <returns>IEnumerator for coroutine</returns>
    // private IEnumerator SmoothBlend(Transform target)
    // {
    //     Vector3 currLookAtPos = currLookAt.transform.position;
    //     Vector3 targetPos = target.position;
    //     SetUpFollowPoint(currLookAt.transform);

    //     while (Vector3.Distance(currLookAt.transform.position, targetPos) > 0.05f)
    //     {
    //         currLookAt.transform.position = Vector3.MoveTowards(
    //             currLookAt.transform.position,
    //             targetPos,
    //             moveSpeed * Time.deltaTime
    //         );
    //         yield return null;
    //     }

    //     currLookAt.transform.position = targetPos;
    //     EventManager.Instance.TriggerCameraBlendFinish();
    // }

    // public void SetUpFollowPoint(Transform target)
    // {
    //     topdownCamera.Follow = target;
    //     oblique45Camera.Follow = target;
    // }    
}
