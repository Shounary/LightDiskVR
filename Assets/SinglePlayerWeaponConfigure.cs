using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerWeaponConfigure : MonoBehaviour
{
    public InventorySelectScript s1;
   // public InventorySelectScript s2;
    public GameObject button; 
    private void Awake() {
        Time.timeScale = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDisks()
    {
        Time.timeScale = 1;
        s1.SpawnWeapons();
        DestroyImmediate(button);
        gameObject.SetActive(false);
    }
}
