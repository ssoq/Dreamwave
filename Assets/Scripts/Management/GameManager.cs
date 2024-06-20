using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum Focus
{
    LeftPlayer,
    RightPlayer,
    Centre
}

public enum Ratings
{
    MFC,
    PFC,
    GFC,
    FC,
    SDCB,
    S,
    A,
    B,
    C,
    D,
    F
}

public enum PlayerCount
{
    One,
    Two
}

public class GameManager : MonoBehaviour
{
    [Header("Instancing")]
    [SerializeField] public static GameManager Instance;

    [Header("Scripts")]
    [SerializeField] public ScrollManager scrollManager;
    [SerializeField] private FanfareEvent _fanfareEventScript;

    [Header("States")]
    [SerializeField] public Focus focus;
    [SerializeField] public bool canSongStart = false;
    [SerializeField] public bool canGhostTap = true;
    [SerializeField] public bool canFreeAnimate = true;
    [SerializeField] public bool shouldDisplayIncomingNoteWarning = true;
    [SerializeField] public bool shouldAutoPause = true;
    [SerializeField] public bool shouldDrawNoteSplashes = true;

    [Header("Keybinds")]
    [SerializeField] public KeyCode left;
    [SerializeField] public KeyCode down;
    [SerializeField] public KeyCode up;
    [SerializeField] public KeyCode right;

    [Header("Game")]
    [SerializeField] public int score = 0;
    [SerializeField] public int combo = 0;
    [SerializeField] public int misses = 0;
    [SerializeField] public float accuracy = 100;
    [SerializeField] public Ratings _playerRating;
    [SerializeField] public int[] scores;
    [SerializeField] public int totalNotes;
    [SerializeField] public int totalNotesHitCorrect;

    [Space(10)]
    [SerializeField] public int shits = 0;
    [SerializeField] public int bads = 0;
    [SerializeField] public int cools = 0;
    [SerializeField] public int sicks = 0;
    [SerializeField] public int dreamys = 0;
    [SerializeField] public int maxScore;

    [Header("Settings")]
    [SerializeField] public int SongStartStep = 0;
    [SerializeField] private Vector3 leftPlayerSideFocus;
    [SerializeField] private Vector3 rightPlayerSideFocus;
    [SerializeField] private Vector3 centreFocus;
    [SerializeField] private Vector3[] mobileFocus;
    [SerializeField] private string[] _unfocusedMessages;

    [Header("DiscordRPC Settings")]
    [SerializeField] public string SongName;
    [SerializeField] public string SongCreatorName;
    [SerializeField] public string SongImageName;
    [SerializeField] public float SongDuration;
    [SerializeField] public float SongPlaybackPosition;

