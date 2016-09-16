using UnityEngine;
using UnityEditor;
using System.Collections;

public class AssetManifestAsset : MonoBehaviour {
	[MenuItem("Assets/Create/Asset Manifest")]
	public static void CreateAssetManifest() {
		AssetManifest manifest = ScriptableObjectUtility.CreateAsset<AssetManifest>("-Manifest");
		manifest.RebuildManifest();
		EditorUtility.SetDirty(manifest);
	}
}
