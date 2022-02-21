using UnityEngine;

[System.Serializable]
public class PlayerConfig : MonoBehaviour
{
    public MatchConfig MatchConfig;

    private int? m_SpawnPoint;

    public int? SpawnPoint
    {
        get { return m_SpawnPoint; }
        set
        {
            if (MatchConfig.RegisterSpawnPoint(value.GetValueOrDefault(-1)))
            {
                m_SpawnPoint = value;
            }
        }
    }

    public Vector3? SpawnPosition
    {
        get {
            if (m_SpawnPoint == null) return null;
            return MatchConfig.Arena?.SpawnPoints[m_SpawnPoint.GetValueOrDefault(0)]; }
    }
}
