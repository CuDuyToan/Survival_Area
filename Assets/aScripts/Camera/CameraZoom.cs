using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform _cameraFollow;


    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZom = 15f;
    [SerializeField] private float maxZom = 90f;

    void Update()
    {
        // Lấy giá trị con lăn chuột
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        ChangeFOV(scrollInput);
    }

    private void ChangeFOV(float scrollInput)
    {

        if (scrollInput != 0f)
        {
            // Lấy current FOV
            float currentFOV = virtualCamera.m_Lens.FieldOfView;

            // Tính toán giá trị FOV mới
            float newFOV = Mathf.Clamp(currentFOV - scrollInput * zoomSpeed, minZom, maxZom);

            // Gán giá trị FOV mới cho virtual camera
            virtualCamera.m_Lens.FieldOfView = newFOV;
        }
    }

    private void ChangePositionCamera(float scrollInput)
    {
        if (scrollInput != 0f)
        {
            float currentPosY = _cameraFollow.position.y;

            float newPos = Mathf.Clamp(currentPosY - scrollInput * zoomSpeed, minZom, maxZom);

            _cameraFollow.position = new Vector3(0, newPos,0);
        }
    }
}
