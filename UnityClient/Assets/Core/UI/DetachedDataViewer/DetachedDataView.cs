using UnityEngine;
using System.Collections;
using System;

public class DetachedDataView : MonoBehaviour
{
    public string m_dataKey = null;

    protected DetachedDataBlackboard m_blackboard = null;

    protected virtual void OnEnable()
    {
        m_blackboard = this.GetComponentInParent<DetachedDataBlackboard>();
        if (m_blackboard == null)
        {
            this.enabled = false;
            return;
        }

        m_blackboard.OnDataChanged += this.HandleDataChanged;
    }

    protected virtual void OnDisable()
    {
        m_blackboard.OnDataChanged -= this.HandleDataChanged;
    }

    protected void HandleDataChanged(string key)
    {
    }
}
