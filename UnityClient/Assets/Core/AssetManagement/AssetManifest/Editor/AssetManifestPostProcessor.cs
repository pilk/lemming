using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AssetManifestPostProcessor : AssetPostprocessor {
	static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) 
	{
#if !RHI_HAS_ASSET_BUNDLES // We don't want to reorganize the manifest automatically when asset bundles are removing files
        string[] manifestPaths = AssetDatabase.FindAssets("t:AssetManifest");
		string[] manifestDirectories = new string[manifestPaths.Length];
		for( int i = 0; i < manifestPaths.Length; i++ ) {
			manifestPaths[i] = AssetDatabase.GUIDToAssetPath( manifestPaths[i] );
			manifestDirectories[i] = Path.GetDirectoryName( manifestPaths[i] );
		}

		HashSet<AssetManifest> manifests = new HashSet<AssetManifest>();
		for( int i = 0; i < importedAssets.Length; i++ )
		{
			string assetPath = Path.GetDirectoryName( importedAssets[i] );

			for( int j = 0; j < manifestPaths.Length; j++ ) {
				if( !assetPath.StartsWith( manifestDirectories[j] ) )
					continue;

				AssetManifest m = AssetDatabase.LoadAssetAtPath( manifestPaths[j], typeof(AssetManifest) ) as AssetManifest;
				if( m != null )
					manifests.Add ( m );
			}
		}
		for( int i = 0; i < deletedAssets.Length; i++ )
		{
			string assetPath = Path.GetDirectoryName( deletedAssets[i] );

			for( int j = 0; j < manifestPaths.Length; j++ ) {
				if( !assetPath.StartsWith( manifestDirectories[j] ) )
					continue;
				
				AssetManifest m = AssetDatabase.LoadAssetAtPath( manifestPaths[j], typeof(AssetManifest) ) as AssetManifest;
				if( m != null )
					manifests.Add ( m );
			}
		}
		for( int i = 0; i < movedAssets.Length; i++ )
		{
			string assetPath = Path.GetDirectoryName( movedAssets[i] );
			
			for( int j = 0; j < manifestPaths.Length; j++ ) {
				if( !assetPath.StartsWith( manifestDirectories[j] ) )
					continue;
				
				AssetManifest m = AssetDatabase.LoadAssetAtPath( manifestPaths[j], typeof(AssetManifest) ) as AssetManifest;
				if( m != null )
					manifests.Add ( m );
			}
		}
		for( int i = 0; i < movedFromAssetPaths.Length; i++ )
		{
			string assetPath = Path.GetDirectoryName( movedFromAssetPaths[i] );
			
			for( int j = 0; j < manifestPaths.Length; j++ ) {
				if( !assetPath.StartsWith( manifestDirectories[j] ) )
					continue;
				
				AssetManifest m = AssetDatabase.LoadAssetAtPath( manifestPaths[j], typeof(AssetManifest) ) as AssetManifest;
				if( m != null )
					manifests.Add ( m );
			}
		}

		foreach( AssetManifest manifest in manifests ) {
			manifest.RebuildManifest();
		}
#endif
	}
}
