using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class DreamwaveGlobalDDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);

        StartCoroutine(LoadSceneSettings("Videos/TestVideo", "TestScene"));
    }

    public IEnumerator LoadSceneSettings(string cutsceneToPlayPath, string sceneAfterCutsceneOrIfNone)
    {
        if (cutsceneToPlayPath.Trim() != "")
        {
            var async = SceneManager.LoadSceneAsync("Cutscene");

            yield return new WaitUntil(() => async.isDone);

            VideoPlayer vp = GameObject.Find("Video Player").GetComponent<VideoPlayer>();

            if (vp != null)
            {
                string filePath = System.IO.Path.Combine($"{Application.streamingAssetsPath}/{cutsceneToPlayPath}.mp4");

                vp.url = filePath;

                yield return new WaitUntil(() => vp.isPrepared);

                vp.Play();

                vp.loopPointReached += OnVideoFinished;

                void OnVideoFinished(VideoPlayer player)
                {
                    vp.loopPointReached -= OnVideoFinished;

                    SceneManager.LoadSceneAsync(sceneAfterCutsceneOrIfNone);
                }
            }
            else SceneManager.LoadSceneAsync(sceneAfterCutsceneOrIfNone);
        }
        else
        {
            SceneManager.LoadSceneAsync(sceneAfterCutsceneOrIfNone);
        }
    }
}
