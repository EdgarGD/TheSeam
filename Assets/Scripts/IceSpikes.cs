using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpikes : MonoBehaviour
{
    public float TimeBetweenAttack = .02f;
    public float TimeTillAttack;
    public int DamageAmount = 1;
    //public KnockBack _knockBack;
    public bool IsOnSpike = false;
    private bool hitBoole;

    void Start()
    {
        TimeTillAttack = 0;
        hitBoole = false;
    }

    void Update()
    {
    //    if (IsOnSpike)
    //    {
    //        TimeTillAttack -= Time.deltaTime;

    //        if (TimeTillAttack < 0)
    //        {
    //            TimeTillAttack = 0;
    //        }
    //    }

    //    if (TimeTillAttack <= 0 && IsOnSpike)
    //    {
    //        HurtTarget();
    //    }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Rabbit Player")
        {
            if (transform.parent.CompareTag("obstacle"))
            transform.parent.GetComponent<BoxCollider2D>().enabled = false;
            HurtTarget();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player"))
        {
            TimeTillAttack = 0;
        }   
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsOnSpike = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        IsOnSpike = false;
        TimeTillAttack = TimeBetweenAttack;
    }

    private void HurtTarget()
    {
        if (hitBoole == false)
        {
            var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
            //StartCoroutine(player.ReceiveDamage(DamageAmount));
            player.Hit(DamageAmount);

            TimeTillAttack = TimeBetweenAttack;
            hitBoole = true; //флаг, чтобы не было двойного удара при отскоке
        }             
    }
}
