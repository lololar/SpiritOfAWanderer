using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AtelierManager
{
    private static AtelierManager Instance;

    public static AtelierManager GetInstance
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
        Instance = new AtelierManager();
        Instance._change = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChangeAtelier>();
    }

    public ChangeAtelier _change;
}

public class ChangeAtelier : MonoBehaviour {

    public enum CameraPosition
    {
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        FIFTH,
        SIXTH,
        SEVENTH,
        EIGHTH,
        NINTH,
        GLOBAL,
        END,
    }

    public CameraPosition _position;
    public CameraPosition _currentRoom;
    public Camera _camera;

    public List<Transform> _ateliersPositions;
    public Transform _globalCameraPosition;


    public float _zoomAtelier;
    public float _zoomGlobal;
    public float _delay = 3.0f;

    // Use this for initialization
    void Start () {

        _camera = GetComponent<Camera>();

        _ateliersPositions = new List<Transform>();
        Transform points = GameObject.Find("Points").transform;
        for (int i = 0; i < points.childCount; i++)
        {
            _ateliersPositions.Add(points.GetChild(i));
        }
        _globalCameraPosition = points;

        RemoveBlockView(0);
        Change(CameraPosition.GLOBAL);

	}
	
	// Update is called once per frame
	void Update () {
	


	}

    public void Change(CameraPosition position)
    {
        switch (position)
        {
            case CameraPosition.GLOBAL:
                _currentRoom = _position;
                StartCoroutine(LerpPosition(_globalCameraPosition.position, _zoomGlobal, -1));
                break;
            case CameraPosition.FIRST:
            case CameraPosition.SECOND:
            case CameraPosition.THIRD:
            case CameraPosition.FOURTH:
            case CameraPosition.FIFTH:
            case CameraPosition.SIXTH:
            case CameraPosition.SEVENTH:
            case CameraPosition.EIGHTH:
            case CameraPosition.NINTH:
            case CameraPosition.END:
                StartCoroutine(LerpPosition(_ateliersPositions[(int)position].position, _zoomAtelier, (int)position));
                break;
            default:
                break;
        }
        _position = position;
    }

    public IEnumerator LerpPosition(Vector3 position, float size, int pos)
    {
        RemoveBlockView(pos);

        Vector3 startPosition = transform.position;
        float startSize = _camera.orthographicSize;
        float timer = 0.0f;
        while (timer < _delay)
        {
            timer += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, position, timer / _delay);
            _camera.orthographicSize = Mathf.Lerp(startSize, size, timer / _delay);

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void RemoveBlockView(int index)
    {
        if (index >= 0 && index < _ateliersPositions.Count)
        {
            _ateliersPositions[index].GetChild(0).gameObject.SetActive(false);
        }
    }
}
