using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    public static MobileControls instance;

    private Dictionary<string, bool> _buttonStates = new();

    private void Awake()
    {
        if (Application.platform != RuntimePlatform.Android) this.gameObject.SetActive(false);

        instance = this;

        _buttonStates = new Dictionary<string, bool>
        {
            {"Left", false },
            {"Down", false },
            {"Up", false },
            {"Right", false }
        };
    }

    public void Left(bool state)
    {
        ButtonsPressed("Left", state);
    }

    public void Down(bool state)
    {
        ButtonsPressed("Down", state);
    }

    public void Up(bool state)
    {
        ButtonsPressed("Up", state);
    }

    public void Right(bool state)
    {
        ButtonsPressed("Right", state);
    }

    public void ButtonsPressed(string direction, bool down)
    {
        if (_buttonStates.ContainsKey(direction))
        {
            _buttonStates[direction] = down;
        }
    }

    public bool GetButtonsPressed(string buttonName)
    {
        if (_buttonStates.ContainsKey(buttonName))
        {
            return _buttonStates[buttonName];
        }
        return false;
    }
}