    [Header("GameObjects")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject _gameUnfocusedUi;
    [SerializeField] private GameObject noteUi;
    [SerializeField] public GameObject noteUiSidePlayer;
    [SerializeField] public GameObject noteUiSidePlayerMiddle;
    [SerializeField] public GameObject noteUiSideOpponent;
    [SerializeField] public GameObject noteUiSideOpponentMiddle;
    [SerializeField] private GameObject noteRendererPlayerMobile;
    [SerializeField] private GameObject noteRendererPlayer;
    [SerializeField] private GameObject noteRendererOpponent;

    [Header("Shaders")]
    [SerializeField] private Material psychShader;
    [SerializeField] private float psychShaderSmoothing = 5f;
    [SerializeField] private float noteUiSmoothing = 2f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _gameUnfocusedText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private TextMeshProUGUI missesText;
    [SerializeField] private TextMeshProUGUI ratingText;
    [SerializeField] private TextMeshProUGUI accuracyText;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private TextMeshProUGUI ffpsText;
    [SerializeField] private TextMeshProUGUI bpmText;
    [SerializeField] private TextMeshProUGUI stepText;
    [SerializeField] private TextMeshProUGUI overallStepText;

    [Header("Prefabs")]
    [SerializeField] public GameObject[] ratings;

    private void Awake()
    {
        Instance = this;
        FanfareStart();
    }

    public void FanfareStart()
    {
        canSongStart = false;
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void FanfareEnd()
    {
        canSongStart = true;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void Start()
    {
        psychShader.SetFloat("_Scale", -2.8f);
        bpmText.text = "Bpm - " + TempoManager.instance.beatsPerMinute.ToString();
#if !UNITY_EDITOR
        DiscordController.instance.largeImage = SongImageName;
#endif
        SongPlaybackPosition = (float)AudioSettings.dspTime;
        SongDuration = TempoManager.instance.audioSource.clip.length;
        
        var allNotes = GameObject.FindGameObjectsWithTag("Note");
        for (int i = 0; i < allNotes.Length; i++)
        {
            totalNotes++;
        }

        accuracy = 100.00f;
        CalculatePlayerRating();
        ConfigureDeviceSettings();
    }

    private void ConfigureDeviceSettings()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                noteUiSideOpponent.SetActive(false);
                noteUiSidePlayer.SetActive(false);
                noteRendererOpponent.SetActive(false);
                noteRendererPlayerMobile.SetActive(true);
                break;
            case RuntimePlatform.WindowsPlayer:
                noteUiSideOpponent.SetActive(true);
                noteRendererPlayerMobile.SetActive(false);
                break;
            default:
                noteUiSideOpponent.SetActive(true);
                noteRendererPlayerMobile.SetActive(false);
                break;
        }
    }

    private void Update()
    {
        fpsText.text = "Fps - " + (1.0f / Time.unscaledDeltaTime).ToString("F0");
        ffpsText.text = "FFps - " + PlayerPrefs.GetFloat("ffps").ToString("F0");

        if (PauseMenu.instance._isPaused) return;
        AnimateBackground();
        PositionNoteUi();
    }

    private void OnEnable()
    {
        NoteHitbox.NoteHit += OnNoteHit;
        TempoManager.OnStep += OnStepHandler;
    }

    private void OnDisable()
    {
        NoteHitbox.NoteHit -= OnNoteHit;
        TempoManager.OnStep -= OnStepHandler;
    }

    private void OnDestroy()
    {
        NoteHitbox.NoteHit -= OnNoteHit;
        TempoManager.OnStep -= OnStepHandler;
    }

    private void OnNoteHit(string newScore, float msDelay, float noteDistance)
    {
        switch (newScore) 
        {
            case "Shit":
                score -= scores[4];
                var shit = Instantiate(ratings[4], ratings[3].transform.position, Quaternion.identity);
                shit.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MS: "+msDelay.ToString("F4");
                shit.transform.SetParent(canvas.transform);
                shit.transform.localScale = new Vector3(0.1338828f, 0.1338828f, 0.1338828f);
                combo = 0;
                misses++;
                comboText.text = "Combo: " + combo.ToString();
                missesText.text = "Misses: " + misses.ToString();
                shits++;
                accuracy -= 0.12f + (noteDistance / 3);
                break;
            case "Bad":
                score -= scores[3];
                var bad = Instantiate(ratings[3], ratings[2].transform.position, Quaternion.identity);
                bad.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MS: "+msDelay.ToString("F4");
                bad.transform.SetParent(canvas.transform);
                bad.transform.localScale = new Vector3(0.1338828f, 0.1338828f, 0.1338828f);
                combo++;
                bads++;
                comboText.text = "Combo: " + combo.ToString();
                missesText.text = "Misses: " + misses.ToString();
                accuracy -= 0.1f + (noteDistance / 2);
                break;
            case "Cool":
                score += scores[2] - ((int)msDelay / 2);
                var good = Instantiate(ratings[2], ratings[1].transform.position, Quaternion.identity);
                good.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MS: "+msDelay.ToString("F4");
                good.transform.SetParent(canvas.transform);
                good.transform.localScale = new Vector3(0.1338828f, 0.1338828f, 0.1338828f);
                combo++;
                cools++;
                totalNotesHitCorrect++;
                comboText.text = "Combo: " + combo.ToString();
                accuracy += 0.05f - (noteDistance / 4);
                break;
            case "Sick":
                score += scores[1] - ((int)msDelay / 3);
                var sick = Instantiate(ratings[1], ratings[0].transform.position, Quaternion.identity);
                sick.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MS: "+msDelay.ToString("F4");
                sick.transform.SetParent(canvas.transform);
                sick.transform.localScale = new Vector3(0.1338828f, 0.1338828f, 0.1338828f);
                combo++;
                sicks++;
                totalNotesHitCorrect++;
                comboText.text = "Combo: " + combo.ToString();
                accuracy += 0.1f - (noteDistance / 3);
                break;
            case "Dreamy":
                score += scores[0] - ((int)msDelay / 4);
                var dreamy = Instantiate(ratings[0], ratings[0].transform.position, Quaternion.identity);
                dreamy.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MS: " + msDelay.ToString("F4");
                dreamy.transform.SetParent(canvas.transform);
                dreamy.transform.localScale = new Vector3(0.1338828f, 0.1338828f, 0.1338828f);
                combo++;
                dreamys++;
                totalNotesHitCorrect++;
                comboText.text = "Combo: " + combo.ToString();
                accuracy += 0.12f - (noteDistance / 2);
                break;
        }

        accuracy = Mathf.Clamp(accuracy, 0f, 100f);
        accuracyText.text = "Accuracy: " + accuracy.ToString("F0") + "%";
        score = Mathf.Clamp(score, 0, 999999999);
        scoreText.text = "Score: " + score.ToString();
        CalculatePlayerRating();
        //Debug.Log(score + " - " + newScore);
    }

    public void CalculatePlayerRating() // PLEASE FIX SOON - FIXED IT MEOW MEOW!!! 20/06/2024
    {
        if (misses == 0)
        {
            if (shits == 0 && bads == 0 && cools == 0 && sicks == 0 && dreamys >= 0)
            {
                _playerRating = Ratings.MFC;
            }
            else if (shits == 0 && bads == 0 && cools == 0 && sicks > 0 && dreamys >= 0)
            {
                _playerRating = Ratings.PFC;
            }
            else if (shits == 0 && bads == 0 && cools > 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.GFC;
            }
            else if (shits == 0 && bads > 0 && cools >= 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.FC;
            }
        }
        else
        {
            if (misses <= 9 && accuracy >= -1 && shits >= 0 && bads >= 0 && cools >= 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.SDCB;
            }
            else if (misses >= 10 && accuracy >= 90 && shits >= 0 && bads >= 0 && cools >= 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.S;
            }
            else if (misses >= 10 && accuracy >= 80 && shits >= 0 && bads >= 0 && cools >= 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.A;
            }
            else if (misses >= 10 && accuracy >= 70 && shits >= 0 && bads >= 0 && cools >= 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.B;
            }
            else if (misses >= 10 && accuracy >= 60 && shits >= 0 && bads >= 0 && cools >= 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.C;
            }
            else if (misses >= 10 && accuracy >= 50 && shits >= 0 && bads >= 0 && cools >= 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.D;
            }
            else if (misses >= 10 && accuracy >= 40 && shits >= 0 && bads >= 0 && cools >= 0 && sicks >= 0 && dreamys >= 0)
            {
                _playerRating = Ratings.F;
            }
        }

        ratingText.text = "Rating: " + _playerRating.ToString();
    }

    private void PositionNoteUi()
    {
        //noteUi.transform.localPosition = mobileFocus[0];

        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                noteUi.transform.localPosition = mobileFocus[0];
                break;
            case RuntimePlatform.WindowsPlayer:
                switch (focus)
                {
                    case Focus.LeftPlayer:
                        noteUi.transform.localPosition = Vector3.Lerp(noteUi.transform.localPosition, leftPlayerSideFocus, noteUiSmoothing * Time.deltaTime);
                        break;
                    case Focus.RightPlayer:
                        noteUi.transform.localPosition = Vector3.Lerp(noteUi.transform.localPosition, rightPlayerSideFocus, noteUiSmoothing * Time.deltaTime);
                        break;
                    case Focus.Centre:
                        noteUi.transform.localPosition = Vector3.Lerp(noteUi.transform.localPosition, centreFocus, noteUiSmoothing * Time.deltaTime);
                        break;
                }
                break;
            default:
                switch (focus)
                {
                    case Focus.LeftPlayer:
                        noteUi.transform.localPosition = Vector3.Lerp(noteUi.transform.localPosition, leftPlayerSideFocus, noteUiSmoothing * Time.deltaTime);
                        break;
                    case Focus.RightPlayer:
                        noteUi.transform.localPosition = Vector3.Lerp(noteUi.transform.localPosition, rightPlayerSideFocus, noteUiSmoothing * Time.deltaTime);
                        break;
                    case Focus.Centre:
                        noteUi.transform.localPosition = Vector3.Lerp(noteUi.transform.localPosition, centreFocus, noteUiSmoothing * Time.deltaTime);
                        break;
                }
                break;
        }
    }

