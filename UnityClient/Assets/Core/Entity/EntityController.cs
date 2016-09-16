using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityController : MonoBehaviour
{
    public delegate void EventHandler(string eventName);

    private event EventHandler m_eventHandlers = null;

    private Dictionary<string, EventHandler> m_eventHandlersByEventName = new Dictionary<string, EventHandler>(System.StringComparer.Ordinal);


    static private Dictionary<int, EntityController> s_instances = new Dictionary<int, EntityController>(32);

    static public EntityController Get(int gameObjectID)
    {
        if( s_instances.ContainsKey(gameObjectID))
        {
            return s_instances[gameObjectID];
        }
        return null;
    }

    private void Awake()
    {
        s_instances.Add(this.gameObject.GetInstanceID(), this);
    }

    private void OnDestroy()
    {
        s_instances.Remove(this.gameObject.GetInstanceID());
    }

    public void RegisterEventHandler(EventHandler eventHandler)
    {
        m_eventHandlers += eventHandler;
    }

    public void UnregisterEventHandler(EventHandler eventHandler)
    {
        m_eventHandlers -= eventHandler;
    }

    public void RegisterEventHandler(EventHandler eventHandler, string eventName)
    {
        if (m_eventHandlersByEventName.ContainsKey(eventName) == false)
            m_eventHandlersByEventName.Add(eventName, null);
        m_eventHandlersByEventName[eventName] += eventHandler;
    }

    public void UnregisterEventHandler(EventHandler eventHandler, string eventName)
    {
        if (m_eventHandlersByEventName.ContainsKey(eventName))
        {
            m_eventHandlersByEventName[eventName] -= eventHandler;
        }
    }

    public void InvokeEvent(string eventName)
    {
        if (m_eventHandlers != null)
        {
            m_eventHandlers.Invoke(eventName);
        }

        if (m_eventHandlersByEventName.ContainsKey(eventName))
        {
            m_eventHandlersByEventName[eventName].Invoke(eventName);
        }
    }
}
