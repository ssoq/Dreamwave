using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    #region Note Renderer Section

    public string CustomNoteParticleName;
    public int _particleSpriteWidth = 1080;
    public int _particleSpriteHeight = 1080;
    [Header("Note Particle Renderers")]
    [SerializeField] private List<SpriteRenderer> _noteParticleSpriteRenderer;

    #endregion

    private void Awake()
    {
        GatherNeededObjects();
    }

    private void GatherNeededObjects()
    {
        GameObject[] _noteParticles = GameObject.FindGameObjectsWithTag("Note Particle");
        for (int i = 0; i < _noteParticles.Length; i++)
        {
            SpriteRenderer spriteRenderer = _noteParticles[i].GetComponent<SpriteRenderer>();
            _noteParticleSpriteRenderer.Add(spriteRenderer);
        }

        LoadCustomAssets(); // calls after for-loops
    }

    private void LoadCustomAssets()
    {
        switch (_typeNoteParticleAsset)
        {
            case TypeNoteParticleAsset.Custom:
                for (int i = 0; i < _noteParticleSpriteRenderer.Count; i++)
                {
                    _noteParticleSpriteRenderer[i].sprite = LoadStreamedSprite($"" +
                        $"{Application.streamingAssetsPath + "/Sprites/UI/Game/NoteParticles/"}", CustomNoteParticleName + ".png",
                        _particleSpriteWidth, _particleSpriteHeight);
                }
                break;
            case TypeNoteParticleAsset.Mod:
                break;
            case TypeNoteParticleAsset.Default:
                break;
        }
    }

    public Sprite LoadStreamedSprite(string filepath, string fileName, int width, int height)
    {
        string filePath = Path.Combine(filepath, fileName);

        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);

            Texture2D texture = new (width, height, TextureFormat.ARGB32, false);
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
}
