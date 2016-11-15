// C# example.
using System;
using UnityEditor;
using UnityEngine;

public class SpecialBuild
{

    [MenuItem("MyTools/Build With PreProcess")]
    public static void BuildGame()
    {
        AssetDatabase.MoveAsset(Levels.StreamingAssetsPath, Levels.ResourcesPath);
        AssetDatabase.SaveAssets();

        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/Menu.unity", "Assets/2DGame.unity", "Assets/3DGame.unity" };
        try
        {
            // Build player.
#if UNITY_WEBGL
        Debug.Log(BuildPipeline.BuildPlayer(levels, path, BuildTarget.WebGL, BuildOptions.None));
#elif UNITY_WEBPLAYER
            Debug.Log(BuildPipeline.BuildPlayer(levels, path, BuildTarget.WebPlayer, BuildOptions.None));
#else
        Debug.Log(BuildPipeline.BuildPlayer(levels, path, BuildTarget.WSAPlayer, BuildOptions.None));
#endif
        }
        catch (Exception)
        {
            AssetDatabase.MoveAsset(Levels.StreamingAssetsPath, Levels.ResourcesPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        //// Run the game (Process class from System.Diagnostics).
        //Process proc = new Process();
        //proc.StartInfo.FileName = path + "BuiltGame.exe";
        //proc.Start();
    }

}
