using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CameraManager : BaseManager<CameraManager>
{
    public static CameraManager instance;
    public CinemachineVirtualCamera topdownCamera;
    public CinemachineVirtualCamera oblique45Camera;
    public CinemachineVirtualCamera currentCamera;

    [Tooltip("The Camera Transition speed")]
    [SerializeField][Range(0f, 10f)] private float blendTime = 1.0f;
    [SerializeField] private GameObject currLookAt;

    private Coroutine blendingCoroutine = null;


    void Start()
    {
        currLookAt.SetActive(false);
        currentCamera = topdownCamera;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TopdownCamera();
            Debug.Log("Topdown Camera Activated");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Oblique45Camera();
            Debug.Log("Oblique 45 Camera Activated");
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (currentCamera == topdownCamera)
            {
                oblique45Camera.transform.rotation = Quaternion.Euler(20, 0, 0);
                Oblique45Camera();

            }
        }
    }

    public void TopdownCamera()
    {
        if (topdownCamera != null && oblique45Camera != null)
        {
            topdownCamera.Priority = 11;
            oblique45Camera.Priority = 1;
            currentCamera = topdownCamera;
        }
    }

    public void Oblique45Camera()
    {
        if (topdownCamera != null && oblique45Camera != null)
        {
            topdownCamera.Priority = 1;
            oblique45Camera.Priority = 11;
            currentCamera = oblique45Camera;
        }
    }

    public void ActiveDummy()
    {
        currLookAt.SetActive(true);
        currLookAt.transform.position = currentCamera.Follow.position;
    }

    public void DeactiveDummy()
    {
        currLookAt.SetActive(false);
    }

    /// <summary>
    /// Smoothly blend the current and the idle camera to new controllable
    /// </summary>
    /// <param name="newTarget">The new controllable look at point</param>
    public void SmoothBlendTo(Transform newTarget)
    {
        ActiveDummy();
        if (blendingCoroutine != null)
        {
            StopCoroutine(blendingCoroutine);
            blendingCoroutine = null;
        }
        blendingCoroutine = StartCoroutine(SmoothBlend(newTarget));
        blendingCoroutine = null;
    }

    /// <summary>
    /// Corresponding coroutine for the blending
    /// </summary>
    /// <param name="target">The new controllable look at point</param>
    /// <returns>IEnumerator for coroutine</returns>
    private IEnumerator SmoothBlend(Transform target)
    {
        Vector3 currLookAtPos = currLookAt.transform.position;
        Vector3 targetPos = target.position;
        float elapsed = 0.0f;
        SetUpFollowPoint(currLookAt.transform);

        while (elapsed < blendTime)
        {
            currLookAt.transform.position = Vector3.Lerp(currLookAtPos, targetPos, elapsed / blendTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        currLookAt.transform.position = targetPos;
        EventManager.Instance.TriggerCameraBlendFinish();
    }

    public void SetUpFollowPoint(Transform target)
    {
        topdownCamera.Follow = target;
        oblique45Camera.Follow = target;
    }    
}
