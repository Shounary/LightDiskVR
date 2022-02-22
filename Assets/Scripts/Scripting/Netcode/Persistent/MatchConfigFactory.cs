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
    public Arena Arena { get { return Arenas[ArenaIndex]; } }

    public Arena GetArena(string name)
    {
        foreach (Arena a in Arenas)
        {
            if (a.name.Equals(name)) return a;
        }
        return null;
    }
}
