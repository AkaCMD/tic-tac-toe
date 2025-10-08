using System;
using QFramework;
using UnityEngine;

public interface IResourceLoader : IUtility
{
    void LoadSprite(string key, Action<Sprite> onLoaded);
    void Release(string key);
}