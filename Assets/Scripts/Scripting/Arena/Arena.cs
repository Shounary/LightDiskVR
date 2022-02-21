using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Arena")]
public class Arena : ScriptableObject
{
    [SerializeField]
    private ScenePicker scene;

    public string ScenePath
    {
        get { return scene.scenePath; }
    }

    [SerializeField]
    private Vector3[] _spawnPoints;

    public Vector3[] SpawnPoints
    {
        get { return _spawnPoints; }
    }

}
