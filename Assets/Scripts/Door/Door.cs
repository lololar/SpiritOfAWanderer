using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

    public GameObject _linkedDoor;
    public ChangeAtelier.CameraPosition _linkedRoom;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            DoorManager.GetInstance.EnterDoor(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            DoorManager.GetInstance.ExitDoor(gameObject);
        }
    }
}
