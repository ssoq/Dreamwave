using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer[] noteRenderers;
    [SerializeField] public Sprite[] noteSprites;
    [SerializeField] private Animator plrAnim;
    [SerializeField] private string currentAnimationPlaying;

    [Header("Animtion Settings")]
    [SerializeField] private float spriteCrossfadeSpeed = 0.1f;
    [SerializeField] private float threeDCrossfadeSpeed = 0.25f;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android) // disable those noties for mobile players as UI replaces their placement
        {
            for (int i = 0; i < noteRenderers.Length; i++)
            {
                noteRenderers[i].enabled = false;
            }
        }
    }

    void Update()
    {
        if (PauseMenu.instance._isPaused) return;

        ControlInput();
    }

    private void ControlInput()
    {
        if (Input.GetKeyDown(GameManager.Instance.left))
        {
            if (PlayerPrefs.GetString("chartPos") == "upScroll") noteRenderers[0].sprite = noteSprites[1];
            else if (PlayerPrefs.GetString("chartPos") == "downScroll") noteRenderers[3].sprite = noteSprites[1];
        }
        else if (Input.GetKeyUp(GameManager.Instance.left))
        {
            if (PlayerPrefs.GetString("chartPos") == "upScroll") noteRenderers[0].sprite = noteSprites[0];
            else if (PlayerPrefs.GetString("chartPos") == "downScroll") noteRenderers[3].sprite = noteSprites[0];
        }

        if (Input.GetKeyDown(GameManager.Instance.down))
        {
            if (PlayerPrefs.GetString("chartPos") == "upScroll") noteRenderers[1].sprite = noteSprites[1];
            else if (PlayerPrefs.GetString("chartPos") == "downScroll") noteRenderers[2].sprite = noteSprites[1];
        }
        else if (Input.GetKeyUp(GameManager.Instance.down))
        {
            if (PlayerPrefs.GetString("chartPos") == "upScroll") noteRenderers[1].sprite = noteSprites[0];
            else if (PlayerPrefs.GetString("chartPos") == "downScroll") noteRenderers[2].sprite = noteSprites[0];
        }

        if (Input.GetKeyDown(GameManager.Instance.up))
        {
            if (PlayerPrefs.GetString("chartPos") == "upScroll") noteRenderers[2].sprite = noteSprites[1];
            else if (PlayerPrefs.GetString("chartPos") == "downScroll") noteRenderers[1].sprite = noteSprites[1];
        }
        else if (Input.GetKeyUp(GameManager.Instance.up))
        {
            if (PlayerPrefs.GetString("chartPos") == "upScroll") noteRenderers[2].sprite = noteSprites[0];
            else if (PlayerPrefs.GetString("chartPos") == "downScroll") noteRenderers[1].sprite = noteSprites[0];
        }

        if (Input.GetKeyDown(GameManager.Instance.right))
        {
            if (PlayerPrefs.GetString("chartPos") == "upScroll") noteRenderers[3].sprite = noteSprites[1];
            else if (PlayerPrefs.GetString("chartPos") == "downScroll") noteRenderers[0].sprite = noteSprites[1];
        }
        else if (Input.GetKeyUp(GameManager.Instance.right))
        {
            if (PlayerPrefs.GetString("chartPos") == "upScroll") noteRenderers[3].sprite = noteSprites[0];
            else if (PlayerPrefs.GetString("chartPos") == "downScroll") noteRenderers[0].sprite = noteSprites[0];
        }
    }
}
