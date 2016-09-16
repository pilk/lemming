using UnityEngine;
using UnityEditor;
using System.Collections;

public static class EditorGUILayoutHelpers 
{
    public static void ApplicationButton(string text, System.Action onSuccessCallback = null, System.Action onFailureCallback = null)
    {
        bool buttonPressed = false;
        using (new LayoutHelper.ApplicationPlaying())
        {
            buttonPressed = GUILayout.Button(text);
        }

        if (buttonPressed)
        {
            if (Application.isPlaying)
            {
                if (onSuccessCallback != null)
                    onSuccessCallback.Invoke();
            }
            else
            {
                if (onFailureCallback != null )
                    onFailureCallback.Invoke();
                EditorUtility.DisplayDialog
                (
                    "Error",
                    "This is only allowed in Play Mode!",
                    "OK"
                );
            }
        }
    }

    public static void EditorButton(string text, System.Action onSuccessCallback = null, System.Action onFailureCallback = null)
    {
        bool buttonPressed = false;
        using (new LayoutHelper.ApplicationPlaying(false))
        {
            buttonPressed = GUILayout.Button(text);
        }

        if (buttonPressed)
        {
            if (Application.isPlaying == false)
            {
                if (onSuccessCallback != null)
                    onSuccessCallback.Invoke();
            }
            else
            {
                if (onFailureCallback != null)
                    onFailureCallback.Invoke();
                EditorUtility.DisplayDialog
                (
                    "Error",
                    "This is only allowed while the Application is not running!",
                    "OK"
                );
            }
        }
    }
}
