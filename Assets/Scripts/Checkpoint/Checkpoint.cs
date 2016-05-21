using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            CheckpointManager.GetInstance.ChangeCheckpoint(gameObject);
        }
    }
}
