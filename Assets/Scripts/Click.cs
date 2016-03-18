using UnityEngine;
using System.Collections;

public class Click : MonoBehaviour {

    GameObject _object;

	// Use this for initialization
	void Start () {
        _object = Resources.Load("Prefabs/Cube") as GameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        /*if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("nsidngsdf");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                Instantiate(_object, hit.point, transform.rotation);
        }*/

	}
}
