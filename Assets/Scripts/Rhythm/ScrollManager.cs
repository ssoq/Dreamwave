using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public float scrollSpeedMultiplier = 1.0f;
    public float snapThreshold = 0.05f;
    public TempoManager tempoManager;
    public bool CanScroll = true;

    private int previousStep;

    private void OnEnable()
    {
        PauseMenu.Pause += OnPause;
    }

    private void OnDisable()
    {
        PauseMenu.Pause -= OnPause;
    }

    private void OnDestroy()
    {
        PauseMenu.Pause -= OnPause;
    }

    private void FixedUpdate()
    {
        if (tempoManager != null && GameManager.Instance.start && CanScroll && GameManager.Instance.canSongStart)
        {
            float scrollSpeed = tempoManager.beatsPerMinute * scrollSpeedMultiplier;

            transform.Translate(Vector3.up * Time.fixedDeltaTime * scrollSpeed, Space.World);

            int currentStep = GameManager.Instance.stepCount;
            if (currentStep != previousStep && Vector3.Distance(transform.position, Vector3.up * currentStep) < snapThreshold)
            {
                transform.position = Vector3.up * currentStep;
                previousStep = currentStep;
            }
        }
    }

    private void OnPause(bool pausedState)
    {
        switch (pausedState)
        {
            case true:
                CanScroll = false;
                break;
            case false:
                CanScroll = true;
                break;
        }
    }
}