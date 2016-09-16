using UnityEngine;
using UnityEditor;
using System.Collections;

namespace PlatformBuilder
{
    public class PlatformBuilderEditor : Editor
    {
        //[MenuItem("Assets/Create/PlatformBuilder/PlatformBuilder")]
        //static public void CreatePlatformBuilder()
        //{
        //    ScriptableObjectUtility.CreateAsset<PlatformBuilder>();
        //}

        [MenuItem("Assets/Create/PlatformBuilder/Android")]
        static public void CreateAndroidPlatformBuilder()
        {
            ScriptableObjectUtility.CreateAsset<PlatformBuilder_Android>();
        }


        [MenuItem("Assets/Create/PlatformBuilder/iOS")]
        static public void CreateiOSPlatformBuilder()
        {
            ScriptableObjectUtility.CreateAsset<PlatformBuilder_iOS>();
        }


        [MenuItem("Assets/Create/PlatformBuilder/PC")]
        static public void CreatePCPlatformBuilder()
        {
            ScriptableObjectUtility.CreateAsset<PlatformBuilder_PC>();
        }
    }
}