using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class InputMapping
{
    static private Dictionary<string, List<Func<bool>>> m_map = new Dictionary<string, List<Func<bool>>>(System.StringComparer.Ordinal);

    static public bool GetInput(string key)
    {
        if (m_map.ContainsKey(key) == false)
            return false;
        for (int i = m_map[key].Count - 1; i >= 0; --i)
        {
            if (m_map[key][i].Invoke())
                return true;
        }
        return false;
    }

    static public void RegisterInput(string key, Func<bool> condition)
    {
        if (m_map.ContainsKey(key) == false)
        {
            m_map.Add(key, new List<Func<bool>>());
        }
        m_map[key].Add(condition);
    }
}
