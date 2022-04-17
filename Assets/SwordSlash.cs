using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSlash : Weapon
{
    public GameObject parentBlade;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!GameObject.ReferenceEquals(collision.collider, parentBlade.GetComponent<Collider>()))
        {
            Destroy(gameObject);
        }
    }
}