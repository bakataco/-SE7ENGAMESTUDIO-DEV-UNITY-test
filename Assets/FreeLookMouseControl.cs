using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FreeLookMouseControl : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public Transform player;

    public float sensitivityX = 1f;
    public float sensitivityY = 1f;

    public float returnSpeed = 5f;

    private float originalXAxis;
    private bool isOrbiting = false;

    void Start()
    {
        // Disable Cinemachine's automatic mouse input
        freeLookCamera.m_XAxis.m_InputAxisName = "";
        freeLookCamera.m_YAxis.m_InputAxisName = "";

        // Save the default horizontal camera angle
        originalXAxis = freeLookCamera.m_XAxis.Value;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Start orbiting
        if (Input.GetMouseButtonDown(0))
        {
            isOrbiting = true;
        }

        if (Input.GetMouseButton(0))
        {
            // LMB held: Independently orbit the camera
            freeLookCamera.m_XAxis.Value += mouseX * sensitivityX;

            freeLookCamera.m_YAxis.Value -= mouseY * sensitivityY;
        }
        else
        {
            // LMB released: Rotate the player horizontally
            player.Rotate(0f, mouseX * sensitivityX, 0f);

            // Camera vertical movement remains active
            freeLookCamera.m_YAxis.Value -= mouseY * sensitivityY;
        }

        // Clamp vertical camera movement
        freeLookCamera.m_YAxis.Value = Mathf.Clamp(
            freeLookCamera.m_YAxis.Value,
            0.05f,
            0.95f
        );

        // Stop orbiting when released
        if (Input.GetMouseButtonUp(0))
        {
            isOrbiting = false;
        }

        // Return only the horizontal camera axis
        if (!isOrbiting)
        {
            freeLookCamera.m_XAxis.Value = Mathf.LerpAngle(
                freeLookCamera.m_XAxis.Value,
                originalXAxis,
                returnSpeed * Time.deltaTime
            );
        }
    }

    public void DisableCameraControl()
    {
        enabled = false;
    }
}


