using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DocumentationPageViewer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private ScrollRect _documentationSlider; [SerializeField] private GameObject _document;

    [Header("Pages")]
    [SerializeField] private List<GameObject> _documentationPages = new();

    private void OnEnable()
    {
        _documentationSlider.content = _document.GetComponent<RectTransform>();
    }

    public void EnableDocumentationPage(int index)
    {
        for (int i = 0; i < _documentationPages.Count; i++)
        {
            _documentationPages[i].SetActive(false);
        }

        _document.transform.localPosition = Vector3.zero;
        _document = _documentationPages[index];
        _document.SetActive(true);
        _documentationSlider.content = _document.GetComponent<RectTransform>();
    }
}
