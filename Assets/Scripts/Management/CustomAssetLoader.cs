using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Custom asset loader.
/// 
/// This script searches through the games mod index for
/// a text document which contains the state of custom assets.
/// </summary>

public enum TypePlayerOne
{
    Custom,
    Mod,
    Default //fallback
}

public enum TypePlayerTwo
{
    Custom,
    Mod,
    Default //fallback
}

public enum TypeNoteAsset
{
    Custom,
    Mod,
    Default //fallback
}

public enum TypeNoteParticleAsset
{
    Custom,
    Mod,
    Default //fallback
}

public class CustomAssetLoader : MonoBehaviour
{
    public TypePlayerOne _typePlayerOne;
    public TypePlayerTwo _typePlayerTwo;
    public TypeNoteAsset _typeNoteAsset;
    public TypeNoteParticleAsset _typeNoteParticleAsset;

    #region Note Particle Renderer Section

    [Space(10)]
    [Header("Note Particle Settings")]
    public string CustomNoteParticleName;
    public int _particleSpriteWidth = 1080;
    public int _particleSpriteHeight = 1080;
    [Header("Note Particle Renderers")]
    [SerializeField] private List<SpriteRenderer> _noteParticleSpriteRenderers;

    #endregion

    #region Note Section

    [Space(10)]
    [Header("Note Renderers")]
    public string CustomNoteFileLocation;
    public string CustomNoteHeldFileLocation;
    public int _NoteWidth = 470;
    public int _NoteHeight = 540;
    public Color[] _noteColors;
    [SerializeField] private List<SpriteRenderer> _noteSpriteRenderers;
    [SerializeField] private NoteController _noteController;

    #endregion

    #region Player One Section

    [Space(10)]
    [Header("Player One Animation Settings")]
    public string CustomPlayerOneFileName;
    public int _playerOneSpriteWidth = 1080;
    public int _playerOneSpriteHeight = 1080;
    public float _playerOneRectX = 1f;
    public float _playerOneRectY = 1f;
    public bool _shouldFlipPlayerOneCustom = true;
    [SerializeField] private DreamwaveCharacter _playerOneScript;

    [Space(5)]
    public string CustomPlayerOneIconFileName;
    public int _playerOneIconWidth = 1080;
    public int _playerOneIconHeight = 1080;
    public float _playerOneIconRectX = 1f;
    public float _playerOneIconRectY = 1f;
    [SerializeField] private DreamwaveIcon _playerOneIconScript;

    #endregion

    #region Player Two Section - AI

    [Space(10)]
    [Header("Player Two Animation Settings")]
    public string CustomAiPlayerTwoFileName;
    public int _playerTwoAiSpriteWidth = 1080;
    public int _playerTwoAiSpriteHeight = 1080;
    public float _playerTwoRectX = 0.5f;
    public float _playerTwoRectY = 0.5f;
    public bool _shouldFlipPlayerTwoCustom = false;
    [SerializeField] private DreamwaveAICharacter _playerTwoAiScript;

    [Space(5)]
    public string CustomPlayerTwoIconFileName;
    public int _playerTwoIconWidth = 1080;
    public int _playerTwoIconHeight = 1080;
    public float _playerTwoIconRectX = 1f;
    public float _playerTwoIconRectY = 1f;
    [SerializeField] private DreamwaveIcon _playerTwoIconScript;

    #endregion

    private void Awake()
    {
        GatherNeededObjects();
    }

    private void GatherNeededObjects()
    {
        #region Notes

        GameObject[] _notes = GameObject.FindGameObjectsWithTag("Note");
        GameObject[] _enemyNotes = GameObject.FindGameObjectsWithTag("EnemyNote");
        GameObject[] _holdNoteParents = GameObject.FindGameObjectsWithTag("Note Hold Parent");
        GameObject[] _noteControllers = GameObject.FindGameObjectsWithTag("Note Object");
        for (int i = 0; i < _notes.Length; i++)
        {
            SpriteRenderer spriteRenderer = _notes[i].GetComponent<SpriteRenderer>();
            _noteSpriteRenderers.Add(spriteRenderer);
        }
        for (int i = 0; i < _enemyNotes.Length; i++)
        {
            SpriteRenderer enemySpriteRenderer = _enemyNotes[i].GetComponent<SpriteRenderer>();
            _noteSpriteRenderers.Add(enemySpriteRenderer);
        }
        for (int i = 0; i < _holdNoteParents.Length; i++)
        {
            SpriteRenderer holdNoteParenSpriteRenderer = _holdNoteParents[i].GetComponent<SpriteRenderer>();
            _noteSpriteRenderers.Add(holdNoteParenSpriteRenderer);
        }
        for (int i = 0; i < _noteControllers.Length; i++)
        {
            SpriteRenderer controllerSpriteRenderer = _noteControllers[i].GetComponent<SpriteRenderer>();
            _noteSpriteRenderers.Add(controllerSpriteRenderer);
        }
        #endregion

        #region Note Particles
        GameObject[] _noteParticles = GameObject.FindGameObjectsWithTag("Note Particle");
        for (int i = 0; i < _noteParticles.Length; i++)
        {
            SpriteRenderer spriteRenderer = _noteParticles[i].GetComponent<SpriteRenderer>();
            _noteParticleSpriteRenderers.Add(spriteRenderer);
        }
        #endregion

        LoadCustomAssets(); // calls after for-loops
    }

