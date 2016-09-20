using UnityEngine;
using System.Collections;
using System;

public class LemmingGameLoader : GameLoader
{
    public string m_IPaddress = null;


    protected override IEnumerator LoadingSequence()
    {
        Debug.Log("Hello world!");
        yield return null;

        yield break;
    }
}
