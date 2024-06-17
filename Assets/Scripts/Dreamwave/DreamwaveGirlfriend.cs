using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
**                                         
**  Dreamwave Girlfriend
**
**  Name    :   DreamwaveGirlfriend.cs
**  Author  :   Lewis-Lee
** 
** 
**  Desc
**      A custom character animator for 2d chracters. Allows for easy
**      modding in-game without having to use the Unity Engine whatsoever.
**      Also better for this type of project than Unity's Animator Component.
**      Dreamwave Girlfriend animates the girlfriend on beat like FNF along
**      with some extra functionalities like looking at the current singer.
**      Could otherwise be known as a background dancer. Simply animates on beat.
**
*******************************************************************************/

public class DreamwaveGirlfriend : MonoBehaviour
{
    #region Variables

    // I should make a class for these guys to inherit from as they all use the same typa shit, will change
    [Header("Sprite Animations")]
    // singing if used
    [SerializeField] public List<Sprite> LeftFrames = new(); public List<Vector2> LeftFramesOffset = new();
    [SerializeField] public List<Sprite> DownFrames = new(); public List<Vector2> DownFramesOffset = new();
    [SerializeField] public List<Sprite> UpFrames = new(); public List<Vector2> UpFramesOffset = new();
    [SerializeField] public List<Sprite> RightFrames = new(); public List<Vector2> RightFramesOffset = new();

    [Space(10f)]
    // some cheeky extra animations
    [SerializeField] public List<Sprite> IdleFramesLeft = new(); public List<Vector2> IdleLeftFramesOffset = new();
    [SerializeField] public List<Sprite> IdleFramesRight = new(); public List<Vector2> IdleRightFramesOffset = new();
    [SerializeField] public List<Sprite> BFMissedFrames = new(); public List<Vector2> BFMissedFramesOffset = new();
    [SerializeField] public List<Sprite> GFCheerBFFrames = new(); public List<Vector2> GFCheerFramesOffset = new();

    [Space(10f)]
    // some cheeky extra EXTRA animations
    [SerializeField] public List<Sprite> IdleFramesLookingOpponentOne = new(); public List<Vector2> IdleOPOneFramesOffset = new();
    [SerializeField] public List<Sprite> IdleFramesLookingOpponentTwo = new(); public List<Vector2> IdleOPTwoFramesOffset = new();
    [SerializeField] public List<Sprite> GFLookOpponentOne = new(); public List<Vector2> GFLookOPOneFramesOffset = new();
    [SerializeField] public List<Sprite> GFLookOpponentTwo = new(); public List<Vector2> GFLookOPTwoFramesOffset = new();

    [Header("Components")]
    [SerializeField] private SpriteRenderer _gfSpriteRenderer;

    [Header("Animation Settings")]
    [SerializeField] public float AnimationFlickDelay = 0.1f;
    [SerializeField] public float AnimationHoldTime = 0.1f;

    #endregion

    #region Processes
    private void Awake() => _gfSpriteRenderer = GetComponent<SpriteRenderer>();
    private void OnEnable() => TempoManager.OnStep += OnStep;
    private void OnDisable() => TempoManager.OnStep -= OnStep;
    private void OnDestroy() => TempoManager.OnStep -= OnStep;
    #endregion
    
    private void OnStep(int step)
    {
        if (step == 4)
        {
            StopCoroutine("IdleAnimation");
            StartCoroutine("IdleAnimation");
        }
    }

    #region Idle Animation

    private string _whichSide = "Left"; // dont think which side matters by default
    private IEnumerator IdleAnimation()
    {
        switch (_whichSide)
        {
            case "Left":
                for (int i = 0; i < IdleFramesLeft.Count; i++)
                {
                    _gfSpriteRenderer.sprite = IdleFramesLeft[i];
                    if (IdleLeftFramesOffset.Count != 0) _gfSpriteRenderer.transform.position = IdleLeftFramesOffset[i];

                    yield return new WaitForSecondsRealtime(AnimationFlickDelay);

                    if (i == IdleFramesLeft.Count - 1) yield break;
                }
                _whichSide = "Right";
                break;
            case "Right":
                for (int i = 0; i < IdleFramesRight.Count; i++)
                {
                    _gfSpriteRenderer.sprite = IdleFramesRight[i];
                    if (IdleRightFramesOffset.Count != 0) _gfSpriteRenderer.transform.position = IdleRightFramesOffset[i];

                    yield return new WaitForSecondsRealtime(AnimationFlickDelay);

                    if (i == IdleFramesRight.Count - 1) yield break;
                }
                _whichSide = "Left";
                break;
        }

        yield break;
    }

    #endregion
}
