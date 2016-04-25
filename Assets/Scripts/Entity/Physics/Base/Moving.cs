using UnityEngine;
using System.Collections;
using System;

public class Moving : MonoBehaviour {

    public float _tileSize = 0.16f;
    public Vector3 _size;
    public float _mass = 1.0f;

    public float _slow = 1.0f;

    public Vector3 _velocity;

    public bool _isAlive = true; 
    public bool _isFalling = true;
    public bool _isMoving = false;
    
    public Coroutine _moveCor;
    public Coroutine _collCor;
    public float _timeBetweenPhysicsFrames = 0.05f;
    public float _collidePrecision = 0.08f;

    public virtual void Start () {
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

    private void CollisionAction()
    {
        CheckCollision(Values.Multiply(_velocity * _timeBetweenPhysicsFrames, Vector3.up));
        CheckCollision(Values.Multiply(_velocity * _timeBetweenPhysicsFrames, Vector3.right));
    }

    private void MoveAction()
    {
        if(_isFalling)
        {
            _velocity = new Vector3(_velocity.x, _velocity.y + GameManager._gravity.y * _timeBetweenPhysicsFrames * _mass, _velocity.z);
        }
        if(Mathf.Abs(_velocity.x) < Values.EPSYLON)
        {
            _velocity.x = 0.0f;
        }
        else
        {
            _velocity = new Vector3(_velocity.x - _velocity.x / _slow * _timeBetweenPhysicsFrames, _velocity.y, _velocity.z);
        }
        transform.position += _velocity * _timeBetweenPhysicsFrames;
    }

    protected virtual void CheckCollision(Vector3 direction)
    {
        bool hasCollided = false;
        Vector3 collisionPointRU = transform.position + transform.localScale / 2;
        Vector3 collisionPointLD = transform.position - transform.localScale / 2;
        for (int i = 0; i < PhysicalObjectManager.GetInstance._staticObjects.Count; i++)
        {
            Static stat = PhysicalObjectManager.GetInstance._staticObjects[i];
            float size = Values.Multiply(transform.localScale, Values.Absolute(direction.normalized) + Vector3.forward).magnitude;
            Vector3 collisionPointStart = transform.position + Values.Multiply(transform.localScale / 2, Vector3.one - Values.Absolute(direction.normalized) - Vector3.forward);
            for (float j = 0.0f; j < size; j+= _collidePrecision)
            {
                /*//*
                if (Collide(direction, collisionPointRU, collisionPointLD, stat.gameObject) && !hasCollided)
                {
                    hasCollided = true;
                }
                /*/if ((Collide(direction, collisionPointStart, stat.gameObject)) && !hasCollided)
                {
                    hasCollided = true;
                }//*/
                collisionPointStart += (Vector3.one - Values.Absolute(direction.normalized) + Vector3.forward) * _collidePrecision;
            }
        }
        for (int i = 0; i < PhysicalObjectManager.GetInstance._movingObjects.Count; i++)
        {
            Moving move = PhysicalObjectManager.GetInstance._movingObjects[i];
            if(move == this)
            {
                continue;
            }
            float size = Values.Multiply(transform.localScale, Values.Absolute(direction.normalized) + Vector3.forward).magnitude;
            Vector3 collisionPointStart = transform.position + Values.Multiply(transform.localScale / 2, direction.normalized);
            for (float j = 0.0f; j < size; j += _collidePrecision)
            {
                /*//*
                if (Collide(direction, collisionPointRU, collisionPointLD, move.gameObject) && !hasCollided)
                {
                    hasCollided = true;
                }
                /*/
                if ((Collide(direction, collisionPointStart, move.gameObject)) && !hasCollided)
                {
                    hasCollided = true;
                }//*/
                collisionPointStart += (Vector3.one - Values.Absolute(direction.normalized) + Vector3.forward) * _collidePrecision;
            }
        }
        _isFalling = !hasCollided;
    }

    private bool Collide(Vector3 direction, Vector3 collisionPointRU, Vector3 collisionPointLD, GameObject other)
    {
        Vector3 RU = other.transform.position + other.transform.localScale / 2;
        Vector3 LD = other.transform.position - other.transform.localScale / 2;
        if (SquaresOverlap(collisionPointRU, collisionPointLD, RU, LD))
        {
            transform.position = (transform.position - Values.Multiply(transform.position, -direction.normalized))
                - Values.Multiply(transform.localScale / 2 + other.transform.localScale / 2 + other.transform.position, direction.normalized);
            _velocity -= Values.Multiply(_velocity, -direction.normalized);
            return true;
        }
        return false;
    }

    private bool Collide(Vector3 direction, Vector3 collisionPoint, GameObject other)
    {
        Vector3 RU = other.transform.position + other.transform.localScale / 2;
        Vector3 LD = other.transform.position - other.transform.localScale / 2;
        if (IsPointInSquare(collisionPoint, RU, LD))
        {
            if(Values.Absolute(direction.normalized) == Vector3.right)
            {
                Debug.Log("Debut");
            }
            transform.position = (transform.position - Values.Multiply(transform.position, -direction.normalized))
                + Values.Multiply(transform.localScale / 2 + other.transform.localScale / 2, -direction.normalized)
                + Values.Multiply(other.transform.position, Values.Absolute(direction.normalized));
            _velocity += Values.Multiply(_velocity, direction.normalized);
            if (Values.Absolute(direction.normalized) == Vector3.right)
            {
                Debug.Log("Fin");
            }
            return true;
        }
        return false;
    }

    protected bool IsPointInSquare(Vector3 pt, Vector3 sqrRU, Vector3 sqrLD)
    {
        return pt.x <= sqrRU.x && pt.y <= sqrRU.y && pt.x >= sqrLD.x && pt.y >= sqrLD.y;
    }

    protected bool SquaresOverlap(Vector3 sqr1RU, Vector3 sqr1LD, Vector3 sqr2RU, Vector3 sqr2LD)
    {
        return sqr1LD.x < sqr2RU.x && sqr1RU.x > sqr2LD.x && sqr1LD.y < sqr2RU.y && sqr1RU.y > sqr2LD.y;
    }

    protected void AddForce(Vector3 force)
    {
        _velocity += force;
    }
}
