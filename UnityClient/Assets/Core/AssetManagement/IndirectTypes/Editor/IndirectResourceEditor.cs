using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class IndirectResourceEditor<T> : PropertyDrawer where T : Object {
	protected const string PATH_PREFIX = "Resources/";

	public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label)
	{
		SerializedProperty pathProp = prop.FindPropertyRelative("resourcePath");
		SerializedProperty guidProp = prop.FindPropertyRelative("assetGuid");
		T asset = null;

		if( pathProp == null || guidProp == null )
			return;

		if( !guidProp.hasMultipleDifferentValues && !string.IsNullOrEmpty(guidProp.stringValue) )
		{
			string assetPath = AssetDatabase.GUIDToAssetPath( guidProp.stringValue );
			asset = AssetDatabase.LoadAssetAtPath( assetPath, typeof(T) ) as T;

			if( asset != null )
			{
				if( !assetPath.StartsWith( pathProp.stringValue ) ) {
					pathProp.stringValue = assetPath.Substring( assetPath.IndexOf(PATH_PREFIX) + PATH_PREFIX.Length );
					pathProp.stringValue = pathProp.stringValue.Substring( 0, pathProp.stringValue.LastIndexOf(".") );	//Remove extension

					prop.serializedObject.ApplyModifiedProperties();
				}
				//EditorUtility.SetDirty(prop.serializedObject.targetObjects);
			}
		}

		EditorGUI.showMixedValue = guidProp.hasMultipleDifferentValues;
		T newAsset = EditorGUI.ObjectField( position, UnityEditor.ObjectNames.NicifyVariableName( prop.name ), asset, typeof(T), false ) as T;
		if( newAsset != asset )
		{
			if( newAsset != null )
			{
				string newAssetPath = AssetDatabase.GetAssetPath(newAsset);
				if( !newAssetPath.Contains(PATH_PREFIX) )
					EditorUtility.DisplayDialog("Error", "" + Path.GetFileNameWithoutExtension(newAssetPath) + " is not in the Resources folder", "OK");
				else
				{
					pathProp.stringValue = newAssetPath.Substring( newAssetPath.IndexOf(PATH_PREFIX) + PATH_PREFIX.Length );
					pathProp.stringValue = pathProp.stringValue.Substring( 0, pathProp.stringValue.LastIndexOf(".") );	//Remove extension
					guidProp.stringValue = AssetDatabase.AssetPathToGUID( newAssetPath );

					asset = newAsset;
				}
			}
			else
			{
				pathProp.stringValue = null;
				guidProp.stringValue = null;

				asset = newAsset;
			}
			prop.serializedObject.ApplyModifiedProperties ();
		}
	}
}
