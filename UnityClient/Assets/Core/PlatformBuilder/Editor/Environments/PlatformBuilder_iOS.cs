using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace PlatformBuilder
{
    public class PlatformBuilder_iOS : PlatformBuilder
    {
        public string bundleIdentifier;
        public string bundleVersion;
        public string keyPass;
        public TextAsset plistAsset;

        public override void PerformBuild()
        {
            throw new NotImplementedException();
        }
    }
}