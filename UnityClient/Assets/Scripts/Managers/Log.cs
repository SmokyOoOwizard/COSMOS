using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public static class Log
{
    public static void Info(object obj)
    {
        Debug.Log(obj);
    }
    public static void Info(object obj, params string[] tags)
    {
        Debug.Log(obj);
    }
    public static void Warning(object obj)
    {
        Debug.LogWarning(obj);
    }
    public static void Warning(object obj, params string[] tags)
    {
        Debug.LogWarning(obj);
    }
    public static void Error(object obj)
    {
        Debug.LogError(obj);
    }
    public static void Error(object obj, params string[] tags)
    {
        Debug.LogError(obj);
    }

    public static void Panic(object obj)
    {
        Debug.LogError(obj);
    }

    internal static void Panic(object obj, params string[] tags)
    {
        Debug.LogError(obj);
    }
}
