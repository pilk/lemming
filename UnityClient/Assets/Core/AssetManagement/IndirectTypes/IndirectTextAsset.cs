using UnityEngine;
using System.Collections;

[System.Serializable]
public class IndirectTextAsset : IndirectResource<TextAsset> {
	public IndirectTextAsset(string path = null) : base(path) {}
}
