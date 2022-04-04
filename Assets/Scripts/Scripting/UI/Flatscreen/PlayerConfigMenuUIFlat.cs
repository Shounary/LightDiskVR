using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Button = UnityEngine.UI.Button;
using Text = UnityEngine.UI.Text;

public class PlayerConfigMenuUIFlat : MonoBehaviour
{
    public Button ReadyButton;
    public Text ReadyText;
    public Button WeaponLeft;
    public Button WeaponRight;
    [SerializeField] Text m_Weapon;

    public GameObject[] AvailableWeapons;

    public static PlayerConfigMenuUIFlat Instance;

    public static Text Weapon => Instance.m_Weapon;

    BaseAccessor owner;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        BaseAccessor accessor = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<BaseAccessor>();
        if (NetworkManager.Singleton.IsHost)
        {
            ReadyText.text = "I'm Ready";
        } else
        {
            ReadyText.text = "Start Match";
        }

        ReadyButton.onClick.RemoveAllListeners();
        ReadyButton.onClick.AddListener(delegate
        {
            if (accessor.IsOwner && NetworkManager.Singleton.IsHost)
            {
                accessor.PlayerConfigExit();
            } else
            {
                accessor.SetLockServerRpc(!accessor.Lock.Value);
            }
            accessor.PlayerConfig.InitialWeapon = AvailableWeapons[accessor.PlayerConfig.WeaponIndex];
        });

        WeaponLeft.onClick.RemoveAllListeners();
        WeaponLeft.onClick.AddListener(delegate
        {
            accessor.SetWeaponIndexServerRpc(MathUtils.Mod(accessor.PlayerConfig.WeaponIndex - 1, AvailableWeapons.Length));
        });

        WeaponRight.onClick.RemoveAllListeners();
        WeaponRight.onClick.AddListener(delegate
        {
            accessor.SetWeaponIndexServerRpc(MathUtils.Mod(accessor.PlayerConfig.WeaponIndex + 1, AvailableWeapons.Length));
        });

        owner = accessor;
    }

    private void Update()
    {
        m_Weapon.text = AvailableWeapons[owner.PlayerConfig.WeaponIndex].GetComponent<Weapon>().weaponName;
    }
}
