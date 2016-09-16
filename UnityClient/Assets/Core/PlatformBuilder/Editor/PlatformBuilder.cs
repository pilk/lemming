using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Security.AccessControl;

namespace PlatformBuilder
{
    public abstract class PlatformBuilder : ScriptableObject
    {
        public const string MENU_ITEM_DIRECTORY = "Assets/Create/AutoBuilder/";

        public string productName;
        public string[] scriptDefineSymbols;
        public BuildOptions buildOptions;
        public BuildTarget buildTarget;


        public string outputPath
        {
            get { return Application.dataPath.Replace("Assets", "") + "Builds/" + buildTarget + " " + projectName + "/"; }
        }

        public virtual string projectName
        {
            get { return string.Format("[{0}][{1}]", this.productName, this.name); }
        }


        public string buildLocationPath
        {
            get { return outputPath + projectName; }
        }





        public abstract void PerformBuild();

        public void PrepareForBuild()
        {
            BuildTargetGroup btg = GetBuildTargetGroup(this.buildTarget);

            // Name and bundleID
            PlayerSettings.productName = this.productName;

            // Scripting define symbols
            PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, "");
            for (int i = this.scriptDefineSymbols.Length - 1; i >= 0; --i)
            {
                string s = PlayerSettings.GetScriptingDefineSymbolsForGroup(btg);
                if (s.Contains(this.scriptDefineSymbols[i]) == true)
                {
                    // already there
                    continue;
                }
                if (string.IsNullOrEmpty(s) == false)
                {
                    s += " ";
                }
                s += this.scriptDefineSymbols[i];
                PlayerSettings.SetScriptingDefineSymbolsForGroup(btg, s);
            }

            BuildTarget sbt = this.buildTarget;
            if (sbt == BuildTarget.StandaloneWindows64)
                sbt = BuildTarget.StandaloneWindows;
            EditorUserBuildSettings.SwitchActiveBuildTarget(sbt);

            //AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            System.IO.Directory.CreateDirectory(outputPath);
        }

        protected string[] GetScenePaths()
        {
            string[] scenePaths = new string[EditorBuildSettings.scenes.Length];
            for (int i = 0; i < scenePaths.Length; i++)
            {
                scenePaths[i] = EditorBuildSettings.scenes[i].path;
            }
            return scenePaths;
        }

        
        protected BuildTargetGroup GetBuildTargetGroup(BuildTarget bt)
        {
            switch (bt)
            {
                case BuildTarget.StandaloneWindows: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneWindows64: return BuildTargetGroup.Standalone;
                case BuildTarget.StandaloneOSXIntel: return BuildTargetGroup.Standalone;
                case BuildTarget.Android: return BuildTargetGroup.Android;
                case BuildTarget.iOS: return BuildTargetGroup.iOS;
                case BuildTarget.WebPlayer: return BuildTargetGroup.WebPlayer;
                case BuildTarget.WebPlayerStreamed: return BuildTargetGroup.WebPlayer;
            }
            return BuildTargetGroup.Unknown;
        }
    };
}
