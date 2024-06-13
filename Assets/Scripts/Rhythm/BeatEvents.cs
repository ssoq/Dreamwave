using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfBeatEvent 
{
    Animation,
    Trigger
}

public class BeatEvents : MonoBehaviour
{
    [Header("States")]
    [SerializeField] public TypeOfBeatEvent typeOfBeatEvent;

    [Header("Animators")]
    [SerializeField] private GameObject[] animatorObjects;
    [SerializeField] private Animator[] animators;

    private void Start()
    {
        TempoManager.OnBeat += OnBeatHandler;
        TempoManager.OnStep += OnStepHandler;

        animatorObjects = GameObject.FindGameObjectsWithTag("BeatAnimator");

        for (int i = 0; i < animatorObjects.Length; i++)
        {
            animators[i] = animatorObjects[i].gameObject.GetComponent<Animator>();
        }
    }

    private void OnBeatHandler()
    {
    }

    private void OnStepHandler(int step) 
    {
        switch (typeOfBeatEvent)
        {
            case TypeOfBeatEvent.Animation:
                for (int i = 0; i < animators.Length; i++)
                {
                    if (animators[i] != null) animators[i].SetTrigger("onBeat");
                }
                break;
            case TypeOfBeatEvent.Trigger:
                Debug.Log("WIP");
                break;
        }
    }

    private void OnDestroy()
    {
        TempoManager.OnBeat -= OnBeatHandler;
        TempoManager.OnStep -= OnStepHandler;
    }

    private void OnDisable()
    {
        TempoManager.OnBeat -= OnBeatHandler;
        TempoManager.OnStep -= OnStepHandler;
    }
}
