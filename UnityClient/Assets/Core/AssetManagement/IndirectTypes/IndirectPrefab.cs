using UnityEngine;
using System.Collections;

[System.Serializable]
public class IndirectPrefab : IndirectResource<GameObject> {
    public IndirectPrefab(string path = null) : base(path) { }
}
