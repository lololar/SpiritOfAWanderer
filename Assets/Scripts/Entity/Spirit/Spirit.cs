using UnityEngine;
using System.Collections;
using System;

public class Spirit : MonoBehaviour
{
    public Transform _possessed;
    public Transform _possessedParent;
    public LayerMask _background;
    public LayerMask _possessible;

    // Use this for initialization
    void Start()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        //StartCoroutine(Possess());

    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(ray, out hit, float.PositiveInfinity, _background);
        transform.position = hit.point;
        if (Input.GetButtonDown("Possession"))
        {
            if (_possessed)
            {
                _possessed.parent = _possessedParent;
                _possessed.GetComponent<Rigidbody2D>().isKinematic = false;
                _possessedParent = null;
                _possessed = null;
            }
            else if (hitSomething && !_possessed)
            {
                Debug.Log("Poss");
                RaycastHit2D hit2;
                Debug.DrawRay(hit.point, Vector3.forward, Color.black, 1f);
                if (hit2 = Physics2D.Raycast(hit.point - Vector3.forward * 30.0f, Vector3.forward, 60.0f, _possessible))
                {
                    _possessed = hit2.transform;
                    _possessed.GetComponent<Rigidbody2D>().isKinematic = true;
                    _possessedParent = _possessed.parent;
                    _possessed.parent = transform;
                }
            }
        }
    }
}
