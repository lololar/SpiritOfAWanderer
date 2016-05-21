﻿using UnityEngine;
using System.Collections;

public class PlayerManager
{
    public enum GameMode
    {
        MENU,
        PAUSE,
        PLAYING,
        SPIRITING,
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
    }

    public PlayerPhysics _player;
    public Spirit _spirit;
    public GameMode _mode = GameMode.SPIRITING;

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

    public override void Start()
    {
        animator = GetComponent<Animator>();
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
                _timeBetInputHandle = Time.deltaTime;
                float h = Input.GetAxis("Horizontal");
                float v = Input.GetAxis("Vertical");
                bool isJumping = Input.GetButton("Jump");

                if (isJumping && !_isFalling)
                {
                    Jump();
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
        animator.Play("Land");
    }

    void Move(float horizontal)
    {
        AddForce(Vector3.right * horizontal * _speed * _timeBetInputHandle);
    }
}
