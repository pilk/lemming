using UnityEngine;
using System.Collections;
using System;
using UnityEditor;

namespace PlatformBuilder
{
    public class PlatformBuilder_Android : PlatformBuilder
    {
        public string bundleIdentifier;
        public string bundleVersion;
        public int bundleVersionCode;
        public string keyPass;
        public string keyStoreName;
        public string keyAliasName;
        public bool useAPKExpansionFiles = false;
        public TextAsset androidManifest;

        public override void PerformBuild()
        {
            BuildPipeline.BuildPlayer(GetScenePaths(), buildLocationPath + ".apk", this.buildTarget, this.buildOptions);
        }
    }
}