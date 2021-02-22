//#define UNITY_LOG

using UnityEngine;


public static class ConditionalLogger
{
    public static void Log(string message, Object context = null)
    {
#if UNITY_LOG
        Debug.Log(message, context);
#endif
    }

    public static void LogWarning(string message, Object context = null)
    {
#if UNITY_LOG
        Debug.LogWarning(message, context);
#endif
    }

    public static void LogError(string message, Object context = null)
    {
#if UNITY_LOG
        Debug.LogError(message, context);
#endif
    }
}
