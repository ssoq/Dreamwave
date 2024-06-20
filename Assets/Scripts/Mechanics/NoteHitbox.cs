using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum Side
{
    Left,
    Down,
    Up,
    Right
}

public class NoteHitbox : MonoBehaviour
{
    public Side side;
    public List<GameObject> notesWithinHitBox = new List<GameObject>();
    public GameObject centre;
    public float delayInMs;
    public float[] ratingThresholds;
    public delegate void HitNoteHandler(string scoreType, float msDelay, float noteDistance);
    public static event HitNoteHandler NoteHit;
    public KeyCode keyForSide;
    public string buttonForSide;
    public GameObject NoteHitParticle;

    private Stopwatch stopwatch;

    private void Start()
    {
        switch (side)
        {
            case Side.Left:
                buttonForSide = "Left";
                break;
            case Side.Down:
                buttonForSide = "Down";
                break;
            case Side.Up:
                buttonForSide = "Up";
                break;
            case Side.Right:
                buttonForSide = "Right";
                break;
        }

        stopwatch = new Stopwatch();
    }

    void Update()
    {
        NoteInput();
    }

    private void NoteInput()
    {
        if (PauseMenu.instance != null && PauseMenu.instance._isPaused) return;
        if (notesWithinHitBox.Count <= 0 || centre == null || ratingThresholds == null || ratingThresholds.Length < 5) return;

        var fn = notesWithinHitBox[0];
        if (fn == null) return;

        var dist = Vector2.Distance(centre.transform.position, fn.transform.position);

        if (Input.GetKeyDown(keyForSide) || (MobileControls.instance != null && MobileControls.instance.GetButtonsPressed(buttonForSide)))
        {
            stopwatch.Stop();
            delayInMs = (float)stopwatch.Elapsed.TotalMilliseconds;

            string scoreType = DetermineScoreType(dist);
            if (NoteHit != null)
                NoteHit(scoreType, delayInMs, dist);

            CreateNoteHitParticle(scoreType);

            stopwatch.Reset();

            if (fn.CompareTag("Note"))
            {
                fn.SetActive(false);
            }
            else if (fn.CompareTag("Note Hold Parent"))
            {
                var spriteRenderer = fn.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                    spriteRenderer.enabled = false;
            }
        }
    }

    private string DetermineScoreType(float distance)
    {
        if (distance >= ratingThresholds[4])
            return "Shit";
        if (distance >= ratingThresholds[3])
            return "Bad";
        if (distance >= ratingThresholds[2])
            return "Cool";
        if (distance >= ratingThresholds[1])
            return "Sick";
        return distance >= ratingThresholds[0] ? "Dreamy" : "Shit";
    }

    private void CreateNoteHitParticle(string scoreType)
    {
        if (GameManager.Instance != null && GameManager.Instance.shouldDrawNoteSplashes && NoteHitParticle != null)
        {
            var nh = Instantiate(NoteHitParticle, NoteHitParticle.transform.position, Quaternion.identity);
            nh.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null || collision.gameObject == null) return;

        if (collision.gameObject.CompareTag("Note") || collision.gameObject.CompareTag("Note Hold Parent") || collision.gameObject.CompareTag("Note Hold"))
        {
            notesWithinHitBox.Add(collision.gameObject);
            stopwatch.Restart();
        }

        if (collision.gameObject.CompareTag("Note Hold") && Input.GetKey(keyForSide))
        {
            NoteHit?.Invoke("Dreamy", delayInMs, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null || collision.gameObject == null) return;

        if (collision.gameObject.CompareTag("Note"))
        {
            HandleNoteExit(collision);
        }
        else if (collision.gameObject.CompareTag("Note Hold Parent"))
        {
            HandleNoteHoldParentExit(collision);
        }
        else if (collision.gameObject.CompareTag("Note Hold"))
        {
            HandleNoteHoldExit(collision);
        }
    }

    private void HandleNoteExit(Collider2D collision)
    {
        if (!Input.GetKeyDown(keyForSide))
        {
            NoteHit?.Invoke("Shit", delayInMs, 2);
        }

        notesWithinHitBox.Remove(collision.gameObject);
    }

    private void HandleNoteHoldParentExit(Collider2D collision)
    {
        if (!Input.GetKey(keyForSide))
        {
            NoteHit?.Invoke("Shit", delayInMs, 2);
        }

        notesWithinHitBox.Remove(collision.gameObject);
        var spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;
    }

    private void HandleNoteHoldExit(Collider2D collision)
    {
        if (!Input.GetKey(keyForSide))
        {
            NoteHit?.Invoke("Shit", delayInMs, 2);
        }

        notesWithinHitBox.Remove(collision.gameObject);
    }
}
