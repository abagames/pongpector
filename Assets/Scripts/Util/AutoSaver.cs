using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AutoSaver
{
    static AutoSaver()
    {
        EditorApplication.playmodeStateChanged += SaveScene;
    }

    static void SaveScene()
    {
        //if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
        if (EditorApplication.isPlayingOrWillChangePlaymode)
        {
            Debug.Log("Auto-Saving scene before entering Play mode: " + EditorSceneManager.GetActiveScene().name);
            EditorSceneManager.SaveOpenScenes();
            //EditorApplication.SaveAssets();
        }
        EditorApplication.playmodeStateChanged -= SaveScene;
    }
}