    private void LoadCustomAssets()
    {
        switch (_typeNoteParticleAsset)
        {
            case TypeNoteParticleAsset.Custom:
                for (int i = 0; i < _noteParticleSpriteRenderers.Count; i++)
                {
                    _noteParticleSpriteRenderers[i].sprite = LoadStreamedSprite($"" +
                        $"{Application.streamingAssetsPath + "/Sprites/UI/Game/NoteParticles/"}", CustomNoteParticleName + ".png", _particleSpriteWidth, _particleSpriteHeight);
                }
                break;
            case TypeNoteParticleAsset.Mod:
                break;
            case TypeNoteParticleAsset.Default:
                break;
        }

        // I need to get the int of how many sprites are available in each file and then apply them to each note controllers sprite lists respectively
        switch (_typeNoteAsset)
        {
            case TypeNoteAsset.Custom:
                _noteController.noteSpritesDown.Clear();
                _noteController.noteSpritesRelease.Clear();

                string filePathHeld = Path.Combine($"{Application.streamingAssetsPath}/{CustomNoteHeldFileLocation}/Held Sprites/");
                string filePathReleased = Path.Combine($"{Application.streamingAssetsPath}/{CustomNoteFileLocation}/Release Sprites/");

                _noteController.noteSpritesDown.AddRange(LoadSpritesFromPath(filePathHeld, _NoteWidth, _NoteHeight, 0.5f, 0.5f));
                _noteController.noteSpritesRelease.AddRange(LoadSpritesFromPath(filePathReleased, _NoteWidth, _NoteHeight, 0.5f, 0.5f));

                for (int i = 0; i < _noteSpriteRenderers.Count; i++)
                {
                    _noteSpriteRenderers[i].sprite = _noteController.noteSpritesRelease[_noteController.noteSpritesRelease.Count - 1];
                }
                break;
        }

        switch (_typePlayerOne)
        {
            case TypePlayerOne.Custom:
                // Clear all animations and offsets
                _playerOneScript.IdleAnimation.Clear();
                _playerOneScript.IdleOffsets.Clear();

                _playerOneScript.LeftAnimations.Clear();
                _playerOneScript.LeftOffsets.Clear();

                _playerOneScript.DownAnimations.Clear();
                _playerOneScript.DownOffsets.Clear();

                _playerOneScript.UpAnimations.Clear();
                _playerOneScript.UpOffsets.Clear();

                _playerOneScript.RightAnimations.Clear();
                _playerOneScript.RightOffsets.Clear();

                _playerOneIconScript._critical.Clear();
                _playerOneIconScript._criticalOffsets.Clear();

                _playerOneIconScript._losing.Clear();
                _playerOneIconScript._losingOffsets.Clear();

                _playerOneIconScript._neutral.Clear();
                _playerOneIconScript._neutralOffsets.Clear();

                _playerOneIconScript._winning.Clear();
                _playerOneIconScript._winningOffsets.Clear();

                if (_shouldFlipPlayerOneCustom) _playerOneScript.Renderer.flipX = true;
                else _playerOneScript.Renderer.flipX = false;

                // Define animation names
                string[] animationNames = { "Idle", "Left", "Down", "Up", "Right" };
                string[] iconAnimationNames = { "Winning", "Neutral", "Losing", "Critical" };

                // load custom character sprites and offsets for animations
                foreach (string animationName in animationNames)
                {
                    string animationPath = Path.Combine($"{Application.streamingAssetsPath}/{CustomPlayerOneFileName}/{animationName}/");
                    string offsetPath = Path.Combine($"{Application.streamingAssetsPath}/{CustomPlayerOneFileName}/{animationName}/offsets.txt");

                    var sprites = LoadSpritesFromPath(animationPath, _playerOneSpriteWidth, _playerOneSpriteHeight, _playerOneRectX, _playerOneRectY);
                    var offsets = LoadSpriteOffsetsFromPath(offsetPath);

                    switch (animationName)
                    {
                        case "Idle":
                            _playerOneScript.IdleAnimation.AddRange(sprites);
                            _playerOneScript.IdleOffsets.AddRange(offsets);
                            break;
                        case "Left":
                            _playerOneScript.LeftAnimations.AddRange(sprites);
                            _playerOneScript.LeftOffsets.AddRange(offsets);
                            break;
                        case "Down":
                            _playerOneScript.DownAnimations.AddRange(sprites);
                            _playerOneScript.DownOffsets.AddRange(offsets);
                            break;
                        case "Up":
                            _playerOneScript.UpAnimations.AddRange(sprites);
                            _playerOneScript.UpOffsets.AddRange(offsets);
                            break;
                        case "Right":
                            _playerOneScript.RightAnimations.AddRange(sprites);
                            _playerOneScript.RightOffsets.AddRange(offsets);
                            break;
                    }
                }

                // load custom icon sprites and offsets for animations
                foreach (string animationName in iconAnimationNames)
                {
                    string animationPath = Path.Combine($"{Application.streamingAssetsPath}/{CustomPlayerOneIconFileName}/{animationName}");
                    string offsetPath = Path.Combine($"{Application.streamingAssetsPath}/{CustomPlayerOneIconFileName}/{animationName}/offsets.txt");

                    var sprites = LoadSpritesFromPath(animationPath, _playerOneIconWidth, _playerOneIconHeight, _playerOneIconRectX, _playerOneIconRectY);
                    var offsets = LoadSpriteOffsetsFromPath(offsetPath);

                    switch (animationName)
                    {
                        case "Winning":
                            _playerOneIconScript._winning.AddRange(sprites);
                            _playerOneIconScript._winningOffsets.AddRange(offsets);
                            break;
                        case "Neutral":
                            _playerOneIconScript._neutral.AddRange(sprites);
                            _playerOneIconScript._neutralOffsets.AddRange(offsets);
                            break;
                        case "Losing":
                            _playerOneIconScript._losing.AddRange(sprites);
                            _playerOneIconScript._losingOffsets.AddRange(offsets);
                            break;
                        case "Critical":
                            _playerOneIconScript._critical.AddRange(sprites);
                            _playerOneIconScript._criticalOffsets.AddRange(offsets);
                            break;
                    }
                }

                _playerOneScript.Renderer.sprite = _playerOneScript.IdleAnimation[0];
                _playerOneScript.Renderer.transform.localPosition = _playerOneScript.IdleOffsets[0];

                break;
        }

        switch (_typePlayerTwo)
        {
            case TypePlayerTwo.Custom:
                // Clear all animations and offsets
                #region Clearing Defualts

                _playerTwoAiScript.IdleAnimation.Clear();
                _playerTwoAiScript.IdleOffsets.Clear();

                _playerTwoAiScript.LeftAnimations.Clear();
                _playerTwoAiScript.LeftOffsets.Clear();

                _playerTwoAiScript.DownAnimations.Clear();
                _playerTwoAiScript.DownOffsets.Clear();

                _playerTwoAiScript.UpAnimations.Clear();
                _playerTwoAiScript.UpOffsets.Clear();

                _playerTwoAiScript.RightAnimations.Clear();
                _playerTwoAiScript.RightOffsets.Clear();

                _playerTwoIconScript._critical.Clear();
                _playerTwoIconScript._criticalOffsets.Clear();

                _playerTwoIconScript._losing.Clear();
                _playerTwoIconScript._losingOffsets.Clear();

                _playerTwoIconScript._neutral.Clear();
                _playerTwoIconScript._neutralOffsets.Clear();

                _playerTwoIconScript._winning.Clear();
                _playerTwoIconScript._winningOffsets.Clear();

                #endregion

                if (_shouldFlipPlayerTwoCustom) _playerTwoAiScript.Renderer.flipX = true;
                else _playerTwoAiScript.Renderer.flipX = false;

                // Define animation names
                string[] animationNames = { "Idle", "Left", "Down", "Up", "Right" };
                string[] iconAnimationNames = { "Winning", "Neutral", "Losing", "Critical" };

                // Iterate over animation names
                foreach (string animationName in animationNames)
                {
                    string animationPath = Path.Combine($"{Application.streamingAssetsPath}/{CustomAiPlayerTwoFileName}/{animationName}/");
                    string offsetPath = Path.Combine($"{Application.streamingAssetsPath}/{CustomAiPlayerTwoFileName}/{animationName}/offsets.txt");

                    var sprites = LoadSpritesFromPath(animationPath, _playerTwoAiSpriteWidth, _playerTwoAiSpriteHeight, _playerTwoRectX, _playerTwoRectY);
                    var offsets = LoadSpriteOffsetsFromPath(offsetPath);

                    switch (animationName)
                    {
                        case "Idle":
                            _playerTwoAiScript.IdleAnimation.AddRange(sprites);
                            _playerTwoAiScript.IdleOffsets.AddRange(offsets);
                            break;
                        case "Left":
                            _playerTwoAiScript.LeftAnimations.AddRange(sprites);
                            _playerTwoAiScript.LeftOffsets.AddRange(offsets);
                            break;
                        case "Down":
                            _playerTwoAiScript.DownAnimations.AddRange(sprites);
                            _playerTwoAiScript.DownOffsets.AddRange(offsets);
                            break;
                        case "Up":
                            _playerTwoAiScript.UpAnimations.AddRange(sprites);
                            _playerTwoAiScript.UpOffsets.AddRange(offsets);
                            break;
                        case "Right":
                            _playerTwoAiScript.RightAnimations.AddRange(sprites);
                            _playerTwoAiScript.RightOffsets.AddRange(offsets);
                            break;
                    }
                }

                // load custom icon sprites and offsets for animations
                foreach (string animationName in iconAnimationNames)
                {
                    string animationPath = Path.Combine($"{Application.streamingAssetsPath}/{CustomPlayerTwoIconFileName}/{animationName}");
                    string offsetPath = Path.Combine($"{Application.streamingAssetsPath}/{CustomPlayerTwoIconFileName}/{animationName}/offsets.txt");

                    var sprites = LoadSpritesFromPath(animationPath, _playerTwoIconWidth, _playerTwoIconHeight, _playerTwoIconRectX, _playerTwoRectY);
                    var offsets = LoadSpriteOffsetsFromPath(offsetPath);

                    switch (animationName)
                    {
                        case "Winning":
                            _playerTwoIconScript._winning.AddRange(sprites);
                            _playerTwoIconScript._winningOffsets.AddRange(offsets);
                            break;
                        case "Neutral":
                            _playerTwoIconScript._neutral.AddRange(sprites);
                            _playerTwoIconScript._neutralOffsets.AddRange(offsets);
                            break;
                        case "Losing":
                            _playerTwoIconScript._losing.AddRange(sprites);
                            _playerTwoIconScript._losingOffsets.AddRange(offsets);
                            break;
                        case "Critical":
                            _playerTwoIconScript._critical.AddRange(sprites);
                            _playerTwoIconScript._criticalOffsets.AddRange(offsets);
                            break;
                    }
                }

                _playerTwoAiScript.Renderer.sprite = _playerTwoAiScript.IdleAnimation[0];
                _playerTwoAiScript.Renderer.transform.localPosition = _playerTwoAiScript.IdleOffsets[0];

                break;
        }
    }

