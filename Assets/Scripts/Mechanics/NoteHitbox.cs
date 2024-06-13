using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
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
    public delegate void HitNote(string scoreType, float msDelay, float noteDistance);
    public static event HitNote NoteHit;
    public KeyCode keyForSide;
    public string buttonForSide;
    public GameObject NoteHitParticle;

    private Stopwatch stopwatch;

    private void Start()
    {
        switch (side) 
        {
            case Side.Left:
                keyForSide = GameManager.Instance.left;
                buttonForSide = "Left";
                break;
            case Side.Down:
                keyForSide = GameManager.Instance.down;
                buttonForSide = "Down";
                break;
            case Side.Up:
                keyForSide = GameManager.Instance.up;
                buttonForSide = "Up";
                break;
            case Side.Right:
                keyForSide = GameManager.Instance.right;
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
        if (PauseMenu.instance._isPaused) return;
        if (notesWithinHitBox.Count <= 0) return;

        var fn = notesWithinHitBox[0].gameObject;
        var dist = Vector2.Distance(centre.transform.position, fn.transform.position);

        if (Input.GetKeyDown(keyForSide) | MobileControls.instance.GetButtonsPressed(buttonForSide))
        {
            stopwatch.Stop();
            delayInMs = (float)stopwatch.Elapsed.TotalMilliseconds;

            if (dist >= ratingThresholds[4])
            {
                NoteHit("Shit", delayInMs, dist);
            }
            else if (dist >= ratingThresholds[3])
            {
                NoteHit("Bad", delayInMs, dist);
            }
            else if (dist >= ratingThresholds[2])
            {
                NoteHit("Cool", delayInMs, dist);
                var nhc = Instantiate(NoteHitParticle, NoteHitParticle.transform.position, Quaternion.identity);
                nhc.SetActive(true);
            }
            else if (dist >= ratingThresholds[1])
            {
                NoteHit("Sick", delayInMs, dist);
                var nhs = Instantiate(NoteHitParticle, NoteHitParticle.transform.position, Quaternion.identity);
                nhs.SetActive(true);
            }
            else if (dist >= ratingThresholds[0])
            {
                NoteHit("Dreamy", delayInMs, dist);
                var nhd = Instantiate(NoteHitParticle, NoteHitParticle.transform.position, Quaternion.identity);
                nhd.SetActive(true);
            }
            else if (dist <= ratingThresholds[0])
            {
                NoteHit("Shit", delayInMs, dist);
            }

            stopwatch.Reset();
            fn.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            notesWithinHitBox.Add(collision.gameObject);
            stopwatch.Restart();
        }

        if (collision.gameObject.CompareTag("Note Hold"))
        {
            notesWithinHitBox.Add(collision.gameObject);
            stopwatch.Restart();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note"))
        {
            notesWithinHitBox.Remove(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Note Hold"))
        {
            notesWithinHitBox.Remove(collision.gameObject);
        }
    }
}
