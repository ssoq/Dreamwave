using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModEditor : MonoBehaviour
{
    [Header("Instancing")]
    public static ModEditor instance;

    [Header("Editor UI")]
    [SerializeField] private TextMeshProUGUI _editorTimeDisplay;
    [SerializeField] private TextMeshProUGUI _editorDateDisplay;

    [Header("Window Setup")]
    [SerializeField] private GameObject _windowContainer;
    [Header("Window Prefabs")]
    [SerializeField] private GameObject _basicWindowPrefab;
    [SerializeField] private GameObject _luaEditorWindowPrefab;
    [SerializeField] private GameObject _luaDocsWindowPrefab;

    private void Awake() => instance = this;
    private void OnDestroy() => instance = null;

    void Start()
    {
        UpdateEditorClock();
        CreateTextBasedWindow("Welcome", "Welcome to the UFNF " +
            "Mod Editor! Here, you can easily edit the current scene, " +
            "program new features with Lua or Blueprints, modify a chart, " +
            "or create an entirely new one. You can even test your mod!\r\n\r\n" +
            "The current message is displayed within a window. The UFNF Mod Editor " +
            "uses windows to display different features for the sake of " +
            "convenience.\r\n\r\nYou can drag windows around, scale them up or down, " +
            "close, open, and much more. You can even use multiple at once for multitasking " +
            "capabilities!\r\n\r\nHave fun! - Lewis");
    }

    private void OnEnable()
    {
        UpdateEditorClock();
        StartCoroutine("UpdateEditorClockRoutine");
    }

    private void OnDisable()
    {
        StopCoroutine("UpdateEditorClockRoutine");
        instance = null;
    }

    void Update()
    {

    }

    private string _date;
    private string _time;
    private void UpdateEditorClock()
    {
        _time = System.DateTime.Now.ToString("HH:mm");
        _date = System.DateTime.Now.ToString("dd/MM/yyyy");
        _editorTimeDisplay.text = _time;
        _editorDateDisplay.text = _date;
    }

    private IEnumerator UpdateEditorClockRoutine()
    {
        yield return new WaitUntil(() => _time != System.DateTime.Now.ToString("HH:mm"));
        UpdateEditorClock();
        StartCoroutine("UpdateEditorClockRoutine");
    }

    public void CreateTextBasedWindow(string _windowName, string _messageContent)
    {
        var _window = Instantiate(_basicWindowPrefab, Vector3.zero, Quaternion.identity);
        var _windowScript = _window.GetComponent<Window>();
        _window.transform.SetParent(_windowContainer.transform);
        _window.transform.localPosition = Vector3.zero;
        _windowScript.WindowTitleText.text = _windowName;
        _windowScript.WindowContentText.text = _messageContent;
    }

    public void CreateLuaEditorWindow()
    {
        var _window = Instantiate(_luaEditorWindowPrefab, Vector3.zero, Quaternion.identity);
        _window.transform.SetParent(_windowContainer.transform);
        _window.transform.localPosition = Vector3.zero;
    }

    public void CreateLuaDocumentationWindow()
    {
        var _window = Instantiate(_luaDocsWindowPrefab, Vector3.zero, Quaternion.identity);
        _window.transform.SetParent(_windowContainer.transform);
        _window.transform.localPosition = Vector3.zero;
    }
}
