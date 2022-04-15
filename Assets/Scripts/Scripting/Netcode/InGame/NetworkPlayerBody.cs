using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerBody : PlayerBody {
    protected override void OnTriggerEnter(Collider other)
    {
        Weapon w = other.gameObject.GetComponent<Weapon>();
        bool namecheck;
        if (w is NetworkWeapon)
        {
            namecheck = !w.GetComponent<NetworkObject>().IsOwner && NetUtils.BaseAccessor.WeaponInventory.weaponList.Contains(w);

            if (w != null && namecheck && intMaskList.Contains(1 << other.gameObject.layer))
                ps.takeDamage(w.damage);
        }
    }

}
