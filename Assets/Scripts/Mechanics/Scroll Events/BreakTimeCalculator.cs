using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum WhichEvent
{
    Start,
    End
}

public class BreakTimeCalculator : MonoBehaviour
{
    public WhichEvent WhichEvent;
    public bool StartTimeCalc = false;
    private float breakTime = 0f;
    [SerializeField] private BreakTimeCalculator _breakTimeCalculator;
    private TextMeshProUGUI _breakTimeText;
    private Animator _breakTimeAnimator;
    private ScrollManager _scrollManager;

    [Header("Two Event Objects")]
    [SerializeField] private GameObject[] _eventObjects;

    private void Start()
    {
        _scrollManager = GameObject.FindGameObjectWithTag("ChartScroller").GetComponent<ScrollManager>();
        _breakTimeText = GameObject.FindGameObjectWithTag("BreakText").GetComponent<TextMeshProUGUI>();
        _breakTimeAnimator = GameObject.FindGameObjectWithTag("BreakTimeAnimator").GetComponent<Animator>();
    }

    void Update()
    {
        if (WhichEvent == WhichEvent.Start) CalculateBreakTime();
    }

    private void CalculateBreakTime()
    {
        if (!StartTimeCalc) return;

        float distance = Vector2.Distance(_eventObjects[0].transform.position, _eventObjects[1].transform.position);
        breakTime = (distance / _scrollManager.scrollSpeedMultiplier);

        string timeInString = Mathf.FloorToInt(breakTime).ToString("F0");

        timeInString = timeInString.Substring(0, 1);

        if (breakTime >= 100) _breakTimeText.text = timeInString;
        else _breakTimeText.text = "0";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ScrollEventTrigger"))
        {
            switch (WhichEvent)
            {
                case WhichEvent.Start:
                    StartTimeCalc = true;
                    _breakTimeAnimator.SetTrigger("cycle");
                    break;
                case WhichEvent.End:
                    _breakTimeCalculator.StartTimeCalc = false;
                    _breakTimeAnimator.SetTrigger("cycle");
                    break;
            }
        }
    }
}
