/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBoxBasic : MonoBehaviour
{
    BasicBehavior bb;
    Vector2 _currentPosition;
    Vector2 _endPosition;
    public float TimeBetweenAttack = 1.5f;  
    public float TimeTillAttack    = 0.0f;
    
    void Awake()
    {
        bb = transform.parent.gameObject.GetComponent(typeof (BasicBehavior)) as BasicBehavior;
        
    }

   
    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player"))    
        {
            if (TimeTillAttack <= 0)
            {
                StartCoroutine(bb.Attack(other.gameObject));
                TimeTillAttack = TimeBetweenAttack;
            }
            TimeTillAttack -= Time.deltaTime;
        }
    }
   
     private void OnTriggerExit2D(Collider2D other) {
     if (other.CompareTag("Player"))    
        {
            TimeTillAttack = TimeBetweenAttack;
            bb.Idle();
        }    
     }
    
}
*/