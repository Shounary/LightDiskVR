using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Samples;
using Unity.Netcode.Components;

[RequireComponent(typeof(ClientNetworkTransform))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NetworkWeapon))]
public class NetworkDisk : NetworkBehaviour
{
    [SerializeField] NetworkWeapon Target;

    public override void OnGainedOwnership()
    {
        base.OnGainedOwnership();
        Debug.Log("Gained Ownership");
    }


    [ServerRpc(RequireOwnership = false)]
    public void DestroyWeaponServerRpc()
    {
        NetworkObject.Despawn(true);
    }


    //Add ownershipRequired = false
    [ServerRpc(RequireOwnership = false)]
    public void AttractWeaponServerRpc(Vector3 f)
    {
        Target.weaponRB.AddForce(f, ForceMode.VelocityChange);
    }
}
