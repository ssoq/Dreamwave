using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamwaveUserSetup : MonoBehaviour
{
    private void Start()
    {
        LoadUserSettings();
    }

    private void LoadUserSettings()
    {
        CheckPlayerNoteRenderPreferences();
    }

    private void CheckPlayerNoteRenderPreferences()
    {
        if (PlayerPrefs.GetInt("downScroll") == 1)
        {
            GameManager.Instance.noteUiSidePlayer.transform.rotation = Quaternion.Euler(180f, 0, 0);
            GameManager.Instance.noteUiSideOpponent.transform.rotation = Quaternion.Euler(180f, 0, 0);
        }
    }

    void Update()
    {
        
    }
}
