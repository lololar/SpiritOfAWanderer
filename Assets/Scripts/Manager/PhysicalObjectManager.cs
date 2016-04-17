using UnityEngine;
using System.Collections.Generic;

public class PhysicalObjectManager : Singleton<PhysicalObjectManager> {

    public List<Moving> _movingObjects;
    public List<Static> _staticObjects;

	// Use this for initialization
	void Start () {
        _movingObjects = new List<Moving>(FindObjectsOfType<Moving>());
        _staticObjects = new List<Static>(FindObjectsOfType<Static>());
	}
}
