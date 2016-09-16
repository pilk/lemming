using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AssetManifest : ScriptableObject {
	[SerializeField]
	private string myDirectory;
	[SerializeField]
	private List<string> assets;

	public List<string> GetFiles(bool stripExtensions = true, params string[] extensionFilters)
	{
		List<string> results = new List<string>();
		foreach( string asset in assets )
		{
			bool extensionMatch = extensionFilters.Length == 0;
			for( int i = 0; i < extensionFilters.Length; i++ )
			{
				if( asset.EndsWith( extensionFilters[i] ) )
				{
					extensionMatch = true;
					break;
				}
			}

			if( !extensionMatch )
				continue;

			if( stripExtensions )
				results.Add( asset.Remove(asset.LastIndexOf('.')) );
			else
				results.Add(asset);
		}
		return results;
	}
	public bool HasFile(string name, bool removeExtension = true)
	{
		return assets.Find((string file) => {
			if( removeExtension ) {
				int extIndex = file.LastIndexOf('.');
				if( extIndex != -1 )
					return file.Remove(extIndex).Equals( name );
				else
					return file.Equals( name );
			}
			else
				return file.Equals( name );
		}) != null;
	}


	public int CompareFiles(string a, string b)
	{
		int aDepth = 0;
		for( int i = 0; i < a.Length; i++ )
			if( a[i] == '/' )
				aDepth++;
		int bDepth = 0;
		for( int i = 0; i < b.Length; i++ )
			if( b[i] == '/' )
				bDepth++;

		if( aDepth != bDepth )
			return aDepth-bDepth;
		else
			return a.CompareTo(b);
	}

#if UNITY_EDITOR
	public void RebuildManifest()
	{
		List<string> newAssetList = new List<string>();
		string myPath = AssetDatabase.GetAssetPath(this);
		string newMyDirectory = Path.GetDirectoryName( myPath );

		string[] guids = AssetDatabase.FindAssets("t:Object t:ScriptableObject", new string[] { newMyDirectory });
		for( int i = 0; i < guids.Length; i++ )
		{
			string path = AssetDatabase.GUIDToAssetPath(guids[i]);
			if(path != myPath && path.Contains("."))	//ignore folders
				newAssetList.Add( path.Substring(newMyDirectory.Length+1) );	//+1 for the slash
		}
		newAssetList.Sort(CompareFiles);

		bool changed = assets == null || assets.Count != newAssetList.Count;
		for( int i = 0; !changed && i < newAssetList.Count; i++ ) {
			if( newAssetList[i] != assets[i] )
				changed = true;
		}

		if(newMyDirectory.Contains("Assets/"))
		{
			newMyDirectory = newMyDirectory.Substring("Assets/".Length);
		}

		changed |= newMyDirectory != myDirectory;

		if( changed )
		{
			assets = newAssetList;
			myDirectory = newMyDirectory;
			EditorUtility.SetDirty(this);
		}
	}
#endif
}
