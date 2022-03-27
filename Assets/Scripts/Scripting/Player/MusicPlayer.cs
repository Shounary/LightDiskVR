using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class MusicPlayer : MonoBehaviour
{
    public int SongIdx
    {
        get { return m_SongIdx; }
        private set {
            if (songs.Length == 0) return;
            m_SongIdx = MathUtils.Mod(value, songs.Length);

            if (currentSong.isValid())
            {
                currentSong.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                currentSong.release();
            }

            currentSong = RuntimeManager.CreateInstance(songs[m_SongIdx]);
            currentSong.start();
        }
    }
    int m_SongIdx = 0;
    public EventReference[] songs;
    public bool loop = false;

    EventInstance currentSong;

    // Start is called before the first frame update
    void Start()
    {
        RandomSong();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentSong.isValid())
        {
            PLAYBACK_STATE p;
            currentSong.getPlaybackState(out p);
            if (p == PLAYBACK_STATE.STOPPED)
            {
                NextSong();
            }
        }
    }

    private void OnDestroy()
    {
        currentSong.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        currentSong.release();
    }

    public void NextSong(bool? mandate = null) {
        if (mandate.GetValueOrDefault(loop))
            SongIdx = SongIdx;
        else
            SongIdx++;
    }

    public void RandomSong() {
        SongIdx = Mathf.FloorToInt(Random.value * 100);
    }
}
