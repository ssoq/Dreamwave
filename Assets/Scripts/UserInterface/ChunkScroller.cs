// developed by sso

/// <summary>
/// You can setup the scroll chunks precisely by taking the yVal of a grid-group. If yVal is 250,
/// you +250 every time you add a new element.
/// 
/// An example: ChunkScroller contains 4 elements, and starts from the Y Position of 0.
/// 
/// A precise list of positions would go as follows:
/// 0,
/// 0 + 250 = 250,
/// 250 + 250 = 500,
/// 500 + 250 = 750
/// 
/// The ChunkScroller would scroll precisely.
/// 
/// This is only a quick prototype, I plan to add an actual way to automate this.
/// </summary>

using UnityEngine;

public class ChunkScroller : MonoBehaviour
{
    #region Variables

    [Header("Scroller")]
    [SerializeField] private GameObject _objectToScroll;

    [Header("Scroll Settings")]
    [SerializeField] private float _yScrollThreshold = 0f;
    [SerializeField] private float _scrollSmoothing = 5f;
    [SerializeField] private float[] _scrollChunkPositions;

    [Header("Scroll Animator Objects")]
    [SerializeField] private Animator[] _scrollAnimatorObjects;

    [Header("Scroll Index")]
    public int _scrollIndex; // was private at one point, not changing the variables name now. Later.

    #endregion

    #region Processes

    private void Start()
    {
        _scrollIndex = 0;
        _scrollAnimatorObjects[0].SetTrigger("active");
    }

    void Update()
    {
        if (!PauseMenu.instance._inModEditor && PauseMenu.instance._isPaused && PauseMenu.instance._inSettings)
        {
            GetScrollInput();
            UpdateScrollerPosition();
        }
    }

    private void OnEnable()
    {
        _scrollIndex = 0;
        _scrollAnimatorObjects[0].SetTrigger("active");
    }

    private void OnDisable()
    {
        _scrollIndex = 0;
        _scrollAnimatorObjects[0].SetTrigger("active");
    }

    #endregion

    #region Scroll Input

    private void GetScrollInput()
    {
        if (Input.mouseScrollDelta.y > _yScrollThreshold | Input.GetKey(Settings.instance.navUp)) // scroll up
        {
            ScrollChunkUp(1);
        }
        else if (Input.mouseScrollDelta.y < _yScrollThreshold | Input.GetKey(Settings.instance.navDown)) // scroll down
        {
            ScrollChunkDown(1);
        }
    }

    #endregion

    #region Scrolling Processes

    private void ScrollChunkUp(int i)
    {
        if (_scrollIndex > 0) { _scrollIndex -= i; _scrollAnimatorObjects[_scrollIndex].SetTrigger("active"); _scrollAnimatorObjects[_scrollIndex+1].SetTrigger("active"); }
    }

    private void ScrollChunkDown(int i)
    {
        if (_scrollIndex < _scrollAnimatorObjects.Length - 1) { _scrollIndex += i; _scrollAnimatorObjects[_scrollIndex].SetTrigger("active"); _scrollAnimatorObjects[_scrollIndex-1].SetTrigger("active"); }
    }

    #endregion

    #region UI Object Positioning On Scroll

    private void UpdateScrollerPosition()
    {
        _scrollIndex = Mathf.Clamp(_scrollIndex, 0, _scrollAnimatorObjects.Length - 1);
        _objectToScroll.transform.localPosition = Vector2.Lerp(_objectToScroll.transform.localPosition, new Vector2(0, _scrollChunkPositions[_scrollIndex]), _scrollSmoothing * Time.unscaledDeltaTime);
    }

    #endregion
}
