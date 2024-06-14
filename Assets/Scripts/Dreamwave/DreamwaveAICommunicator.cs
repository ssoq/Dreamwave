using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
**                                         
**  Dreamverse AI Communicator
**
**  Name    :   DreamverseAICommunicator.cs
**  Author  :   Lewis-Lee
** 
** 
**  Desc
**      A custom character animator for 2d chracters. Allows for easy
**      modding in-game without having to use the Unity Engine whatsoever.
**      Also better for this type of project than Unity's Animator Component.
**      This is for AI as the second player when playing solo.
**
*******************************************************************************/

public class DreamwaveAICommunicator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private WhichSide _whichSide;

    [Header("Scripts")]
    [SerializeField] private DreamwaveAICharacter _character;

    [Header("GameObjects")]
    [SerializeField] private GameObject _noteParticle;

    [Header("Components")]
    [SerializeField] public SpriteRenderer _spriteRenderer;
    [SerializeField] public Sprite[] _noteSprites;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyNote"))
        {
            switch (_whichSide)
            {
                case WhichSide.Left:
                    _character.StopAllCoroutines();
                    _character.StartCoroutine(_character.SingAnimation("Left"));
                    var pl = Instantiate(_noteParticle, _noteParticle.transform.position, Quaternion.identity);
                    pl.SetActive(true);
                    break;
                case WhichSide.Down:
                    _character.StopAllCoroutines();
                    _character.StartCoroutine(_character.SingAnimation("Down"));
                    var pd = Instantiate(_noteParticle, _noteParticle.transform.position, Quaternion.identity);
                    pd.SetActive(true);
                    break;
                case WhichSide.Up:
                    _character.StopAllCoroutines();
                    _character.StartCoroutine(_character.SingAnimation("Up"));
                    var pu = Instantiate(_noteParticle, _noteParticle.transform.position, Quaternion.identity);
                    pu.SetActive(true);
                    break;
                case WhichSide.Right:
                    _character.StopAllCoroutines();
                    _character.StartCoroutine(_character.SingAnimation("Right"));
                    var pr = Instantiate(_noteParticle, _noteParticle.transform.position, Quaternion.identity);
                    pr.SetActive(true);
                    break;
            }

            collision.gameObject.SetActive(false);
        }
    }
}
