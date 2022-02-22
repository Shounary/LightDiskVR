using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public class PlayerConfig
{
    public MatchConfig MatchConfig;

    private int m_SpawnPoint = 0;

    public int SpawnPoint
    {
        get { return m_SpawnPoint; }
        set
        {
            Arena arena = MatchConfig.Arena;
            if (arena == null)
                throw new UnityException("Invalid Match Config (contains no arena)!");
            m_SpawnPoint = MathUtils.Mod(value, MatchConfig.Arena.SpawnPoints.Length);
        }
    }

    public Vector3? SpawnPosition => MatchConfig.Arena?.SpawnPoints[m_SpawnPoint];
}
