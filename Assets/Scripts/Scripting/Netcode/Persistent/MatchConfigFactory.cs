using UnityEngine;

public class MatchConfigFactory : MonoBehaviour
{
    public static MatchConfigFactory Instance
    {
        get { return m_Instance; }
    }
    private static MatchConfigFactory m_Instance;

    private void Awake()
    {
        Debug.Log("Match Config resources loaded");
        m_Instance = this;
    }

    [SerializeField]
    private Arena[] Arenas;
    public int ArenaIndex
    {
        get { return m_ArenaIndex; }
        set
        {
            m_ArenaIndex = MathUtils.Mod(value, Arenas.Length);
        }
    }
    private int m_ArenaIndex = 0;
    public Arena Arena { get { Debug.Log(string.Format("trying to get arena at arr index {0}", m_ArenaIndex)); return Arenas[ArenaIndex]; } }

    public Arena GetArena(int id)
    {
        foreach (Arena a in Arenas)
        {
            if (a.BuildIndex == id) return a;
        }
        return null;
    }
}
