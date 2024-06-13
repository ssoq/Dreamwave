using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour
{
    [Header("Internals")]
    [SerializeField] private GameObject _windowInterface;
    [SerializeField] private GameObject _toolbar;

    [Header("Externals")]
    public TextMeshProUGUI WindowTitleText;
    public TextMeshProUGUI WindowContentText;

    [Header("UI")]
    [SerializeField] private RawImage _windowViewRenderer;
    [SerializeField] private Texture[] _windowViewTextures;
    [SerializeField] private RectTransform _windowRect;
    [SerializeField] private Transform[] _windowCorners;

    private void OnEnable() => UpdateZIndex();

    private void Awake() => _windowRect = GetComponent<RectTransform>();

    public void UpdateZIndex()
    {
        transform.SetAsLastSibling();
    }

    public void Drag()
    {
        transform.position = Input.mousePosition - new Vector3(0, _toolbar.transform.localPosition.y, 0);
    }

    public void Close()
    {
        Destroy(gameObject);
    }

    public void WindowViewState()
    {
        switch (_windowInterface.activeSelf)
        {
            case true:
                _windowInterface.SetActive(false);
                _windowViewRenderer.texture = _windowViewTextures[0];
                break;
            case false:
                _windowInterface.SetActive(true);
                _windowViewRenderer.texture = _windowViewTextures[1];
                break;
        }
    }

    public void StretchWindow()
    {
        _windowRect.sizeDelta += new Vector2(Input.mousePosition.x - _windowCorners[0].transform.position.x, Input.mousePosition.y - _windowCorners[0].transform.position.y);

        if (_windowRect.sizeDelta.x <= 736.54f) _windowRect.sizeDelta = new Vector2(736.54f, _windowRect.sizeDelta.y);
        if (_windowRect.sizeDelta.y <= 458.05f) _windowRect.sizeDelta = new Vector2(_windowRect.sizeDelta.x, 458.05f);
    }
}
