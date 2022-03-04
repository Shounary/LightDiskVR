using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Arena")]
public class Arena : ScriptableObject
{
    public int BuildIndex;

    [SerializeField]
    protected Vector3[] _spawnPoints;

    public Vector3[] SpawnPoints
    {
        get { return _spawnPoints; }
    }
}
