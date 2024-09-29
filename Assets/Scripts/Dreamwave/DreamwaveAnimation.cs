using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamwaveAnimation : MonoBehaviour
{
    protected bool _complete = false;

    protected void PlayAnimation(SpriteRenderer renderer, List<Sprite> sprites, List<Vector2> offsets, float timeToFlick)
    {
        StopCoroutine(RunAnimation(renderer, sprites, offsets, timeToFlick));
        StartCoroutine(RunAnimation(renderer, sprites, offsets, timeToFlick));
    }

    private IEnumerator RunAnimation(SpriteRenderer renderer, List<Sprite> sprites, List<Vector2> offsets, float timeToFlick)
    {
        if (sprites == null || sprites.Count == 0 || (offsets != null && offsets.Count != sprites.Count))
        {
            Debug.LogError("Sprite list is empty, or offsets do not match the number of sprites.");
            yield break;
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            if (sprites.Count > 0) renderer.sprite = sprites[i];

            if (offsets != null && offsets.Count > i)
                renderer.transform.localPosition = offsets[i];

            yield return new WaitForSecondsRealtime(timeToFlick);

            if (i == sprites.Count - 1)
            {
                _complete = true;
                yield break;
            }
        }
    }
}
