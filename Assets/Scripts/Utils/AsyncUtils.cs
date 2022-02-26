using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public static class AsyncUtils
{
    public static void DelayLaunch<T>(this T self, Action action, float seconds, bool realTime = false) where T:MonoBehaviour {
        self.StartCoroutine(DelayLaunch(action, seconds, realTime));
    }

    static IEnumerator DelayLaunch(Action action, float seconds, bool realTime)
    {
        if (realTime)
        {
            yield return new WaitForSecondsRealtime(seconds);
        }
        else
        {
            yield return new WaitForSeconds(seconds);
        }
        
        action();
    }
}
