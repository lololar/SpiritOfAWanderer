using UnityEngine;
using System.Collections;
using System;

public class Moving : MonoBehaviour
{

    enum CollidePoint
    {
        UP, DOWN, LEFT, RIGHT, END,
    }
    protected Rigidbody2D rigid;

    public bool _isAlive = true;
    public bool _isFalling = true;
    public Coroutine _moveCor;
    public float _timeBetweenPhysicsFrames = 0.05f;

    bool[] collidePoint;

    public virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        _moveCor = StartCoroutine(MovePhysics());
        collidePoint = new bool[(int)CollidePoint.END];
    }

    private IEnumerator MovePhysics()
    {
        while (_isAlive)
        {
            MoveAction();
            yield return new WaitForSeconds(_timeBetweenPhysicsFrames);
        }
    }

    protected virtual void MoveAction()
    {
        if (!_isFalling)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, 0.0f);
        }
        if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.3f)
        {
            rigid.velocity = new Vector2(0.0f, rigid.velocity.y);
        }
    }

    protected void AddForce(Vector3 force)
    {
        rigid.AddForce(force);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        int collideU = 0;
        int collideD = 0;
        int collideL = 0;
        int collideR = 0;

        for (int i = 0; i < coll.contacts.Length; i++)
        {
            ContactPoint2D cont = coll.contacts[i];
            if (cont.point.y > transform.position.y)
            {
                collideU++;
            }
            else if (cont.point.y < transform.position.y)
            {
                collideD++;
            }
            if (cont.point.x > transform.position.x)
            {
                collideR++;
            }
            else if (cont.point.x < transform.position.x)
            {
                collideL++;
            }
        }

        /*Debug.Log("COLLSION");
        Debug.Log("UP : " + collideU);
        Debug.Log("DOWN : " + collideD);
        Debug.Log("RIGHT : " + collideR);
        Debug.Log("LEFT : " + collideL);*/

        if (collideU == 2)
        {
            collidePoint[(int)CollidePoint.UP] = true;
        }
        if (collideD == 2)
        {
            collidePoint[(int)CollidePoint.DOWN] = true;
            _isFalling = false;
            Land();
        }
        if (collideR == 2)
        {
            collidePoint[(int)CollidePoint.RIGHT] = true;
        }
        if (collideL == 2)
        {
            collidePoint[(int)CollidePoint.LEFT] = true;
        }
    }

    protected virtual void Land()
    {
        
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        int collideU = 0;
        int collideD = 0;
        int collideL = 0;
        int collideR = 0;

        for (int i = 0; i < coll.contacts.Length; i++)
        {
            ContactPoint2D cont = coll.contacts[i];
            if (cont.point.y > transform.position.y)
            {
                collideU++;
            }
            else if (cont.point.y < transform.position.y)
            {
                collideD++;
            }
            if (cont.point.x > transform.position.x)
            {
                collideR++;
            }
            else if (cont.point.x < transform.position.x)
            {
                collideL++;
            }
        }

        /*Debug.Log("COLLSION");
        Debug.Log("UP : " + collideU);
        Debug.Log("DOWN : " + collideD);
        Debug.Log("RIGHT : " + collideR);
        Debug.Log("LEFT : " + collideL);*/

        if (collideU == 2)
        {
            collidePoint[(int)CollidePoint.UP] = false;
        }
        if (collideD == 2)
        {
            collidePoint[(int)CollidePoint.DOWN] = false;
            _isFalling = true;
        }
        if (collideR == 2)
        {
            collidePoint[(int)CollidePoint.RIGHT] = false;
        }
        if (collideL == 2)
        {
            collidePoint[(int)CollidePoint.LEFT] = false;
        }
    }
}
