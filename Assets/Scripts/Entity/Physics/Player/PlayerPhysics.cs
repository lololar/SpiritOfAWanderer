using UnityEngine;
using System.Collections;
using System;

public class PlayerManager
{
    public enum GameMode
    {
        MENU,
        PAUSE,
        PLAYING,
        SPIRITING,
        END,
    }

    private static PlayerManager Instance;

    public static PlayerManager GetInstance
    {
        get
        {
            if (Instance == null)
            {
                Awake();
            }
            return Instance;
        }
    }

    static void Awake()
    {
        Instance = new PlayerManager();
        Instance._player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerPhysics>();
        Instance._spirit = GameObject.FindGameObjectWithTag("Spirit").GetComponent<Spirit>();
        Instance._endScreen = GameObject.FindGameObjectWithTag("End");
        Instance._endScreen.SetActive(false);
        Instance._menuScreen = GameObject.FindGameObjectWithTag("Menu");
    }

    public void Play()
    {
        _menuScreen.SetActive(false);
        _mode = GameMode.SPIRITING;
}

    public void End()
    {
        _endScreen.SetActive(true);
        _mode = GameMode.END;
    }

    public PlayerPhysics _player;
    public Spirit _spirit;
    public GameObject _menuScreen;
    public GameObject _endScreen;
    public GameMode _mode = GameMode.MENU;

    public void Change(GameMode game)
    {
        _mode = game;
    }

}

public class PlayerPhysics : Moving {

    public float _timeBetInputHandle = 0.1f;
    public float _jumpPower = 5.0f;
    public float _speed = 5.0f;
    public float _maxSpeed;

    public Animator animator;
    public SpriteRenderer _renderer;
    ParticleSystem _landing;

    public override void Start()
    {
        animator = GetComponent<Animator>();
        _landing = transform.FindChild("Landing").GetComponent<ParticleSystem>();
        _renderer = GetComponent<SpriteRenderer>();
        StartCoroutine(InputHandler());
        base.Start();
    }

    IEnumerator InputHandler()
    {
        while(_isAlive)
        {
            if (PlayerManager.GetInstance._mode == PlayerManager.GameMode.PLAYING)
            {
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                bool isJumping = Input.GetButton("Jump");
                bool isActivatingDoor = Input.GetButtonDown("Door");

                if (isJumping && !_isFalling)
                {
                    Jump();
                }
                if (isActivatingDoor && Mathf.Abs(h) < 0.3f)
                {
                    DoorManager.GetInstance.WarpToLinkedDoor(gameObject);
                }
                if (Mathf.Abs(h) > 0.3f)
                {
                    if (!_isFalling)
                    {
                        animator.Play("Walk");
                        animator.SetBool("Walk", true);
                        if (h > 0)
                        {
                            _renderer.flipX = false;
                        }
                        else
                        {
                            _renderer.flipX = true;
                        }
                    }
                    Move(h);
                }
                else
                {
                    animator.SetBool("Walk", false);
                }
            }
            else
            {
                animator.SetBool("Walk", false);
            }
            yield return new WaitForSeconds(_timeBetInputHandle);
        }
    }

    protected override void MoveAction()
    {
        base.MoveAction();

        if (Mathf.Abs(rigid.velocity.x) > _maxSpeed)
        {
            rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x, -_maxSpeed, _maxSpeed), rigid.velocity.y);
        }
    }

    void Jump()
    {
        _isFalling = true;
        animator.Play("Jump");
        AddForce(Vector3.up * _jumpPower);
    }

    protected override void Land()
    {
        base.Land();
        _landing.Play();
        animator.SetTrigger("Land");
    }

    void Move(float horizontal)
    {
        AddForce(Vector3.right * horizontal * _speed * _timeBetInputHandle);
    }
}
