using UnityEngine;
using System.Collections;

[System.Serializable]
public class IndirectTexture : IndirectResource<Texture> {
	public IndirectTexture(string path = null) : base(path) {}
}
