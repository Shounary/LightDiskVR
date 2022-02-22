using UnityEngine;
using Unity.Netcode;

[System.Serializable]
public class MatchConfig
{

    private Arena m_Arena = MatchConfigFactory.Instance.Arena;
    public Arena Arena
    {
        get { return m_Arena; }
        set
        {
            m_Arena = value;

            if (value == null) return;

            m_SpawnAvailability = new bool[value.SpawnPoints.Length];
            for (int i = 0; i < m_SpawnAvailability.Length; i++) m_SpawnAvailability[i] = true;

            Debug.Log("Arena has changed, availabilities have been cleared!");
        }
    }

    private int m_MaxTeams = 1; // 1 = every one is their own team; >1 = actually function like teams

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

    /// <summary>
    /// for UI description of the max team number
    /// </summary>
    public string GetMaxTeamDescription
    {
        get
        {
            return MaxTeams > 1 ? "NO TEAM" : string.Format("{0} TEAMS", MaxTeams); 
        }
    }

    public System.Tuple<string, System.Func<bool>> WinCondition
    {
        get { return WinConditions[WinConditionIndex]; }
    }

    private static System.Tuple<string, System.Func<bool>>[] WinConditions =
    {
        // NO EXIT! >:]
        new System.Tuple<string, System.Func<bool>>("NO WIN", () => false),

        // INSTANT EXIT! ]:<
        new System.Tuple<string, System.Func<bool>>("INSTANT WIN", () => true)
    };

    public int WinConditionIndex
    {
        get { return m_WinConditionIndex; }
        set
        {
            m_WinConditionIndex = MathUtils.Mod(value, WinConditions.Length);
        }
    }
    private int m_WinConditionIndex = 0;

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
