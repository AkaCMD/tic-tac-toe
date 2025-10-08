using System;
using UnityEngine;

public class Storage : IStorage 
{
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    public void SaveEnum<T>(string key, T value) where T : Enum
    {
        PlayerPrefs.SetInt(key, Convert.ToInt32(value));
    }

    public T LoadEnum<T>(string key, T defaultValue) where T : Enum
    {
        int intValue = PlayerPrefs.GetInt(key, Convert.ToInt32(defaultValue));
        return (T)Enum.ToObject(typeof(T), intValue);
    }
}
