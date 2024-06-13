using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer[] noteRenderers;
    [SerializeField] private Sprite[] noteSprites;
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
            noteRenderers[0].sprite = noteSprites[1];
        }
        else if (Input.GetKeyUp(GameManager.Instance.left))
        {
            noteRenderers[0].sprite = noteSprites[0];
        }

        if (Input.GetKeyDown(GameManager.Instance.down))
        {
            noteRenderers[1].sprite = noteSprites[1];
        }
        else if (Input.GetKeyUp(GameManager.Instance.down))
        {
            noteRenderers[1].sprite = noteSprites[0];
        }

        if (Input.GetKeyDown(GameManager.Instance.up))
        {
            noteRenderers[2].sprite = noteSprites[1];
        }
        else if (Input.GetKeyUp(GameManager.Instance.up))
        {
            noteRenderers[2].sprite = noteSprites[0];
        }

        if (Input.GetKeyDown(GameManager.Instance.right))
        {
            noteRenderers[3].sprite = noteSprites[1];
        }
        else if (Input.GetKeyUp(GameManager.Instance.right))
        {
            noteRenderers[3].sprite = noteSprites[0];
        }
    }
}
