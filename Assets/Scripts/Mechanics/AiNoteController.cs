using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THIS IS OLD AND NO LONGER USED

public enum WhichSide
{
    Left,
    Up,
    Down,
    Right
}

public class AiNoteController : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Animator aiAnim;

    [Header("Animtion Settings")]
    [SerializeField] private float spriteCrossfadeSpeed = 0f;
    [SerializeField] private float threeDCrossfadeSpeed = 0.25f;

    public WhichSide whichSide;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyNote"))
        {
            switch (whichSide)
            {
                case WhichSide.Left:
                    aiAnim.Play("LeftHold"); aiAnim.SetBool("Holding", false);
                    break;
                case WhichSide.Down:
                    aiAnim.Play("RightHold"); aiAnim.SetBool("Holding", false);
                    break;
                case WhichSide.Up:
                    aiAnim.Play("UpHold"); aiAnim.SetBool("Holding", false);
                    break;
                case WhichSide.Right:
                    aiAnim.Play("DownHold"); aiAnim.SetBool("Holding", false);
                    break;
            }

            collision.gameObject.SetActive(false);
        }

        if (aiAnim == null) return;
    }
}
