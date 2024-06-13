using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Custom asset loader.
/// 
/// This script searches through the games mod index for
/// a text document which contains the state of custom assets.
/// </summary>

public enum CustomPlayerOne
{
    Custom,
    Mod,
    Default //fallback
}

public enum CustomPlayerTwo
{
    Custom,
    Mod,
    Default //fallback
}

public enum CustomNoteAsset
{
    Custom,
    Mod,
    Default //fallback
}

public class CustomAssetLoader : MonoBehaviour
{
    
}
