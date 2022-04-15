using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkPlayerStats : PlayerStats
{
    protected new void Start()
    {
        base.Start();
        NetUtils.NetworkVRPlayer.SetHealthServerRpc(health);
    }

    public override void takeDamage(int damage)
    {
        health = calculateDamage(damage);
        healthBar.displayHealth(health);
        NetUtils.NetworkVRPlayer.SetHealthServerRpc(health);
    }
}
