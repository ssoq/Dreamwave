using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TypeOfEngineBuild
{
    CustomBuild,
    Dreamwave
}

public class DreamwaveBootstrapper : MonoBehaviour
{
    [Header("Bootstrap Setup")]
    [SerializeField] private bool _completedBootstrap = false;
    [SerializeField] private TypeOfEngineBuild _typeOfEngineBuild;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _bootstrapProgressText;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "Bootstrap.txt");

        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split("=");

                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        if (int.TryParse(value, out int valueStr))
                        {
                            SetupGameSettings(key, valueStr);
                        }
                    }
                }
            }

            _completedBootstrap = true;
        }
    }

    private void SetupGameSettings(string key, int value)
    {
        switch (key)
        {
            case "_isCustomEngine":
                if (value == 1) _typeOfEngineBuild = TypeOfEngineBuild.CustomBuild;
                else _typeOfEngineBuild = TypeOfEngineBuild.Dreamwave;

                _bootstrapProgressText.text = $"Build Type: {_typeOfEngineBuild.ToString()}";

                StartCoroutine(InitEngineBuildType());
                break;
        }
    }

    private IEnumerator InitEngineBuildType()
    {
        yield return new WaitUntil(() => _completedBootstrap);
        
        yield return new WaitForSecondsRealtime(1f);

        _bootstrapProgressText.text = "Completed! Now loading...";

        yield return new WaitForSecondsRealtime(1f);

        switch (_typeOfEngineBuild)
        {
            case TypeOfEngineBuild.CustomBuild:
                SceneManager.LoadSceneAsync("CustomMenu");
                break;
            case TypeOfEngineBuild.Dreamwave:
                SceneManager.LoadSceneAsync("Menu");
                break;
        }
    }
}
