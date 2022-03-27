using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Singleton<T> : NetworkBehaviour 
    where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>().GetComponent<T>();
            }
            return _instance;
        } 
    }
}
