using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetUtils
{
    public static bool IsUnderNetwork => NetworkManager.Singleton != null && NetworkManager.Singleton.IsClient;
    public static BaseAccessor BaseAccessor => NetworkManager.Singleton.LocalClient.PlayerObject.GetComponent<BaseAccessor>();
    public static NetworkVRPlayer NetworkVRPlayer => BaseAccessor.Player;
}