    private void AnimateBackground()
    {
        switch (onBeat)
        {
            case true:
                psychShader.SetFloat("_WarpScale", Mathf.Lerp(psychShader.GetFloat("_WarpScale"), -20f, psychShaderSmoothing * Time.deltaTime));
                //psychShader.SetFloat("_RotationAngle", Mathf.Lerp(psychShader.GetFloat("_RotationAngle"), -10f * Mathf.Deg2Rad, psychShaderSmoothing * Time.deltaTime));
                break;
            case false:
                psychShader.SetFloat("_WarpScale", Mathf.Lerp(psychShader.GetFloat("_WarpScale"), 20f, psychShaderSmoothing * Time.deltaTime));
                //psychShader.SetFloat("_RotationAngle", Mathf.Lerp(psychShader.GetFloat("_RotationAngle"), 10f * Mathf.Deg2Rad, psychShaderSmoothing * Time.deltaTime));
                break;
        }
    }

    private bool onBeat = false;
    public bool start = false;
    public int stepCount = 0;
    private void OnStepHandler(int step) 
    {
        stepCount++;
        //Debug.Log(stepCount);
        if (stepCount >= SongStartStep) start = true;

        switch (onBeat)
        {
            case true:
                if (step == 2) onBeat = false;
                break;
            case false:
                if (step == 2) onBeat = true;
                break;
        }

        stepText.text = "Step - " + step.ToString();
        overallStepText.text = "Steps - " + stepCount.ToString();
    }

    private void OnApplicationFocus(bool focus)
    {
        if (shouldAutoPause && !PauseMenu.instance._isPaused) StartCoroutine(OnUnfocus(focus));
    }

    private IEnumerator OnUnfocus(bool focus)
    {
        yield return new WaitUntil(() => !_fanfareEventScript.InFanfare);

        switch (focus)
        {
            case false:
                Time.timeScale = 0;
                AudioListener.pause = true;
                _gameUnfocusedText.text = _unfocusedMessages[Random.Range(0, _unfocusedMessages.Length)];
                _gameUnfocusedUi.SetActive(true);
                break;
            case true:
                Time.timeScale = 1;
                AudioListener.pause = false;
                _gameUnfocusedUi.SetActive(false);
                _fanfareEventScript.StopSong();
                break;
        }

        yield break;
    }

    public void ResetSettings()
    {
        Time.fixedDeltaTime = 1f / 60f;
        PlayerPrefs.SetInt("_preferredFixedFrameRate", 60);
    }
}
