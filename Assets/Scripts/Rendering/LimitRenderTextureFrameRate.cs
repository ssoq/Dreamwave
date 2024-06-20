// ****************************************************************************
//  Name    :  LimitCCTVFrameRate.cs
//  Author  :  Lewis-Lee
//  Date    :  07/12/2023
//  Time    :  20:20
//  Version :  V0.1
//  Copyright (C) 2023 SSO & AMBUSH
//
//  Unpublished-rights reserved under the Copyright Laws of the United Kingdom.
// ****************************************************************************
//  Description:
//      This script contains all of the logic for limiting CCTV
//      render rates for optimisation.
// ****************************************************************************
//  Comments:
//      Optimisation at it's finest :)
//
//      sso was here 0_0
// ****************************************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class LimitRenderTextureFrameRate : MonoBehaviour
{
    [Header("Rendering")]
    [SerializeField] private Camera _camera;

    [Header("Settings")]
    [SerializeField] private float FPS = 5f;
    [SerializeField] private float _fps;

    private void Start()
    {
        FPS = PlayerPrefs.GetFloat("ffps");
        _fps = 1f / FPS;
        StartCoroutine("Render");
    }

    void OnEnable()
    {
        PauseMenu.Pause += OnPause;
    }

    void OnDisable()
    {
        PauseMenu.Pause -= OnPause;
    }

    private IEnumerator Render()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(_fps);

            _camera.enabled = true;
            yield return new WaitForEndOfFrame(); // Wait until the end of the frame to ensure rendering is completed

            _camera.enabled = false;
        }
    }

    private void OnPause(bool paused)
    {
        FPS = PlayerPrefs.GetFloat("ffps");
        _fps = 1f / FPS;
    }
}