    public Sprite LoadStreamedSprite(string filepath, string fileName, int width, int height)
    {
        string filePath = Path.Combine(filepath, fileName);

        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);

            Texture2D texture = new(width, height, TextureFormat.ARGB32, false);
            texture.LoadImage(fileData);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            return sprite;
        }
        else
        {
            Debug.LogError("Could not find any file with the given path " + filePath);
            return null;
        }
    }

    public List<Sprite> LoadSpritesFromPath(string filePath, int width, int height, float rectX, float rectY)
    {
        List<Sprite> sprites = new();

        if (Directory.Exists(filePath))
        {
            // Get all files in the directory that match the pattern *.png
            string[] imageFiles = Directory.GetFiles(filePath, "*.png");

            foreach (string file in imageFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (int.TryParse(fileName, out int index))
                {
                    // Load the image file into a Texture2D
                    byte[] fileData = File.ReadAllBytes(file);
                    Texture2D texture = new Texture2D(width, height);
                    if (texture.LoadImage(fileData))
                    {
                        // Convert the Texture2D to a Sprite
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(rectX, rectY));

                        // Add the Sprite to the list
                        sprites.Add(sprite);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Directory does not exist: " + filePath);
        }

        return sprites;
    }

    public List<Vector2> LoadSpriteOffsetsFromPath(string filePath)
    {
        List<Vector2> offsets = new();

        if (File.Exists(filePath))
        {
            // Read all lines from the offsets.txt file
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                // Split the line by comma to get x and y values
                string[] values = line.Split(',');

                if (values.Length == 2 &&
                    float.TryParse(values[0].Trim(), out float x) &&
                    float.TryParse(values[1].Trim(), out float y))
                {
                    // Create a new Vector2 with the parsed x and y values
                    Vector2 offset = new Vector2(x, y);
                    // Add the Vector2 to the list of offsets
                    offsets.Add(offset);
                }
                else
                {
                    Debug.LogWarning("Invalid line in file: " + filePath + " Line: " + line);
                }
            }
        }
        else
        {
            Debug.LogError("File does not exist: " + filePath);
        }

        return offsets;
    }
} 
