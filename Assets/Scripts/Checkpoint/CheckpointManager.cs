using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheckpointManager : MonoBehaviour {
    
    private static CheckpointManager Instance;

    public static CheckpointManager GetInstance
    {
        get
        {
            if (Instance == null)
            {
                Instance = new CheckpointManager();
            }
            return Instance;
        }
    }

    void Awake()
    {
        Instance = GetComponent<CheckpointManager>();
    }

    public List<GameObject> _checkpoints;
    public GameObject _lastCheckpoint;
    public GameObject _currentCheckpoint;

	// Use this for initialization
	void Start () {

        _checkpoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Checkpoint"));

	}

    public void ChangeCheckpoint(GameObject chck)
    {
        if (_checkpoints.Contains(chck))
        {
            _lastCheckpoint = _currentCheckpoint = chck;
        }
    }

    public void ExitCheckpoint(GameObject chck)
    {
        if (_checkpoints.Contains(chck) && _currentCheckpoint == chck)
        {
            _currentCheckpoint = null;
        }
    }
}
