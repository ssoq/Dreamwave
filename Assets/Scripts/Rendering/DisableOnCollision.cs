using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnCollision : MonoBehaviour
{
    [SerializeField] private WhichSide _whichSide;
    [SerializeField] private string _colName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_colName))
        {
            if (_whichSide == WhichSide.Left && Input.GetKey(GameManager.Instance.left))
            {
                gameObject.SetActive(false);
            }

            if (_whichSide == WhichSide.Down && Input.GetKey(GameManager.Instance.down))
            {
                gameObject.SetActive(false);
            }

            if (_whichSide == WhichSide.Up && Input.GetKey(GameManager.Instance.up))
            {
                gameObject.SetActive(false);
            }

            if (_whichSide == WhichSide.Right && Input.GetKey(GameManager.Instance.right))
            {
                gameObject.SetActive(false);
            }
        }
    }
}
