using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class IndirectResource<T> where T : Object
{
    public static Dictionary<string, int> loadCounts = new Dictionary<string, int>();   //Key is path
    public static event System.Action<string> LoadCountChanged;

    public string resourcePath;
    public string assetGuid;	//Only needed in editor

    public string assetName
    {
        get
        {
            if (!string.IsNullOrEmpty(resourcePath))
            {
                string[] splits = resourcePath.Split('/');
                return splits[splits.Length - 1];
            }
            return null;
        }
    }

    [System.NonSerialized]
    private T loadedResource = null;

    private int LoadCount
    {
        get
        {
            if (loadCounts.ContainsKey(resourcePath))
                return loadCounts[resourcePath];
            else
                return 0;
        }
        set
        {
            if (value > 0)
                loadCounts[resourcePath] = value;
            else if (loadCounts.ContainsKey(resourcePath))
                loadCounts.Remove(resourcePath);

            if (LoadCountChanged != null)
                LoadCountChanged(resourcePath);
        }
    }
    //	[System.NonSerialized]
    //	private int loadRequests = 0;

    public bool Empty
    {
        get
        {
            return string.IsNullOrEmpty(resourcePath);
        }
    }

    public T LoadedResource
    {
        get
        {
            if (LoadCount == 0)
            {
                DebugUtil.LogWarning("Attempting to get " + System.IO.Path.GetFileName(resourcePath) + " without loading it first");
            }
            else if (!Loaded)
            {
                DebugUtil.LogError("Resource: " + resourcePath + " wasn't loaded correctly!");
            }
            return loadedResource;
        }
    }

    public bool Loaded
    {
        get
        {
            return loadedResource != null;
        }
    }

    public IndirectResource(string path = null)
    {
        resourcePath = path;
    }

    public bool Load()
    {
        LoadCount++;
        //		loadRequests = count;

        if (LoadCount > 0 && !Loaded)   //Because load count is shared, it can be >1 and this instance still needs loading
        {
#if RHI_INDIRECT_LOAD_ASSETBUNDLES
            UnityEngine.Object obj = ResourcesFacade.FindAnyCachedObjectFromAssetBundle(resourcePath);
            if (obj != null)
            {
                loadedResource = (T)obj;
            }
            else
            {
                loadedResource = Resources.Load<T>(resourcePath);
            }
#else
            loadedResource = Resources.Load<T>(resourcePath);
#endif
            if (loadedResource == null)
                DebugUtil.LogError("Couldn't load " + resourcePath);
            //Debug.Log("Loaded " + loadedResource.name + " from " + resourcePath);
        }
        return Loaded;
    }
    public void Unload()
    {
        --LoadCount;
        if (LoadCount < 0)
        {
            DebugUtil.LogWarning("Too many unload requests on " + resourcePath);
            LoadCount = 0;
        }
        //		loadRequests = count;

        if (LoadCount == 0 && Loaded)
        {
            // Cannot unload prefabs
            if (!(loadedResource is GameObject))
            {
#if RHI_INDIRECT_LOAD_ASSETBUNDLES
                UnityEngine.Object obj = ResourcesFacade.FindAnyCachedObjectFromAssetBundle(resourcePath);
                if (obj == null)
                {
                    Resources.UnloadAsset(loadedResource);
                }
#else
                Resources.UnloadAsset(loadedResource);
#endif
            }
            loadedResource = null;
        }
    }
}
