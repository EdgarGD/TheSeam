using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileInput : MonoBehaviour
{
    private PlayerBehaviour main;

    private float MInput;
    private bool left;
    private bool right;
    private KeyboardInput kInput;

    private void Start()
    {
        main = GetComponent<PlayerBehaviour>();
    }

    private void Update()
    {
        if (left)
        {
            MInput = -1;
        }
        if (left==false && right==false)
        {
            MInput = 0;
        }
        if (right)
        {
            MInput = 1;
        }
        main.MInput = MInput;
        if (main.Acc)
        {
            main.AccelerationPower = Mathf.Lerp(main.AccelerationPower, main.Speed, main.AccelerationTime * Time.deltaTime);
        }
        else
        {
            main.AccelerationPower = Mathf.Lerp(main.AccelerationPower, 0f, main.DecelerationTime * Time.deltaTime);
        }
    }

    public void LeftDown()
    {
        left = true;
        main.Acc = true;
        main.RunDir = main.MInput;
    }
    public void LeftUp()
    {
        left = false;
        main.Acc = false;
        main.RunDir = main.MInput;
    }

    public void RightDown()
    {
        right = true;
        main.Acc = true;
        main.RunDir = main.MInput;
    }

    public void RightUp()
    {
        right = false;
        main.Acc = false;
        main.RunDir = main.MInput;
    }

    public void Jump()
    {
        main.Jump();
    }

    public void Attack()
    {
        if (main.Anim.GetBool("Attack") == false)
        {
            main.DetectEnemy();
        }
    }

    public void SwitchUp()//Переход между платформами
    {
        if (main.IsGrounded && !main.IsOnSky)
        {
            main.Rigidbody.velocity = Vector2.up * main.PlatformJump * 3.5f;

            if (main.CurrentPlatform != null)
            {
                main.CurrentPlatform.enabled = true;
            }
            Debug.Log("Up");
        }
    }
    public void SwitchDown()//Переход между платформами
    {
        if (main.IsGrounded && main.IsOnSky)
        {
            if (main.CurrentPlatform != null)
            {
                main.CurrentPlatform.enabled = false;
            }
            Debug.Log("Doun");
        }
    }

}
