#if !UNITY_ANDROID && !UNITY_IOS
using Discord;
#endif
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiscordController : MonoBehaviour
{
    public long applicationID;
    [Space]
    public string state = "";
    public string details = "";
    [Space]
    public string largeImage = "";
    public string largeText = "UFNF";

#if !UNITY_ANDROID && !UNITY_IOS
    private Discord.Discord discord;
#endif
    private double songStartTime;

    private static bool instanceExists;
    public static DiscordController instance;

    private void Awake()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            // If running on mobile platforms, disable this GameObject
            this.gameObject.SetActive(false);
        }

#if !UNITY_EDITOR
        if (!instanceExists)
        {
            instanceExists = true;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
#endif
    }

    private void Start()
    {
#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_EDITOR
        discord = new Discord.Discord(applicationID, (ulong)Discord.CreateFlags.NoRequireDiscord);
        StartSong();
        SceneManager.sceneLoaded += OnSceneLoaded;
#endif
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_EDITOR
        if (discord != null)
        {
            discord.Dispose();
        }
#endif
    }

    void StartSong()
    {
        songStartTime = AudioSettings.dspTime;
    }

    void Update()
    {
#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_EDITOR
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(gameObject);
        }
#endif
    }

    private void LateUpdate()
    {
#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_EDITOR
        StatusUpdate();
#endif
    }

#if !UNITY_ANDROID && !UNITY_IOS && !UNITY_EDITOR
    private void StatusUpdate()
    {
        // Calculate the elapsed time since the song started in seconds
        double elapsedSongTimeInSeconds = (AudioSettings.dspTime - songStartTime) * TempoManager.instance.audioSource.pitch;

        // Convert GameManager's duration from seconds to seconds
        long songDurationInSeconds = (long)Math.Round(GameManager.Instance.SongDuration);

        // Current playback position in seconds
        long songPlaybackPositionInSeconds = (long)elapsedSongTimeInSeconds;

        // Calculate the end timestamp
        long currentTimeInSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
        long endTimeInSeconds = currentTimeInSeconds + (songDurationInSeconds - songPlaybackPositionInSeconds);

        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = state + " Song: " + GameManager.Instance.SongName,
                State = details + GameManager.Instance.score + " ~ " + GameManager.Instance.accuracy.ToString("F2") + "% ~ " + GameManager.Instance._playerRating.ToString(),
                Assets = {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
                Timestamps = {
                    End = endTimeInSeconds
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok)
                    Debug.LogWarning("Failed to update Discord activity: " + res);
            });
        }
        catch (Exception ex)
        {
            Debug.LogError("Discord update failed: " + ex.Message);
            Destroy(gameObject);
        }
    }
#endif

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartSong();
    }
}
