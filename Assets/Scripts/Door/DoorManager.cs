using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorManager : MonoBehaviour {

    private static DoorManager Instance;

    public static DoorManager GetInstance
    {
        get
        {
            if (Instance == null)
            {
                Instance = new DoorManager();
            }
            return Instance;
        }
    }

    void Awake()
    {
        Instance = GetComponent<DoorManager>();
    }

    public List<GameObject> _doors;
    public GameObject _currentDoor;

    // Use this for initialization
    void Start()
    {

        _doors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Door"));

    }

    public void EnterDoor(GameObject door)
    {
        if (_doors.Contains(door))
        {
            _currentDoor = door;
        }
    }

    public void ExitDoor(GameObject door)
    {
        if (_doors.Contains(door) && _currentDoor == door)
        {
            _currentDoor = null;
        }
    }

    public void WarpToLinkedDoor(GameObject entity)
    {
        if (entity && _currentDoor && _currentDoor.GetComponent<Door>()._linkedDoor)
        {
            entity.transform.position = _currentDoor.GetComponent<Door>()._linkedDoor.transform.position;
            AtelierManager.GetInstance._change.Change(_currentDoor.GetComponent<Door>()._linkedRoom);
        }

    }
}
