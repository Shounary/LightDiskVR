using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class InventorySelectScript : MonoBehaviour
{
    public WeaponInventory weaponInventory;
    public Hand hand;
    public Weapon[] weapons;
    public Weapon chosenWeapon;
    public string arenaName;
    public TextMeshProUGUI activeWeaponText;
    public PlayerStats stats;


    private void Start() {
        SceneManager.activeSceneChanged += OnSceneChangeSpawnDisk;
    }

    public void ChooseActiveDisk(int index) //chooses the active disk
    {
        chosenWeapon = weapons[index];
        activeWeaponText.text = chosenWeapon.name;
    }

    public void OnSceneChangeSpawnDisk(Scene curr, Scene next)
    {
        if(next.name.Equals(arenaName))
        {
            chosenWeapon.gameObject.SetActive(true);
            chosenWeapon.gameObject.transform.SetPositionAndRotation(this.transform.position, Quaternion.identity);
            weaponInventory.setActiveWeapon(chosenWeapon, hand);
            stats.playerName = weaponInventory.playerName;
        }
    }

    public void amogus()
    {
        Debug.Log("When the imposter is sus");
    }

}
