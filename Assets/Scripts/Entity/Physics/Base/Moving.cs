using UnityEngine;
using System.Collections;
using System;

public class Moving : MonoBehaviour {

    public float _tileSize = 0.16f;
    public Vector3 _size;

    public float _slow = 1.0f;

    public Vector3 _velocity;

    public bool _isAlive = true; 
    public bool _isFalling = true;
    public bool _isMoving = false;

    public Coroutine _fallCor;
    public Coroutine _moveCor;
    public Coroutine _collCor;
    public float _timeBetweenPhysicsFrames = 0.05f;


    public virtual void Start () {
        _fallCor = StartCoroutine(Falling());
        _moveCor = StartCoroutine(MovePhysics());
        _moveCor = StartCoroutine(CollisionCor());
	}

    private IEnumerator CollisionCor()
    {
        while (_isAlive)
        {
            CollisionAction();
            yield return new WaitForSeconds(_timeBetweenPhysicsFrames);
        }
    }

    private IEnumerator MovePhysics()
    {
        while (_isAlive)
        {
            MoveAction();
            yield return new WaitForSeconds(_timeBetweenPhysicsFrames);
        }
    }
    protected IEnumerator Falling()
    {
        while (_isAlive)
        {
            FallingAction();
            yield return new WaitForSeconds(_timeBetweenPhysicsFrames);
        }
    }

    private void CollisionAction()
    {
        //Debug.Log((_velocity * _timeBetweenPhysicsFrames).ToString("F4"));
        //Debug.Log(_timeBetweenPhysicsFrames);
        CheckCollision(Values.Multiply(_velocity * _timeBetweenPhysicsFrames, Vector3.up));
        //CheckCollision(Values.Multiply(_velocity * _timeBetweenPhysicsFrames, Vector3.left));
    }

    private void MoveAction()
    {
        transform.position += _velocity * _timeBetweenPhysicsFrames;
    }

    private void FallingAction()
    {
        if(_isFalling)
        {
            _velocity = _velocity / _slow + GameManager._gravity * _timeBetweenPhysicsFrames;
        }
    }

    protected virtual void CheckCollision(Vector3 direction)
    {
        //Debug.Log(direction);
        bool hasCollided = false;
        //Debug.Log(transform.localScale.ToString("F4"));
        Vector3 collisionPoint = transform.position + Values.Multiply(transform.localScale, direction.normalized);
        //Debug.Log("Position : " + transform.position.ToString("F4") + " ///////// Collision detection : " + collisionPoint.ToString("F4"));
        for (int i = 0; i < PhysicalObjectManager.GetInstance._staticObjects.Count; i++)
        {
            Static stat = PhysicalObjectManager.GetInstance._staticObjects[i];
            hasCollided = !hasCollided || Collide(direction, collisionPoint, stat.gameObject);
        }
        for (int i = 0; i < PhysicalObjectManager.GetInstance._movingObjects.Count; i++)
        {
            Moving move = PhysicalObjectManager.GetInstance._movingObjects[i];
            if(move == this)
            {
                continue;
            }
            hasCollided = !hasCollided || Collide(direction, collisionPoint, move.gameObject);
        }
        if(Mathf.Abs(direction.y) > Values.EPSYLON)
        {
            _isFalling = !hasCollided;
        }
    }

    private bool Collide(Vector3 direction, Vector3 collisionPoint, GameObject other)
    {
        Vector3 RU = other.transform.position + other.transform.localScale;
        Vector3 LD = other.transform.position - other.transform.localScale;
        if (IsPointInSquare(collisionPoint, RU, LD))
        {
            Debug.Log("Collision");
            transform.position = Values.Multiply(transform.localScale + other.transform.localScale, -direction.normalized) + other.transform.position;
            _velocity = Values.Multiply(_velocity, Vector3.one - direction.normalized);
            return true;
        }
        return false;
    }

    protected bool IsPointInSquare(Vector3 pt, Vector3 sqrRU, Vector3 sqrLD)
    {
        Debug.Log("Collision : ");
        Debug.Log("PT : " + pt);
        Debug.Log("RU : " + sqrRU);
        Debug.Log("LD : " + sqrLD);
        return pt.x < sqrRU.x && pt.y < sqrRU.y && pt.x > sqrLD.x && pt.y > sqrLD.y;
    }

    protected void AddForce(Vector3 force)
    {
        _velocity += force;
    }
}
