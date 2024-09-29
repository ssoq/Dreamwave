using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamwaveAnimation : MonoBehaviour
{
    protected void PlayAnimation(SpriteRenderer renderer, List<Sprite> sprites, List<Vector2> offsets, float timeToFlick)
    {
        StopCoroutine(RunAnimation(renderer, sprites, offsets, timeToFlick));
        StartCoroutine(RunAnimation(renderer, sprites, offsets, timeToFlick));
    }

    private IEnumerator RunAnimation(SpriteRenderer renderer, List<Sprite> sprites, List<Vector2> offsets, float timeToFlick)
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            if (sprites.Count != 0) renderer.sprite = sprites[i];
            if (offsets.Count != 0) renderer.transform.localPosition = offsets[i];

            yield return new WaitForSecondsRealtime(timeToFlick);

            if (i == sprites.Count - 1) yield break;
        }
    }
}
