using UnityEngine;
using System.Collections;

public class EntityHealth : EntityComponent
{
    public int m_healthPoints = 1;

    public void TakeDamage(int damage)
    {
        m_entityController.InvokeEvent("damage_received");
    }
}
