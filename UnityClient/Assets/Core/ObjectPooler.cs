using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler<T>
{
    private Queue<T> m_pool = null;
    private System.Func<T> m_creationFunction = null;
    private System.Action<T> m_deletionFunction = null;
    private bool m_initialized = false;

    public bool initialized
    {
        get { return m_initialized; }
    }

    //public ObjectPooler()
    //{

    //}

    public ObjectPooler(int bufferSize, int prebufferSize, System.Func<T> creationFunction, System.Action<T> deletionFunction)
    {
        Init(bufferSize, prebufferSize, creationFunction, deletionFunction);
    }

    public void Init(int bufferSize, int prebufferSize, System.Func<T> creationFunction, System.Action<T> deletionFunction)
    {
        m_creationFunction = creationFunction;
        m_deletionFunction = deletionFunction;
        m_pool = new Queue<T>(bufferSize);
        if (prebufferSize > bufferSize)
            prebufferSize = bufferSize;
        Buffer(prebufferSize);
        m_initialized = true;
    }

    public void Purge()
    {
        m_initialized = false;
        if (m_deletionFunction != null)
        {
            while (m_pool.Count > 0)
            {
                m_deletionFunction.Invoke(m_pool.Dequeue());
            }
        }
        m_pool = null;
    }

    private void Buffer(int count)
    {
        for (int i = count - 1; i >= 0; --i)
        {
            m_pool.Enqueue(m_creationFunction.Invoke());
        }
    }

    public T Request(bool expandPool = false)
    {
        if (m_pool.Count > 0)
        {
            return m_pool.Dequeue();
        }
        else if (expandPool)
        {
            Buffer(1);
            return m_pool.Dequeue();
        }

        return default(T);
    }

    public void Return(T resource)
    {
        if (m_pool.Contains(resource) == false)
        {
            m_pool.Enqueue(resource);
        }
    }
}
