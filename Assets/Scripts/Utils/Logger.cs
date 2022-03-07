using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static void Log(string str)
    {
        if (Application.isEditor)
        {
            Debug.Log(str);
        } else
        {
            Debug.LogError(str);
        }
    }
}
