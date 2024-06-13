using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{
    #region Variables

    public static Settings instance;

    [Header("Player Preferences")]
    #region UI Navigation
    public KeyCode back;
    public KeyCode navUp;
    public KeyCode navDown;
    public KeyCode navLeft;
    public KeyCode navRight;
    #endregion
    #region Gameplay
    public KeyCode gameUp;
    public KeyCode gameDown;
    public KeyCode gameLeft;
    public KeyCode gameRight;
    #endregion

    [Header("GameObjects")]
    [SerializeField] private GameObject _settingsObj;

    [Header("Scripts")]
    [SerializeField] private ChunkScroller _chunkScroller;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI[] settingsTextAssets;

    #endregion

    private void OnEnable() => instance = this;
    private void OnDisable() => instance = null;
    private void OnDestroy() => instance = null;

    private void Start()
    {
        localFps = PlayerPrefs.GetInt("_preferredFrameRate");
        localFfps = PlayerPrefs.GetFloat("_preferredFixedFrameRate");

        ConvertStringToSpriteText(settingsTextAssets[0], localFps.ToString("F0"));
        ConvertStringToSpriteText(settingsTextAssets[1], localFfps.ToString());
        
        Application.targetFrameRate = localFps;
        Time.fixedDeltaTime = (1.0f / localFfps);
    }

    private void Update()
    {
        if (!PauseMenu.instance._inModEditor && PauseMenu.instance._isPaused && PauseMenu.instance._inSettings) SwitchScrollIndex();
    }

    private void SwitchScrollIndex()
    {
        switch (_chunkScroller._scrollIndex)
        {
            case 0:
                UpdateSavedFrameRate();
                break;
            case 1:
                UpdateSavedFixedFrameRate();
                break;
        }
    }

    #region Frame Rate Setting

    private int localFps;
    public void UpdateSavedFrameRate()
    {
        if (Input.GetKeyDown(navLeft))
        {
            if (localFps > 30) localFps -= 10;
            Application.targetFrameRate = localFps;
            ConvertStringToSpriteText(settingsTextAssets[0], localFps.ToString("F0"));
            PlayerPrefs.SetInt("_preferredFrameRate", localFps);
            PlayerPrefs.Save();
        }
        else if (Input.GetKeyDown(navRight))
        {
            if (localFps < 990) localFps += 10;
            Application.targetFrameRate = localFps;
            ConvertStringToSpriteText(settingsTextAssets[0], localFps.ToString("F0"));
            PlayerPrefs.SetInt("_preferredFrameRate", localFps);
            PlayerPrefs.Save();
        }
    }

    #endregion

    #region Fixed Frame Rate Setting

    public float localFfps;
    public void UpdateSavedFixedFrameRate()
    {
        if (Input.GetKeyDown(navLeft))
        {
            if (localFfps > 30) localFfps -= 10;
            Time.fixedDeltaTime = (1 / localFfps);
            ConvertStringToSpriteText(settingsTextAssets[1], localFfps.ToString("F0"));
            PlayerPrefs.SetFloat("_preferredFixedFrameRate", localFfps);
            PlayerPrefs.Save();
        }
        else if (Input.GetKeyDown(navRight))
        {
            if (localFfps < 120) localFfps += 10;
            Time.fixedDeltaTime = (1 / localFfps);
            ConvertStringToSpriteText(settingsTextAssets[1], localFfps.ToString("F0"));
            PlayerPrefs.SetFloat("_preferredFixedFrameRate", localFfps);
            PlayerPrefs.Save();
        }
    }

    #endregion

    #region String To Sprite Converter

    private void ConvertStringToSpriteText(TextMeshProUGUI textAsset, string input)
    {
        char[] splitInput = input.ToCharArray();

        textAsset.text = string.Empty;

        foreach (char c in splitInput)
        {
            textAsset.text += $"<sprite name=\"{c}\">";
        }
    }

    #endregion
}
