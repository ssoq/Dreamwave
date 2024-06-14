using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncomingNoteWarning : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sideWarningRenderer;

    [Header("States")]
    [SerializeField] private bool _showSideWarning;

    [Header("Settings")]
    [SerializeField] private float _smoothing = 15f;

    private void LateUpdate()
    {
        Color color = _sideWarningRenderer.color;
        
        if (_showSideWarning)
        {
            while (color.a < 1)
            {
                color.a = Mathf.Lerp(color.a, 1, _smoothing * Time.deltaTime);
                _sideWarningRenderer.color = color;

                if (color.a >= 1) break;
            }
        }
        else
        {
            while (color.a > 0)
            {
                color.a = Mathf.Lerp(color.a, 0, _smoothing * Time.deltaTime);
                _sideWarningRenderer.color = color;

                if (color.a <= 0) break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            _showSideWarning = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            _showSideWarning = false;
        }
    }
}
