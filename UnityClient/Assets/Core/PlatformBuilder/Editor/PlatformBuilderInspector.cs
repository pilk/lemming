using UnityEngine;
using UnityEditor;
using System.Collections;

namespace PlatformBuilder
{
    [CustomEditor(typeof(PlatformBuilder), true)]
    public class PlatformBuilderInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            //PlatformBuilder pb = target as PlatformBuilder;
            EditorGUILayoutHelpers.EditorButton("Perform Button", ()=>{
                PerformBuild(target as PlatformBuilder);
            });
        }

        private void PerformBuild(PlatformBuilder platformBuilder)
        {
            platformBuilder.PrepareForBuild();
            platformBuilder.PerformBuild();
            System.Diagnostics.Process.Start("explorer.exe", "/select," + platformBuilder.outputPath.Replace(@"/", @"\"));// explorer doesn't like front slashes
        }
    }

}