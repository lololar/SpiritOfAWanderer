using UnityEngine;
using System.Collections;

public class PlayerPhysics : Moving {

    public float _timeBetInputHandle = 0.1f;
    public float _jumpPower = 5.0f;
    public float _speed = 5.0f;

    public override void Start()
    {
        StartCoroutine(InputHandler());
        base.Start();
    }

    IEnumerator InputHandler()
    {
        while(_isAlive)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            bool isJumping = Input.GetButton("Jump");

            if(isJumping && !_isFalling)
            {
                Jump();
            }
            if(Mathf.Abs(h) > 0.3f)
            {
                Move(h);
            }
            yield return new WaitForSeconds(_timeBetInputHandle);
        }
    }

    void Jump()
    {
        AddForce(Vector3.up * _jumpPower);
    }

    void Move(float horizontal)
    {
        AddForce(Vector3.right * horizontal * _speed);
    }
}
