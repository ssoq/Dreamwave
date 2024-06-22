using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DreamwaveIcon : MonoBehaviour
{
    [Header("Animation")] // custom icon animation sprites and offsets
    [SerializeField] public List<Sprite> _neutral = new(); [SerializeField] public List<Vector2> _neutralOffsets = new();
    [SerializeField] public List<Sprite> _critical = new(); [SerializeField] public List<Vector2> _criticalOffsets = new();
    [SerializeField] public List<Sprite> _losing = new(); [SerializeField] public List<Vector2> _losingOffsets = new();
    [SerializeField] public List<Sprite> _winning = new(); [SerializeField] public List<Vector2> _winningOffsets= new();

    [Header("Settings")]
    [SerializeField] private WhichSide _whichSide;
    [SerializeField] private float _animationSpeed = 0.1f;
    [SerializeField] private float _animationDelay = 0.2f;

    [Header("Components")]
    [SerializeField] private Image _renderer;
    [SerializeField] private Animation _bumpAnimation;

    private void Start()
    {
        _renderer = GetComponent<Image>();
        _bumpAnimation = GetComponent<Animation>();
    }

    private void OnEnable()
    {
        TempoManager.OnStep += OnStep;
    }

    private void OnDisable()
    {
        TempoManager.OnStep -= OnStep;
    }

    private void OnDestroy()
    {
        TempoManager.OnStep -= OnStep;
    }

    private void OnStep(int step)
    {
        if (step == 2)
        {
            _bumpAnimation.Play();
            AnimationOnHealthState();
        }
        else if (step == 4)
        {
            _bumpAnimation.Play();
            AnimationOnHealthState();
        }
    }

    private void AnimationOnHealthState()
    {
        var health = GameManager.Instance.health;

        if (WithinRange(health, 0, 20))
        {
            switch (_whichSide)
            {
                case WhichSide.Left:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Winning"));
                    break;
                case WhichSide.Right:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Critical"));
                    break;
            }
        }
        else if (WithinRange(health, 20, 40))
        {
            switch (_whichSide)
            {
                case WhichSide.Left:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Neutral"));
                    break;
                case WhichSide.Right:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Losing"));
                    break;
            }
        }
        else if (WithinRange(health, 40, 60))
        {
            switch (_whichSide)
            {
                case WhichSide.Left:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Neutral"));
                    break;
                case WhichSide.Right:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Neutral"));
                    break;
            }
        }
        else if (WithinRange(health, 60, 80))
        {
            switch (_whichSide)
            {
                case WhichSide.Left:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Losing"));
                    break;
                case WhichSide.Right:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Neutral"));
                    break;
            }
        }
        else if (WithinRange(health, 80, 101))
        {
            switch (_whichSide)
            {
                case WhichSide.Left:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Critical"));
                    break;
                case WhichSide.Right:
                    StopAllCoroutines();
                    StartCoroutine(IconAnimation("Winning"));
                    break;
            }
        }

    }

    private IEnumerator IconAnimation(string state)
    {
        var animations = state switch
        { 
            "Neutral" => _neutral,
            "Critical" => _critical,
            "Losing" => _losing,
            "Winning" => _winning,
            _ => _neutral
        };
        var offsets = state switch
        {
            "Neutral" => _neutralOffsets,
            "Critical" => _criticalOffsets,
            "Losing" => _losingOffsets,
            "Winning" => _winningOffsets,
            _ => _neutralOffsets
        };

        if (animations.Count <= 0) yield break;

        for (int i = 0; i < animations.Count; i++)
        {
            _renderer.sprite = animations[i];
            if (offsets.Count != 0) _renderer.transform.parent.localPosition = offsets[i];

            yield return new WaitForSecondsRealtime(_animationSpeed);

            if (i == animations.Count - 1) break;
        }

        yield return new WaitForSecondsRealtime(_animationDelay);

        yield break;
    }

    #region Some math range check shite
    private bool WithinRange(int init, int min, int max)
    {
        bool inRange = (init >= min && init <= max);
        return inRange;
    }
    #endregion
}
