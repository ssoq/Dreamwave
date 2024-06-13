using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class LuaCodeEditor : MonoBehaviour
{
    [Header("Internals")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text displayText;

    private void Start()
    {
        if (gameObject.tag == "LuaEditor")
        {
            inputField.onValueChanged.AddListener(UpdateSyntaxHighlighting);
            displayText.text = inputField.text;
            UpdateSyntaxHighlighting(inputField.text);
        }
    }

    public void UpdateSyntaxHighlighting(string text)
    {
        displayText.text = HighlightSyntax(text);
    }

    private string HighlightSyntax(string text)
    {
        Dictionary<string, string> syntaxColors = new()
        {
            // UFNF Functions
            { "Heartbeat", "#67F0DF" },    // Teal
            { "GetCurrentStep", "#67F0DF" },    // Teal
            { "GetTotalSteps", "#67F0DF" },    // Teal
            { "GetBpm", "#67F0DF" },    // Teal
            // UFNF UI
            { "CreateUiText", "#67F0DF" },    // Teal
            { "UpdateUiText", "#67F0DF" },    // Teal
            // UFNF Input
            // Left
            { "LeftKeyPressed", "#67F0DF" },    // Teal
            { "LeftKeyReleased", "#67F0DF" },    // Teal
            { "LeftKeyHeld", "#67F0DF" },    // Teal
            // Right
            { "RightKeyPressed", "#67F0DF" },    // Teal
            { "RightKeyReleased", "#67F0DF" },    // Teal
            { "RightKeyHeld", "#67F0DF" },    // Teal
            // Up
            { "UpKeyPressed", "#67F0DF" },    // Teal
            { "UpKeyReleased", "#67F0DF" },    // Teal
            { "UpKeyHeld", "#67F0DF" },    // Teal
            // Down
            { "DownKeyPressed", "#67F0DF" },    // Teal
            { "DownKeyReleased", "#67F0DF" },    // Teal
            { "DownKeyHeld", "#67F0DF" },    // Teal

            // UFNF Debug Functions
            { "Print", "#67F0DF" },    // Teal
            { "PrintDebug", "#67F0DF" },    // Teal

            // Lua Synax
            { "function", "#67A4F0" },    // Light Blue
            { "end", "#67A4F0" },    // Light Blue
            
            { "if", "#67F0A4" },       // Light Green
            { "elseif", "#67F0A4" },       // Light Green
            { "else", "#67F0A4" },     // Light Green
            { "and", "#67F0A4" },     // Light Green
            { "then", "#67F0A4" },    // Light Green
            { "do", "#67F0A4" },    // Light Green

            { "for", "#F0E267" },      // Light Yellow
            { "while", "#F0E267" },    // Light Yellow
            
            { "return", "#F0A867" },    // Light Orange
            { "local", "#F0A867" },    // Light Orange

            { "true", "#990000" },    // Dark Red
            { "false", "#990000" },    // Dark Red
            { "nil", "#990000" },    // Dark Red

            // Lua Functions
            { "tostring", "#FF6600" },    // Dark Orange
        };

        foreach (var entry in syntaxColors)
        {
            string keyword = entry.Key;
            string color = entry.Value;
            string pattern = "\\b" + Regex.Escape(keyword) + "\\b";
            text = Regex.Replace(text, pattern, $"<color={color}>{keyword}</color>");
        }

        text = HighlightComments(text);
        text = HighlightString(text);

        return text;
    }

    private string HighlightComments(string text)
    {
        string commentColor = "#F06767"; // Light Red

        string pattern = @"--.*?(?=\r?\n|$)";
        text = Regex.Replace(text, pattern, $"<color={commentColor}>$0</color>");

        return text;
    }

    private string HighlightString(string text)
    {
        string stringColor = "#4AC9BD"; // Turquoise Green

        // Highlight strings wrapped in single quotes
        string patternSingleQuotes = @"'[^']*'";
        text = Regex.Replace(text, patternSingleQuotes, $"<color={stringColor}>$0</color>");

        // Highlight strings wrapped in double quotes
        string patternDoubleQuotes = @"""[^""]*""";
        text = Regex.Replace(text, patternDoubleQuotes, $"<color={stringColor}>$0</color>");

        return text;
    }
}
