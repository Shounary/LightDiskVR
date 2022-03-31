using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponSelectUiController : MonoBehaviour
{
    public List<TextMeshProUGUI> descriptions;
    public List<Image> images;

    public void UpdateWeaponDisplay(List<Weapon> weaponList)
    {
        for(int i=0; i < weaponList.Count; i++)
        {
            descriptions[i].text = weaponList[i].weaponName;
            images[i].sprite = weaponList[i].weaponImage;
        }
    }
}
