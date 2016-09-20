using UnityEngine;
using System.Collections;
using System.Collections.Generic;

static public class GameSystems
{
    static private readonly Dictionary<System.Type, object> m_systems = new Dictionary<System.Type, object>();

    static public void Register<T>(object target)
    {
        if (m_systems.ContainsKey(typeof(T)))
        {
            DebugUtil.LogError("There is already a type of : " + typeof(T) + " that exists");
        }
        else
        {
            m_systems.Add(typeof(T), target);
        }
    }

    static public T Get<T>()
    {
        object ret = null;
        m_systems.TryGetValue(typeof(T), out ret);
        return (T)ret;
    }
}
