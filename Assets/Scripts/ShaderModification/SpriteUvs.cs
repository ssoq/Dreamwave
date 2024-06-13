using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteUvs : MonoBehaviour
{
    public RawImage rawImage; // Assign this in the Inspector or via code
    public Sprite sprite; // The sprite you want to display

    void Start()
    {
        // Get the sprite's texture and apply it to the RawImage
        rawImage.texture = sprite.texture;

        // Calculate UV scale and offset
        Vector2 uvScale = new Vector2(sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height);
        Vector2 uvOffset = new Vector2(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height);

        // Apply UV scale and offset to the shader
        rawImage.material.SetFloat("_UVScaleX", uvScale.x);
        rawImage.material.SetFloat("_UVScaleY", uvScale.y);
        rawImage.material.SetFloat("_UVOffsetX", uvOffset.x);
        rawImage.material.SetFloat("_UVOffsetY", uvOffset.y);
    }

}
