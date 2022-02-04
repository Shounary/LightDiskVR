using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaInfo : MonoBehaviour
{
    public Rigidbody[] playerDisks;
    public static ArenaInfo instance;

    private void Awake() {
        instance = this;
    }
}
