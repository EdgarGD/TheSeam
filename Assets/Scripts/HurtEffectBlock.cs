using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEffectBlock : MonoBehaviour
{
    public int DamageAmount = 1;

    public float TimeToActivateBlock = 1.5f;
    private float _timeLeft;

    private Vector3 _blockOriginalScale;
    private Vector3 _blockOriginalPosition;

    private bool _someoneEnteredBlockOnce;
    private bool _someoneOnBlock;
    private bool _blockIsActive;

    private void Start()
    {
        _blockOriginalScale = transform.localScale;
        _blockOriginalPosition = transform.localPosition;

        _blockIsActive = true;
    }

    private void FixedUpdate()
    {
        if (_someoneEnteredBlockOnce && !_someoneOnBlock)
        {
            _timeLeft -= Time.deltaTime;
            ActivateTheBlock();
        }
    }

    private void OnTriggerEnter2D(Collider2D obj)
    {
        _someoneOnBlock = true;
        if (obj.gameObject.CompareTag("Player") && _blockIsActive)
        {
            var playerBehaviour = obj.GetComponent<PlayerBehaviour>();
            StartCoroutine(playerBehaviour.ReceiveDamage(DamageAmount));

            DisactivateTheBlock();
        }

        if (obj.gameObject.CompareTag("Enemy") && _blockIsActive)
        {
            //var enemyBasicAI = obj.GetComponent<EnemyBasicAI>();
            //StartCoroutine(enemyBasicAI.ReceiveDamage(DamageAmount));

           DisactivateTheBlock();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _someoneOnBlock = false;
    }

    private void DisactivateTheBlock()
    {
        _someoneEnteredBlockOnce = true;
        _blockIsActive = false;
        _timeLeft = TimeToActivateBlock;

        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);

        var offset = transform.localScale.y / 2;
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - offset, transform.localPosition.z);
    }

    private void ActivateTheBlock()
    {
        if (_timeLeft <= 0)
        {
          _blockIsActive = true;
          transform.localScale = _blockOriginalScale;
          transform.localPosition = _blockOriginalPosition;
        }
    }
  
}
