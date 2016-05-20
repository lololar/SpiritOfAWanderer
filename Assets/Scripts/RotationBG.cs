using UnityEngine;
using System.Collections;

public class RotationBG : MonoBehaviour {

    public GameObject _inner;
    public GameObject _middle;
    public GameObject _outer;
    public float _speedInner;
    public float _speedMiddle;
    public float _speedOuter;

    // Use this for initialization
    void Start () {

        _inner = transform.FindChild("Inner").gameObject;
        _middle = transform.FindChild("Middle").gameObject;
        _outer = transform.FindChild("Outer").gameObject;

    }
	
	// Update is called once per frame
	void Update () {

        _inner.transform.Rotate(Vector3.forward, Time.deltaTime * _speedInner);
        _middle.transform.Rotate(Vector3.forward, Time.deltaTime * _speedMiddle);
        _outer.transform.Rotate(Vector3.forward, Time.deltaTime * _speedOuter);

    }
}
