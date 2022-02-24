using UnityEngine;
using Unity.Netcode;
using System;

[Serializable]
public class MatchConfig
{
    public MatchConfig(Arena arena)
    {
        Arena = arena;
    }

    public Arena Arena { get; set; }

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
    public string GetMaxTeamDescription => MaxTeams > 1 ? "NO TEAM" : string.Format("{0} TEAMS", MaxTeams);

    public (string, Func<bool>) WinCondition => WinConditions[WinConditionIndex];

    private static (string, Func<bool>)[] WinConditions =
    {
        // NO EXIT! >:]
        ("NO WIN", () => false),

        // INSTANT EXIT! ]:<
        ("INSTANT WIN", () => true)
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
}
