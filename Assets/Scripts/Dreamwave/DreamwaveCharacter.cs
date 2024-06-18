/*****************************************************************************
**                                         
**  Dreamverse Character
**
**  Name    :   DreamwaveCharacter.cs
**  Author  :   Lewis-Lee
** 
** 
**  Desc
**      A custom character animator for 2d chracters. Allows for easy
**      modding in-game without having to use the Unity Engine whatsoever.
**      Also better for this type of project than Unity's Animator Component.
**
*******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamwaveCharacter : MonoBehaviour
{
    [Header("Settings")]
    public bool IsCustom = false; // decides whether you're using a base character or a custom character

    [Header("States")]
    [SerializeField] private bool _isSinging = false;
    [SerializeField] private bool _canAnimate = true;

    [Header("Rendering")]
    public SpriteRenderer Renderer;
    public float AnimationSpeed = 0.1f;
    public float SingAnimationHold = 0.05f;

    [Header("Character Animation Lists")]
    public List<Sprite> LeftAnimations = new(); public List<Vector2> LeftOffsets = new();
    public List<Sprite> DownAnimations = new(); public List<Vector2> DownOffsets = new();
    public List<Sprite> UpAnimations = new(); public List<Vector2> UpOffsets = new();
    public List<Sprite> RightAnimations = new(); public List<Vector2> RightOffsets = new();
    public List<Sprite> IdleAnimation = new(); public List<Vector2> IdleOffsets = new();

    private void OnEnable()
    {
        TempoManager.OnStep += PlayStillAnimation;
        PauseMenu.Pause += OnPause;
    }

    private void OnDisable()
    {
        TempoManager.OnStep -= PlayStillAnimation;
        PauseMenu.Pause -= OnPause;
    }
    private void OnDestroy()
    {
        TempoManager.OnStep -= PlayStillAnimation;
        PauseMenu.Pause -= OnPause;
    }

    private void OnPause(bool paused)
    {
        if (paused) { _canAnimate = false; StopAllCoroutines(); }
        else _canAnimate = true;
    }

    private void Update()
    {
        InputStates();
        //AnimationStates();
    }

    private void InputStates()
    {
        if (!_canAnimate) return;
        if (!GameManager.Instance.canSongStart) return;

        if (Input.GetKeyDown(GameManager.Instance.left))
        {
            StopAllCoroutines();
            StartCoroutine(SingAnimation("Left"));
        }
        else if (Input.GetKeyDown(GameManager.Instance.down))
        {
            StopAllCoroutines();
            StartCoroutine(SingAnimation("Down"));
        }
        else if (Input.GetKeyDown(GameManager.Instance.up))
        {
            StopAllCoroutines();
            StartCoroutine(SingAnimation("Up"));
        }
        else if (Input.GetKeyDown(GameManager.Instance.right))
        {
            StopAllCoroutines();
            StartCoroutine(SingAnimation("Right"));
        }
    }

    private void PlayStillAnimation(int step)
    {
        switch (step)
        {
            case 2:
                if (!_isSinging)
                {
                    StopCoroutine("StillAnimation");
                    StartCoroutine("StillAnimation");
                }
                break;
            case 4:
                if (!_isSinging)
                {
                    StopCoroutine("StillAnimation");
                    StartCoroutine("StillAnimation");
                }
                break;
        }
    }

    private IEnumerator StillAnimation() // means idle but cannot use it due to variable using same name
    {
        for (int i = 0; i < IdleAnimation.Count; i++)
        {
            Renderer.sprite = IdleAnimation[i];
            if (IdleOffsets.Count != 0) Renderer.gameObject.transform.localPosition = IdleOffsets[i];
            yield return new WaitForSecondsRealtime(AnimationSpeed);

            if (i == IdleAnimation.Count - 1) yield break;
        }
    }

    private IEnumerator SingAnimation(string direction)
    {
        _isSinging = true;

        var animations = direction switch
        {
            "Left" => LeftAnimations,
            "Right" => RightAnimations,
            "Up" => UpAnimations,
            "Down" => DownAnimations,
            _ => IdleAnimation
        };
        if (animations.Count == 0) { _isSinging = false; yield break; }

        var offsets = direction switch
        {
            "Left" => LeftOffsets,
            "Right" => RightOffsets,
            "Up" => UpOffsets,
            "Down" => DownOffsets,
            _ => IdleOffsets
        };

        for (int i = 0; i < animations.Count; i++)
        {
            Renderer.sprite = animations[i];
            if (offsets.Count != 0) Renderer.gameObject.transform.localPosition = offsets[i];
            if (i != animations.Count - 1) yield return new WaitForSecondsRealtime(AnimationSpeed);

            if (i == animations.Count - 1)
            {
                yield return new WaitForSecondsRealtime(SingAnimationHold);
            }
        }

        _isSinging = false;
        yield break;
    }
}
