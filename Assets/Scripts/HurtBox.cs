using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public float TimeBetweenAttack = .2f;  
    public float TimeTillAttack;

    private Animator _anim;
    //private EnemyBasicAI _enemy;
    //private KnockBack _knockBack;


    void Start()
    {
        //_knockBack = GetComponent<KnockBack>();
        //_enemy = transform.parent.GetComponent<EnemyBasicAI>();
        //_anim = _enemy.Anim;

        if(_anim == null)
        {
            Debug.Log("Enemy doesn't have an Animator!");
        }
    }

    void Update()
    {
  
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (TimeTillAttack <= 0)
            {
                var target = other.GetComponent<Rigidbody2D>();

                StartCoroutine(AttackThePlayer(target));

                TimeTillAttack = TimeBetweenAttack;
            }
            TimeTillAttack -= Time.deltaTime;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TimeTillAttack = TimeBetweenAttack;
        //_enemy.EnemyState = EnemyBasicAI.EnemyStates.Running;
    }

    private IEnumerator AttackThePlayer(Rigidbody2D player)
    {
        //_enemy.EnemyState = EnemyBasicAI.EnemyStates.Attacking;
        _anim.SetBool("isAttacking", true);
        _anim.SetBool("isRunningEnemy", false);

        yield return null;

        //var amount = transform.parent.GetComponent<EnemyBasicAI>().Attack;
        var playerBehaviour = player.GetComponent<PlayerBehaviour>();

        //StartCoroutine(playerBehaviour.ReceiveDamage(amount));

        yield return new WaitForSeconds(.6f);

        _anim.SetBool("isAttacking", false);
      //  yield return new WaitForSeconds(.3f);    // время, которое занимает проигрыш получения удара у игрока

        yield return null;
        //_enemy.EnemyState = EnemyBasicAI.EnemyStates.Idling;
    }
}
