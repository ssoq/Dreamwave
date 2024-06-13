using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteCuller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note")) { collision.gameObject.GetComponent<SpriteRenderer>().enabled = true; }
        if (collision.gameObject.CompareTag("EnemyNote")) { collision.gameObject.GetComponent<SpriteRenderer>().enabled = true; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Note")) { collision.gameObject.SetActive(false); }
        if (collision.gameObject.CompareTag("EnemyNote")) { collision.gameObject.SetActive(false); }
    }
}
