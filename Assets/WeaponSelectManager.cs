using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSelectManager : MonoBehaviour
{

    public List<GameObject> weaponPrefabs = new List<GameObject>();
    public List<GameObject> weaponsToSpawn = new List<GameObject>();
    public List<GameObject> defaultWeapons = new List<GameObject>(); //weapons which are assigned without being selected (acc field, tp disk in final version)
    public GameObject queuedWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //queues the weapon from the given index 
    public void QueueWeapon(int index)
    {
        queuedWeapon = weaponPrefabs[index];
    }

    public void ConfirmWeapon()
    {
        if(queuedWeapon != null)
        {
            weaponsToSpawn.Add(Instantiate(queuedWeapon));
            queuedWeapon = null;
        }
    }

    public void GoToScene(string name) 
    {
        foreach (GameObject o in weaponsToSpawn)
            DontDestroyOnLoad(o);
        SceneManager.LoadScene(name);

    }
}
