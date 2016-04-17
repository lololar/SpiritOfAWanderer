using UnityEngine;
using System.Collections;
using System;

public class Moving : MonoBehaviour {

    public float _tileSize = 0.16f;
    public Vector3 _size;

    public float _slow = 1.0f;

    public Vector3 _velocity;

    public bool _isFalling = false;
    public bool _isMoving = false;

    public Coroutine _physicCor;
    public float _timeBetweenPhysicsFrames = 0.05f;


    public virtual void Start () {
        _physicCor = StartCoroutine(Falling());
	}

    protected IEnumerator Falling()
    {
        bool isAlive = true; 
        while (isAlive)
        {
            FallingAction();
            yield return new WaitForSeconds(_timeBetweenPhysicsFrames);
        }
    }

    private void FallingAction()
    {
        if(_isFalling)
        {
            _velocity = _velocity / _slow + GameManager._gravity;
        }
        CheckCollision(Vector3.Cross(Vector3.up, _velocity));
        CheckCollision(Vector3.Cross(Vector3.left, _velocity));
        if(_isFalling)
        {
            transform.position += _velocity;
        }
    }

    protected virtual void CheckCollision(Vector3 direction)
    {
        Vector3 collisionPoint = transform.position + Vector3.Cross(direction.normalized, transform.localScale) + direction;
        for (int i = 0; i < PhysicalObjectManager.GetInstance._staticObjects.Count; i++)
        {
            Static stat = PhysicalObjectManager.GetInstance._staticObjects[i];
            Vector3 RU = stat.transform.position + stat.transform.localScale / 2;
            Vector3 LD = stat.transform.position - stat.transform.localScale / 2;
            if (IsPointInSquare(collisionPoint, RU, LD))
            {
                transform.position = Vector3.Cross(-direction.normalized, transform.localScale + stat.transform.localScale) + stat.transform.position;
                _velocity = Vector3.zero;
            }
        }
        for (int i = 0; i < PhysicalObjectManager.GetInstance._movingObjects.Count; i++)
        {
            Moving move = PhysicalObjectManager.GetInstance._movingObjects[i];
            if(move == this)
            {
                continue;
            }
            Vector3 RU = move.transform.position + move.transform.localScale / 2;
            Vector3 LD = move.transform.position - move.transform.localScale / 2;
            if (IsPointInSquare(collisionPoint, RU, LD))
            {
                transform.position = Vector3.Cross(-direction.normalized, transform.localScale + move.transform.localScale) + move.transform.position;
                _velocity = Vector3.zero;
            }
        }
    }

    protected bool IsPointInSquare(Vector3 pt, Vector3 sqrRU, Vector3 sqrLD)
    {
        return pt.x < sqrRU.x && pt.y < sqrRU.y && pt.x > sqrLD.x && pt.y > sqrLD.y;
    }
}
