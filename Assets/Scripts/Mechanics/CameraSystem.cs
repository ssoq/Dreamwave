using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float smoothing = 2f;

    [Header("Instancing")]
    [SerializeField] public static CameraSystem cameraSystem;

    [Header("Positions")]
    [SerializeField] private Vector3 leftPos;
    [SerializeField] private Vector3 rightPos;
    [SerializeField] private Vector3 centrePos;

    private void Awake()
    {
        cameraSystem = this;
    }

    void LateUpdate()
    {
        SwitchFocus();
    }

    private void SwitchFocus()
    {
        switch (GameManager.Instance.focus)
        {
            case Focus.LeftPlayer:
                transform.position = Vector3.LerpUnclamped(transform.position, leftPos, smoothing * Time.deltaTime);
                break;
            case Focus.RightPlayer:
                transform.position = Vector3.LerpUnclamped(transform.position, rightPos, smoothing * Time.deltaTime);
                break;
            case Focus.Centre:
                transform.position = Vector3.LerpUnclamped(transform.position, centrePos, smoothing * Time.deltaTime);
                break;
        }
    }
}
