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

    private void Update()
    {
        if (tempoManager != null && GameManager.Instance.start && CanScroll && GameManager.Instance.canSongStart)
        {
            float scrollSpeed = tempoManager.beatsPerMinute * scrollSpeedMultiplier;

            // Use Time.deltaTime for frame rate independent movement
            transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed, Space.World);

            int currentStep = GameManager.Instance.stepCount;
            if (currentStep != previousStep && Mathf.Abs(transform.position.y - currentStep) < snapThreshold)
            {
                transform.position = new Vector3(transform.position.x, currentStep, transform.position.z);
                previousStep = currentStep;
            }
        }
    }

    private void OnPause(bool pausedState)
    {
        CanScroll = !pausedState;
    }
}
