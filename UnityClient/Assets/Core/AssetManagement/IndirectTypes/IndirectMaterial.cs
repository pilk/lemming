using UnityEngine;
using System.Collections;

[System.Serializable]
public class IndirectMaterial : IndirectResource<Material>  {
    public IndirectMaterial(string path = null) : base(path) { }
}