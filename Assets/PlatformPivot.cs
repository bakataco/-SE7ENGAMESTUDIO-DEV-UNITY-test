using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHinge : MonoBehaviour
{
    public float rotationAngle = 90f;
    public float rotationSpeed = 90f;

    private bool isDown = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;

        openRotation = closedRotation *
            Quaternion.Euler(-rotationAngle, 0f, 0f);
    }

    void Update()
    {
        Quaternion targetRotation = isDown
            ? openRotation
            : closedRotation;

        transform.localRotation = Quaternion.RotateTowards(
            transform.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void Toggle()
    {
        isDown = !isDown;
    }
}
