using UnityEngine;
using System.Collections;
using System;

public class Spirit : MonoBehaviour
{
    enum EntityColor
    {
        PLAYER,
        ELEMENT,
        END,
    }

    public Transform _possessed;
    public Transform _possessedParent;
    public LayerMask _background;
    public LayerMask _possessible;

    public ParticleSystem _deconstruction;

    public Color color;
    public Color[] entitiesColor;

    Renderer render;
    bool playerControlled;

    Color GetColor(Texture2D tex)
    {
        Color color = Color.black;
        for (int i = 0; i < tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                if(tex.GetPixel(i, j).a > 0.8f)
                    color = Color.Lerp(color, tex.GetPixel(i, j), 0.5f);
            }
        }
        return color;
    }

    // Use this for initialization
    void Start()
    {
        color = GetColor(GetComponent<SpriteRenderer>().sprite.texture);

        entitiesColor = new Color[(int)EntityColor.END];

        entitiesColor[(int)EntityColor.PLAYER] = GetColor(GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sprite.texture);
        entitiesColor[(int)EntityColor.ELEMENT] = GetColor(GameObject.FindGameObjectWithTag("Element").GetComponent<SpriteRenderer>().sprite.texture);

        _deconstruction = GetComponent<ParticleSystem>();
        render = GetComponent<Renderer>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        //StartCoroutine(Possess());

    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitBackground = Physics.Raycast(ray, out hit, float.PositiveInfinity, _background);
        transform.position = hit.point;
        if (Input.GetButtonDown("Possession"))
        {
            if (_possessed)
            {
                Unpossession();
            }
            else if (hitBackground && !_possessed)
            {
                Debug.Log("Try Possession");
                RaycastHit2D hit2;
                Debug.DrawRay(hit.point, Vector3.forward, Color.black, 1f);
                if (hit2 = Physics2D.Raycast(hit.point - Vector3.forward * 30.0f, Vector3.forward, 60.0f, _possessible))
                {
                    Possession(hit2.transform);
                }
            }
        }
    }

    private void Possession(Transform trans)
    {
        _possessed = trans;
        _possessed.GetComponent<Rigidbody2D>().isKinematic = true;
        _possessedParent = _possessed.parent;
        _possessed.parent = transform;
        render.enabled = false;


        Deconstruction(trans, true);
    }

    private void Unpossession()
    {
        _possessed.parent = _possessedParent;
        _possessed.GetComponent<Rigidbody2D>().isKinematic = false;
        _possessedParent = null;
        Transform trans = _possessed;
        _possessed = null;
        render.enabled = true;


        Deconstruction(trans, false);
    }

    void Deconstruction(Transform trans, bool possession)
    {
        int col = 0;

        switch (trans.tag)
        {
            case "Player":
                col = (int)EntityColor.PLAYER;
                break;
            case "Element":
                col = (int)EntityColor.ELEMENT;
                break;
            default:
                break;
        }
        if (possession)
        {
            _deconstruction.GetComponent<ParticleAttractor>().startColor = color;
            _deconstruction.GetComponent<ParticleAttractor>().endColor = entitiesColor[col];
            _deconstruction.GetComponent<ParticleAttractor>().attractor = trans;
            _deconstruction.Play();
        }
        else
        {
            _deconstruction.GetComponent<ParticleAttractor>().startColor = entitiesColor[col];
            _deconstruction.GetComponent<ParticleAttractor>().endColor = color;
            _deconstruction.GetComponent<ParticleAttractor>().attractor = transform;
            _deconstruction.Play();
        }
    }
}
