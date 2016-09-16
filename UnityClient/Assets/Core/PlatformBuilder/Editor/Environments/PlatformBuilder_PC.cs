using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

namespace PlatformBuilder
{
    public class PlatformBuilder_PC : PlatformBuilder
    {
        public override void PerformBuild()
        {
            BuildPipeline.BuildPlayer(GetScenePaths(), buildLocationPath + ".exe", this.buildTarget, this.buildOptions);
        }
    }
}