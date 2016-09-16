using UnityEngine;
using System.Collections;
using System;

public class LemmingGameLoader : GameLoader
{
    protected override IEnumerator LoadingSequence()
    {
        yield return null;

        yield break;
    }
}
