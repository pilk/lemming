using UnityEngine;
using System.Collections;
using System;

public class EntityComponent : MonoBehaviour
{
    protected EntityController m_entityController = null;

    protected virtual void Start()
    {
        if (m_entityController == null)
        {
            m_entityController = this.GetComponent<EntityController>();
            if (m_entityController == null)
            {
                m_entityController = this.GetComponentInParent<EntityController>();
            }
        }

        if (m_entityController != null)
        {
            m_entityController.RegisterEventHandler(this.HandleEntityEvent);
        }
    }

    protected virtual void OnDestroy()
    {
        if (m_entityController != null)
        {
            m_entityController.UnregisterEventHandler(this.HandleEntityEvent);
        }
    }

    public void SendEntityEvent(string eventName)
    {
        m_entityController.InvokeEvent(eventName.ToLower());
    }

    protected virtual void HandleEntityEvent(string eventName)
    {
    }
}
