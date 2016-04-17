using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour  where T : Object{

    private static T Instance;

    public static T GetInstance
    {
        get
        {
            if(Instance == null)
            {
                Instance = FindObjectOfType<T>();
            }
            return Instance;
        }
    }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = FindObjectOfType<T>();
        }
    }
}
