using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [Header("Keyboard Input Settings")]
    public KeyCode JumpButton = KeyCode.Space;
    public KeyCode AttackButton = KeyCode.E;

    private PlayerBehaviour _player;

    enum kJumpStage { None, Track, Levitate }
    kJumpStage JumpStage;
    float LevitateTimer;
    float LevitateTime = 1f;
    Vector2 beginPosPlatform;
    Vector2 currentPosPlatform;
    public bool useMagnetTEST = true;
    public Camera mainCamera;
    public bool InternalRunning = true;

    public Transform Platform;
    

    void Start()
    {
        _player = GetComponent<PlayerBehaviour>();

        beginPosPlatform = Platform.localPosition;
    }

    void Update()
    {

        currentPosPlatform = Platform.localPosition;
        if (beginPosPlatform != currentPosPlatform)
        {
            Platform.localPosition = beginPosPlatform; //Debug.Log("Произошло смещение." + currentPosPlatform);
        }

        if (!InternalRunning)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                _player.RunDir = _player.MInput;
            }
        }

        if (_player.Acc)
        {
            _player.AccelerationPower = Mathf.Lerp(_player.AccelerationPower, _player.Speed, _player.AccelerationTime * Time.deltaTime);
        }
        else
        {
            _player.AccelerationPower = Mathf.Lerp(_player.AccelerationPower, 0f, _player.DecelerationTime * Time.deltaTime);
        }

        switch (JumpStage)
        {
            case kJumpStage.None:
                {
                    Platform.gameObject.SetActive(false);
                }
                break;
            case kJumpStage.Track:
                {
                    if (_player.Rigidbody.velocity.y < 0)
                    {
                        JumpStage = kJumpStage.Levitate;
                    }
                }
                break;
            case kJumpStage.Levitate:
                {
                    LevitateTimer += Time.deltaTime;
                    if (LevitateTimer > LevitateTime) JumpStage = kJumpStage.None;
                    _player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, 0);
                    Platform.gameObject.SetActive(true);

                    _player.Anim.SetBool("isGrounded", true);

                }
                break;
        }
        SwitchPlatform(); //Переход между платформами
    }

    public void KeyboardWalkAndAttack()
    {
        if (!InternalRunning)
            _player.MInput = Input.GetAxisRaw("Horizontal");
        else
        {
            _player.MInput = 1;
            _player.Acc = true;
        }
        if (Input.GetKeyDown(AttackButton))      // атаковать enemy
        {
            if (_player.Anim.GetBool("Attack") == false)
            {
                Debug.Log("Pressing E");
                _player.DetectEnemy();
            }
        }

        if (useMagnetTEST)
        {
            findObjects();
        }
        if (!InternalRunning)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                _player.Acc = true;
            }
            else
           if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                _player.Acc = false;
                _player.Speed = 0f;
            }
        }
        if (_player.Acc)
        {
            _player.Rigidbody.velocity = new Vector2(_player.MInput * _player.Speed, _player.Rigidbody.velocity.y);
        }
        else
        {
            _player.Rigidbody.velocity = new Vector2(_player.RunDir * _player.Speed, _player.Rigidbody.velocity.y);
        }
        _player.IsGrounded = Physics2D.OverlapCircle(_player.Feet.position, _player.FeetRadius, _player.Groundlayer);

        KeyboardJump();

    }

    public void findObjects()
    {
        foreach (var a in _player.GameObjectsinView)
        {
            MoveObjectToPlayer(a.gameObject);
        }
    }

    public void MoveObjectToPlayer(GameObject target)
    {
        var PickUp = target.GetComponent<PickUps>();
        PickUp.flyToTarget = true;

    }

    public void KeyboardJump()
    {
        if (Input.GetKeyDown(JumpButton) && _player.JumpsNum < 1)
        {
            _player.Jump();
            _player.JumpsNum++;
        }
        if (_player.Rigidbody.velocity.y < 0)            //Ускорение падения
        {
            _player.Rigidbody.velocity = new Vector2(_player.Rigidbody.velocity.x, _player.Rigidbody.velocity.y * _player.FallAccelerationValue);
        }
        if (_player.IsGrounded)
        {
            _player.JumpsNum = 0;
        }
    }
    public void SwitchPlatform()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchUp();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwitchDown();
        }
    }

    public void SwitchUp()
    {
        if (_player.IsGrounded && !_player.IsOnSky)
        {
            _player.Rigidbody.velocity = Vector2.up * _player.PlatformJump * 3.5f;

            if (_player.CurrentPlatform != null)
            {
                _player.CurrentPlatform.enabled = true;
            }
            Debug.Log("Up");
        }
    }
    public void SwitchDown()
    {
        if (_player.IsGrounded && _player.IsOnSky)
        {
            if (_player.CurrentPlatform != null)
            {
                _player.CurrentPlatform.enabled = false;
            }
            Debug.Log("Doun");
        }
    }
}
    

    
  

