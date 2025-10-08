using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

/// <summary>
/// 资源加载工具类
/// 负责从 Addressables 系统加载和缓存游戏资源，如精灵图片
/// </summary>
public class ResourceLoader : IResourceLoader
{
    private readonly Dictionary<string, Object> cache = new();
    
    public void LoadSprite(string key, Action<Sprite> onLoaded)
    {
        // 若已缓存，直接返回
        if (cache.TryGetValue(key, out var cached))
        {
            onLoaded?.Invoke((Sprite)cached);
            return;
        }

        Addressables.LoadAssetAsync<Sprite>(key).Completed += handle =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                cache[key] = handle.Result;
                onLoaded?.Invoke(handle.Result);
            }
            else
            {
                Debug.LogError($"Failed to load texture: {key}");
            }
        };
    }

    public void Release(string key)
    {
        if (cache.TryGetValue(key, out var cached))
        {
            Addressables.Release(cached);
            cache.Remove(key);
        }
    }
}