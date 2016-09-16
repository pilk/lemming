using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetachedDataBlackboard : MonoBehaviour {
    
    public delegate void DataChangedHandler(string key);
    public event DataChangedHandler OnDataChanged = null;

    private Dictionary<string, object> m_dataTable = new Dictionary<string, object>(System.StringComparer.Ordinal);

    public void Refresh()
    {
        if (OnDataChanged != null)
        {
            List<string> keys = new List<string>(m_dataTable.Keys);
            for (int i = keys.Count - 1; i >= 0; --i)
            {
                OnDataChanged.Invoke(keys[i]);
            }
        }
    }

    public void SetValue<T>(string key, T value)
    {
        if (m_dataTable.ContainsKey(key))
        {
            m_dataTable[key] = value;
        }
        else
        {
            m_dataTable.Add(key, value);
        }

        if (OnDataChanged != null)
        {
            OnDataChanged.Invoke(key);
        }
    }

    public T GetValue<T>(string key)
    {
        object ret;
        if (m_dataTable.TryGetValue(key, out ret))
        {
            return (T)ret;
        }
        return default(T);
    }

}
