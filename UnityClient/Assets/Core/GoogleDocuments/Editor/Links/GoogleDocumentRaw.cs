using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GoogleDocumentRaw : GoogleDocumentLink<RawGoogleDocumentObject>
{
    public override string format { get { return "csv"; } }

    override public void OnCreate()
    {

    }

#if UNITY_EDITOR
    public override void UpdateData(string data)
    {
        base.UpdateData(data);
        if (this.outputFile == null)
            return;


        Debug.Log("[" + this.name + "] Saving to file : " + AssetDatabase.GetAssetPath(this.outputFile.GetInstanceID()));
        if (this.outputFile == null)
        {
            this.outputFile = (RawGoogleDocumentObject)Resources.Load(this.name);
        }

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(this.outputFile);

        P4Checkout(this);
        this.outputFile.text = data;
        this.outputFile.OnDataUpdate();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
#endif
}
