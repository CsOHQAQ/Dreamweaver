using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public CinemachineVirtualCamera topdownCamera;
    public CinemachineVirtualCamera oblique45Camera;
    public CinemachineVirtualCamera currentCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
}
