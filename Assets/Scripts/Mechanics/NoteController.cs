using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private SpriteRenderer[] noteRenderers;
    [SerializeField] public List<Sprite> noteSpritesDown;
    [SerializeField] public List<Sprite> noteSpritesRelease;
    [SerializeField] private Animator plrAnim;
    [SerializeField] private string currentAnimationPlaying;

    [Header("Animtion Settings")]
    [SerializeField] private float noteAnimSpeed = 0.1f;

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

    void Update()
    {
        if (PauseMenu.instance._isPaused) return;

        ControlInput();
    }

    private void ControlInput() // why the fuck did I invert this shit - fix soon
    {
        if (Input.GetKeyDown(GameManager.Instance.left))
        {
            StopCoroutine(KeyReleaseSpriteFlick(0));
            StartCoroutine(KeyDownSpriteFlick(0));
        }
        else if (Input.GetKeyUp(GameManager.Instance.left))
        {
            StopCoroutine(KeyDownSpriteFlick(0));
            StartCoroutine(KeyReleaseSpriteFlick(0));
        }

        if (Input.GetKeyDown(GameManager.Instance.down))
        {
            StopCoroutine(KeyReleaseSpriteFlick(1));
            StartCoroutine(KeyDownSpriteFlick(1));
        }
        else if (Input.GetKeyUp(GameManager.Instance.down))
        {
            StopCoroutine(KeyDownSpriteFlick(1));
            StartCoroutine(KeyReleaseSpriteFlick(1));
        }

        if (Input.GetKeyDown(GameManager.Instance.up))
        {
            StopCoroutine(KeyReleaseSpriteFlick(2));
            StartCoroutine(KeyDownSpriteFlick(2));
        }
        else if (Input.GetKeyUp(GameManager.Instance.up))
        {
            StopCoroutine(KeyDownSpriteFlick(2));
            StartCoroutine(KeyReleaseSpriteFlick(2));
        }

        if (Input.GetKeyDown(GameManager.Instance.right)) // pressed - says it here but I think I am retarded so I need easier visuals
        {
            StopCoroutine(KeyReleaseSpriteFlick(3));
            StartCoroutine(KeyDownSpriteFlick(3));
        }
        else if (Input.GetKeyUp(GameManager.Instance.right)) // released
        {
            StopCoroutine(KeyDownSpriteFlick(3));
            StartCoroutine(KeyReleaseSpriteFlick(3));
        }
    }

    private IEnumerator KeyDownSpriteFlick(int index)
    {
        for (int i = 0; i < noteSpritesDown.Count; i++)
        {
            noteRenderers[index].sprite = noteSpritesDown[i];

            yield return new WaitForSecondsRealtime(noteAnimSpeed);

            if (i == noteSpritesDown.Count - 1) yield break;
        }

        noteRenderers[index].sprite = noteSpritesDown[noteSpritesDown.Count - 1];
        yield break;
    }

    private IEnumerator KeyReleaseSpriteFlick(int index)
    {
        for (int i = 0; i < noteSpritesRelease.Count; i++)
        {
            noteRenderers[index].sprite = noteSpritesRelease[i];

            yield return new WaitForSecondsRealtime(noteAnimSpeed);

            if (i == noteSpritesRelease.Count - 1) yield break;
        }

        noteRenderers[index].sprite = noteSpritesRelease[noteSpritesRelease.Count - 1];
        yield break;
    }

    private void OnPause(bool paused)
    {
        if (paused) StopAllCoroutines(); // temp - will add a way soon to store what the last frame was and play from there - as goes for all types of this fix
    }
}
