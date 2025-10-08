using System;
using QFramework;

public interface IStorage : IUtility
{
    void SaveInt(string key, int value);
    int LoadInt(string key, int defaultValue = 0);
    void SaveEnum<T>(string key, T value) where T : Enum;
    T LoadEnum<T>(string key, T defaultValue) where T : Enum;
}