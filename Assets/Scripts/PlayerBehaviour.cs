using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Player Attributes")]
    public int Health = 5;    // значение здоровья игрока не менять!
    public int Attack = 1;
    public float Speed = 4;
    public Transform SightDistance;    // поле зрения игрока
    //public Transform Platform;
    [Range(1, 10)]
    public float JumpingVelocity;
    public float PlatformJump = 10;
    public bool DoubleJump = false;
    public bool WasHit = false;
    [HideInInspector] public bool IsAlive = true;
    [HideInInspector] public bool IsOnSky;

    [Header("Enemy Settings")]
    [Range(0, 1)] public float DamageTime;

    [Header("Input Settings")]
    public bool KeyboardInput = false;          //Управление с клавиатуры
    [HideInInspector] public float MInput;                        //Движение персонажа

    [Header("Audio settings")]
    public AudioSource ASourсe;
    private AudioSource  audioSourсeC;
    public AudioClip[] FootstepsSounds;
    public AudioClip[] AttackSounds;
    public AudioClip[] JumpSounds;
    public AudioClip[] HitSounds;
    [HideInInspector] public float JumpVelosThreshold = 2.5f;

    [Header("Physics")]
    [Range(1, 1.3f)] public float FallAccelerationValue = 1.055f;
    [HideInInspector] public bool Acc;
    [HideInInspector] public float AccelerationPower;
    [Range(1, 6)] public float AccelerationTime = 6f;
    [Range(1, 6)] public float DecelerationTime = 6f;
    [HideInInspector] public float RunDir;

    [Header("Ground/Layers")]
    public Transform Feet;
    public float FeetRadius;
    public LayerMask Groundlayer;
    public bool WasGrounded = true;
    public bool IsGrounded = false;
    public GameObject ParticleEffect;

    [HideInInspector] public Animator Anim;
    [HideInInspector] public int JumpsNum;
    [HideInInspector] public Rigidbody2D Rigidbody;
    
    [Header("PlayerStates")]
    public PlayerStates State = PlayerStates.Idling;
    public enum PlayerStates { Idling, Jumping, Falling, ReceivingDamage, Attacking, Walking, Dying };

    [Header("Other")]
    [HideInInspector] public List<GameObject> GameObjectsinView = new List<GameObject>();
    [HideInInspector] public Collider2D CurrentPlatform;
    
    private Collider2D playerCollider;

    private float _scale;
    private Vector2 _currentPosition;
    private Vector2 _endPosition;

    private KnockBack _knockBack;     // экземпляр класса KnockBack, который отталкивает противника
    public KeyboardInput KeyboardInputC;

    void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        Anim = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Rigidbody.gravityScale = 2;
        _scale = transform.localScale.x;

        _knockBack = GetComponent<KnockBack>();
        ASourсe = GetComponent<AudioSource>();

        audioSourсeC = GetComponentInChildren<AudioSource>();

            KeyboardInputC = GetComponent<KeyboardInput>();
    }

    private void Start()
    {
        string Scname = SceneManager.GetActiveScene().name; //Это вынести в отдельный метод
        for (int i =1; i <= 5; i++)//с какой по какую сцену должна быть скорость одинаковой
        {
            if (Scname == "Level" + i)
            {
                Speed = 8;
            }
        }
        for (int i = 6; i <= 8; i++)//с какой по какую сцену должна быть скорость одинаковой
        {
            if (Scname == "Level" + i)
            {
                Speed =10;
            }
        }
    }

    void Update()
    {   
        if (Health <= 0)
        {
            IsAlive = false;
        }
        if ((WasGrounded && !IsGrounded || !WasGrounded && IsGrounded) && ParticleEffect !=null)
        {
            Instantiate(ParticleEffect, Feet.position - new Vector3(0, 0.5f, 0), ParticleEffect.transform.rotation);
            WasGrounded = IsGrounded;
        }
        
        GetPlayerStates();               // при мерже - оставить эту строку

        if (State != PlayerStates.ReceivingDamage)
        {
            if (KeyboardInput)
            {
                KeyboardInputC.KeyboardWalkAndAttack();
            }
            else
            {
                Walk();
            }
        }

        AnimationController();
    }

    public void GetPlayerStates()
    {
        if (!IsAlive)
        {
            State = PlayerStates.Dying;
            GetComponent<Collider2D>().enabled = false;
        }
        if (Anim.GetBool("ReceiveDamage"))
        {
            State = PlayerStates.ReceivingDamage;
        }
        if (Anim.GetBool("Attack"))
        {
            State = PlayerStates.Attacking;
        }
        if (!Anim.GetBool("IsGrounded"))
        {
            if (Anim.GetFloat("JumpVeloc") > 0.1f)
            {
                State = PlayerStates.Jumping;
            }
            else if (Anim.GetFloat("JumpVeloc") < 0f)
            {
                State = PlayerStates.Falling;
            }
        }
        if (Anim.GetBool("IsGrounded") && Anim.GetFloat("Speed") > 0.01f)
        {
            State = PlayerStates.Walking;
        }
        else if (Anim.GetBool("IsGrounded") && Anim.GetFloat("Speed") < 0.01f && !Anim.GetBool("Attack") && !Anim.GetBool("ReceiveDamage"))
        {
            State = PlayerStates.Idling;
        }
    }

    public void Hit(int takenDamage)
    {
        if (!WasHit)
        {
            Health -= takenDamage;
            StartCoroutine(MakeInvincible(3f));
        }
    }

    public IEnumerator MakeInvincible(float t)
    {
        WasHit = true;
        Debug.Log("I am invincible!");
        yield return new WaitForSeconds(t);
        WasHit = false;
        Debug.Log("I am not:(!");
    }
    
    public void AddingLife()
    {
        Health += 1;
    }

    public IEnumerator ReceiveDamage(int takenDamage)
    {
        Anim.SetBool("ReceiveDamage", true);
        Health -= takenDamage;
        transform.GetComponent<Renderer>().material.color = Color.red;
        //Handheld.Vibrate();                              //Вибрация

        yield return null;

        yield return new WaitForSeconds(.3f);            // Knockback доделать

        yield return null;

        Anim.SetBool("ReceiveDamage", false);
        transform.GetComponent<Renderer>().material.color = Color.white;
    }

    public void Walk()
    {
        if (Acc)
        {
            Rigidbody.velocity = new Vector2(MInput * AccelerationPower, Rigidbody.velocity.y);
        }
        else
        {
            Rigidbody.velocity = new Vector2(RunDir * AccelerationPower, Rigidbody.velocity.y);
        }
        
        IsGrounded = Physics2D.OverlapCircle(Feet.position, FeetRadius, Groundlayer);
    }

    public void Jump()    // прыжок для мобильных устройств, вызывается по нажатию кнопки в MobileInput
    {
        if (!DoubleJump)
        {
            if (IsGrounded && State != PlayerStates.ReceivingDamage)
            {
                Rigidbody.velocity = Vector2.up * JumpingVelocity * 3;
            }
            if (Rigidbody.velocity.y < 0) //Ускорение падения
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, Rigidbody.velocity.y * FallAccelerationValue);
            }
        }
        else
        {
            if (JumpsNum < 1)
            {
                ++JumpsNum;
                Rigidbody.velocity = (Vector2.up * JumpingVelocity) + new Vector2(Rigidbody.velocity.x, 0);
            }
            else if (IsGrounded && JumpsNum > 0)
            {
                JumpsNum = 0;
            }
        }
    }

    public void DetectEnemy()              // определяем, что враг находится в поле зрения игрока
    {
        _currentPosition = new Vector2(transform.position.x, SightDistance.position.y);
        _endPosition = new Vector2(SightDistance.position.x, SightDistance.position.y);
        var hits = Physics2D.LinecastAll(_currentPosition, _endPosition);
       
        foreach (var obj in hits)
        {
            var target = obj.collider.gameObject;

            if(State!= PlayerStates.Attacking && State!= PlayerStates.Walking && State!=PlayerStates.ReceivingDamage)
            {
                if (target.CompareTag("Enemy"))   // игрок увидел противника
                {
                        StartCoroutine(AttackTheEnemy(target));           // атаковать противника

                    _knockBack.HitSomeObject(target);
                }
                else if (target.CompareTag("Chest"))
                {
                    StartCoroutine(OpenChest(target));
                }
                else
                {
                    StartCoroutine(AttackTheEnemy(null));    // если игрок не видит врага - просто влючить анимацию взамаха меча
                }
            }
        }
    }

    private IEnumerator AttackTheEnemy(GameObject enemy)
    {
        if (Anim.GetBool("Attack") == false)
        Anim.SetBool("Attack", true);

        yield return null;
        yield return new WaitForSeconds(GetComponent<Animation>().clip.length*DamageTime);
        
        if (enemy != null )     // если врага нет - в методе просто проигрывается анимация взмаха меча
        {
            var enemyBasicAI = enemy.GetComponent<EnemyBasicAI>();
            var enemyBasicBeh = enemy.GetComponent<BasicBehavior>();
            if (enemyBasicAI != null)
            StartCoroutine(enemyBasicAI.ReceiveDamage(Attack));
            else
            StartCoroutine(enemyBasicBeh.ReceiveDamage(Attack));
        }
        

        yield return new WaitForSeconds(GetComponent<Animation>().clip.length);

        Anim.SetBool("Attack", false);
    }

    private IEnumerator OpenChest(GameObject target)
    {
        ParametersOfGeneration pof = target.GetComponent<ParametersOfGeneration>();
        GameObject artefactPrefub = target.transform.Find("Artefact").gameObject;
        pof.ArtefactCreated(artefactPrefub, target);
        target.transform.GetComponent<Renderer>().material.color = Color.gray;
        yield return null;
    }
    public void AnimationController()
    {
        Anim.SetFloat("Speed", Mathf.Abs(MInput));
        Anim.SetFloat("JumpVeloc", Rigidbody.velocity.y);
        if (State == PlayerStates.Walking) 
        {
            Anim.speed = Rigidbody.velocity.x / 8; //Изменение скорости анимации бега в зависимости от скорости персонажа.
        }

        if (MInput < 0)
        {
            transform.localScale = new Vector3(-_scale, transform.localScale.y, transform.localScale.z);
        }
        else if (MInput > 0)
        {
            transform.localScale = new Vector3(_scale, transform.localScale.y, transform.localScale.z);
        }

        Anim.SetBool("IsGrounded", IsGrounded);

        if(GameManager.Gm.GameState == GameManager.GameStates.BeatLevel)
        {
            Anim.SetBool("LevelCompleted", true);
        }
    }

    public void PlayAudioClipEvent()
    {
        if (Anim.GetBool("Attack") && AttackSounds.Length > 0)
        {
            ASourсe.PlayOneShot(AttackSounds[Random.Range(0, AttackSounds.Length)]);
        }
        if (Anim.GetFloat("Speed") >= 0.01f && IsGrounded == true && FootstepsSounds.Length > 0)
        {
            if (!audioSourсeC.isPlaying)
            {
                audioSourсeC.PlayOneShot(FootstepsSounds[Random.Range(0, FootstepsSounds.Length)]);
            }
        }
        if (Anim.GetFloat("JumpVeloc") > JumpVelosThreshold && JumpSounds.Length>0)
        {
            ASourсe.PlayOneShot(JumpSounds[Random.Range(0, JumpSounds.Length)]);
        }
        if (Anim.GetBool("ReceiveDamage") && HitSounds.Length > 0)
        {
            ASourсe.PlayOneShot(HitSounds[Random.Range(0, HitSounds.Length)]);
        }
    }

    void OnDrawGizmosSelected()      // показывает поле зрения игрока
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(Feet.position, FeetRadius);
    }

   
}
