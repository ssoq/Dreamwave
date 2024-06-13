using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DocumentationPage : MonoBehaviour
{
    [SerializeField] private LuaCodeEditor _syntaxColourer;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _syntaxColourer.UpdateSyntaxHighlighting(_text.text);
    }
}
