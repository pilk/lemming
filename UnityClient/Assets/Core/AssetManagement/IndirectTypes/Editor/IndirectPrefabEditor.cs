using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[CustomPropertyDrawer(typeof(IndirectPrefab))]
public class IndirectPrefabEditor : IndirectResourceEditor<GameObject> {
	public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label)
	{
		base.OnGUI(position, prop, label);
	}
}
