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
using UnityEngine.Rendering;

[RequireComponent(typeof(Camera))]

public class LimitRenderTextureFrameRate : MonoBehaviour
{
    [Header("Rendering")]
    [SerializeField] private Camera _camera;

    [Header("Settings")]
    [SerializeField] private float FPS = 5f;

    private void Start()
    {
        FPS = Settings.instance.localFfps;
        InvokeRepeating("Render", 0f, 1f / FPS);
    }

    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += RenderPipelineManager_endCameraRendering;
    }

    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= RenderPipelineManager_endCameraRendering;
    }

    private void Render()
    {
        _camera.enabled = true;
    }

    private void RenderPipelineManager_endCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }

    private void OnPostRender()
    {
        _camera.enabled = false;
    }
}
