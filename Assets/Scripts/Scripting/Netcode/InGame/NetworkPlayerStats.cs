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
        base.takeDamage(damage);
        NetUtils.NetworkVRPlayer.SetHealthServerRpc(health);
    }
}
