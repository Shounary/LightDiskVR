using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerWeaponConfigure : MonoBehaviour
{
    public InventorySelectScript s1;
   // public InventorySelectScript s2;
    public GameObject button; 
    public GameObject uiHand1;
    public GameObject uiHand2;
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
        s1.gameObject.transform.localScale = Vector3.zero;
        button.SetActive(false);
        button.transform.localScale = Vector3.zero;
        Destroy(uiHand1);
        Destroy(uiHand2);
        DestroyImmediate(button);
        gameObject.SetActive(false);
    }
}
