using UnityEngine;
using MoonSharp.Interpreter;
using TMPro;
using System.Collections.Generic;
using System;

public class LuaInteractions : MonoBehaviour
{
    public static LuaInteractions Instance;
    private Script _luaScript;

    [Header("Lua Script Settings")]
    private List<Script> _activeScripts = new();

    [Header("Lua Script Internals")]
    private DynValue _heartbeatFunction;

    private void Awake()
    {
        Instance = this;
        _luaScript = new Script(CoreModules.Preset_Complete);

        UserData.RegisterAssembly();
        InitLuaGlobals();

        Script.DefaultOptions.DebugPrint = message => Debug.Log("[Lua] " + message);
    }

    private void InitLuaGlobals()
    {
        // Debug
        _luaScript.Globals["Print"] = (System.Action<string>)Print;
        _luaScript.Globals["PrintDebug"] = (System.Action<string>)PrintDebug;

        // UI
        _luaScript.Globals["CreateUiText"] = (Action<string, string, int[], float, float[]>)CreateTMPElement;
        _luaScript.Globals["UpdateUiText"] = (Action<string, string, int[], float, float[]>)UpdateExistingTMPElement;

        // UFNF Functions
        _luaScript.Globals["GetBpm"] = (System.Func<float>)GetBpm;
        _luaScript.Globals["GetCurrentStep"] = (System.Func<int>)GetCurrentStep;
        _luaScript.Globals["GetTotalSteps"] = (System.Func<int>)GetTotalSteps;
        #region Input Binding
        // left key
        _luaScript.Globals["LeftKeyPressed"] = (System.Func<bool>)PlayerLeftKeyPressed;
        _luaScript.Globals["LeftKeyReleased"] = (System.Func<bool>)PlayerLeftKeyReleased;
        _luaScript.Globals["LeftKeyHeld"] = (System.Func<bool>)PlayerLeftKeyHeld;

        // right key
        _luaScript.Globals["RightKeyPressed"] = (System.Func<bool>)PlayerRightKeyPressed;
        _luaScript.Globals["RightKeyReleased"] = (System.Func<bool>)PlayerRightKeyReleased;
        _luaScript.Globals["RightKeyHeld"] = (System.Func<bool>)PlayerRightKeyHeld;

        // up key
        _luaScript.Globals["UpKeyPressed"] = (System.Func<bool>)PlayerUpKeyPressed;
        _luaScript.Globals["UpKeyReleased"] = (System.Func<bool>)PlayerUpKeyReleased;
        _luaScript.Globals["UpKeyHeld"] = (System.Func<bool>)PlayerUpKeyHeld;

        // down key
        _luaScript.Globals["DownKeyPressed"] = (System.Func<bool>)PlayerDownKeyPressed;
        _luaScript.Globals["DownKeyReleased"] = (System.Func<bool>)PlayerDownKeyReleased;
        _luaScript.Globals["DownKeyHeld"] = (System.Func<bool>)PlayerDownKeyHeld;
        #endregion

    }

    private void Update()
    {
        if (_heartbeatFunction != null && _heartbeatFunction.IsNotNil()) _luaScript.Call(_heartbeatFunction, Time.deltaTime);
    }

    public void ParseLua()
    {
        TMP_InputField inputField = GameObject.FindGameObjectWithTag("LuaEditor").GetComponent<TMP_InputField>();
        if (inputField != null)
        {
            string scriptText = inputField.text;
            //Debug.Log("Lua Script: " + scriptText);
            ExecuteLuaScript(scriptText);
        }
        else
        {
            Debug.LogError("TMP_InputField not found with tag 'LuaEditor'");
        }
    }

    private void ExecuteLuaScript(string script)
    {
        try
        {
            _luaScript.DoString(script);
            // after script is loaded check for Heartbeat function
            _heartbeatFunction = _luaScript.Globals.Get("Heartbeat");
            if (_heartbeatFunction.Type != DataType.Function) _heartbeatFunction = null; // not a function then discard it
        }
        catch (ScriptRuntimeException ex)
        {
            //Debug.LogError("Execution Error: " + ex.Message);
            ModEditor.instance.CreateTextBasedWindow("Script Runtime Exception", "Your Lua script produced the following error: " + ex.Message);
        }
        catch (SyntaxErrorException ex)
        {
            ModEditor.instance.CreateTextBasedWindow("Syntax Error Exception", "Your Lua script(s) contains invalid syntax: " + ex.Message);
        }
    }

    #region Lua Functions

    public void Print(string message)
    {
        Debug.Log(message);
    }

    public void PrintDebug(string message)
    {
        ModEditor.instance.CreateTextBasedWindow("Print Debug", $"Debug: {message}");
    }

    public void CreateTMPElement(string name, string text, int[] rgba, float alpha, float[] pos)
    {
        var tmp = new GameObject(name).AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.color = new Color(rgba[0], rgba[1], rgba[2], alpha);
        tmp.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        tmp.transform.position = new Vector2(pos[0], pos[1]);
    }

    public void UpdateExistingTMPElement(string name, string text, int[] rgba, float alpha, float[] pos)
    {
        var tmp = GameObject.Find(name).GetComponent<TextMeshProUGUI>();

        tmp.text = text;
        tmp.color = new Color(rgba[0], rgba[1], rgba[2], alpha);
        tmp.transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        tmp.transform.position = new Vector2(pos[0], pos[1]);
    }

    public float GetBpm()
    {
        return TempoManager.instance.beatsPerMinute;
    }

    public int GetCurrentStep()
    {
        return TempoManager.instance.currentStep;
    }

    public int GetTotalSteps()
    {
        return GameManager.Instance.stepCount;
    }

    #region Exposed Input Functions

    #region Left Key States
    public bool PlayerLeftKeyPressed()
    {
        bool res = Input.GetKeyDown(GameManager.Instance.left);
        return res;
    }
    public bool PlayerLeftKeyReleased()
    {
        bool res = Input.GetKeyUp(GameManager.Instance.left);
        return res;
    }
    public bool PlayerLeftKeyHeld()
    {
        bool res = Input.GetKey(GameManager.Instance.left);
        return res;
    }
    #endregion

    #region Right Key States
    public bool PlayerRightKeyPressed()
    {
        bool res = Input.GetKeyDown(GameManager.Instance.right);
        return res;
    }
    public bool PlayerRightKeyReleased()
    {
        bool res = Input.GetKeyUp(GameManager.Instance.right);
        return res;
    }
    public bool PlayerRightKeyHeld()
    {
        bool res = Input.GetKey(GameManager.Instance.right);
        return res;
    }
    #endregion

    #region Up Key States
    public bool PlayerUpKeyPressed()
    {
        bool res = Input.GetKeyDown(GameManager.Instance.up);
        return res;
    }
    public bool PlayerUpKeyReleased()
    {
        bool res = Input.GetKeyUp(GameManager.Instance.up);
        return res;
    }
    public bool PlayerUpKeyHeld()
    {
        bool res = Input.GetKey(GameManager.Instance.up);
        return res;
    }
    #endregion

    #region Down Key States
    public bool PlayerDownKeyPressed()
    {
        bool res = Input.GetKeyDown(GameManager.Instance.down);
        return res;
    }
    public bool PlayerDownKeyReleased()
    {
        bool res = Input.GetKeyUp(GameManager.Instance.down);
        return res;
    }
    public bool PlayerDownKeyHeld()
    {
        bool res = Input.GetKey(GameManager.Instance.down);
        return res;
    }
    #endregion

    #endregion

    #endregion
}
