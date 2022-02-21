[System.Serializable]
public class MatchConfig
{

    private Arena m_Arena;
    public Arena Arena
    {
        get { return m_Arena; }
        set
        {
            m_Arena = value;

            m_SpawnAvailability = new bool[value.SpawnPoints.Length];
            for (int i = 0; i < m_SpawnAvailability.Length; i++) m_SpawnAvailability[i] = true;
        }
    }

    private int m_MaxTeams;

    public int MaxTeams
    {
        get { return m_MaxTeams; }
        set
        {
            if (value > 0)
            {
                m_MaxTeams = value;
            }
        }
    }

    public System.Func<bool> WinCondition;

    private bool[] m_SpawnAvailability;

    /// <summary>
    /// check if a spawn point is available
    /// if so, register that spawn point
    /// </summary>
    /// <param name="spot">index for registrering spawn point</param>
    /// <returns></returns>
    public bool RegisterSpawnPoint(int spot)
    {
        if (
            m_SpawnAvailability == null 
            || spot < 0 
            || spot >= m_SpawnAvailability.Length 
            || !m_SpawnAvailability[spot]
            ) 
            return false;

        m_SpawnAvailability[spot] = false;
        
        return true;
    }
}
