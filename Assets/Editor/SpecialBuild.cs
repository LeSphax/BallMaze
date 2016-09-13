// C# example.
using UnityEditor;
using UnityEngine;

public class SpecialBuild
{
    [MenuItem("MyTools/Build With PreProcess")]
    public static void BuildGame()
    {        
        Debug.Log(AssetDatabase.CopyAsset("Assets/StreamingAssets/LevelFiles", "Assets/Resources/LevelFiles"));

        // Get filename.
        string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/Cube.unity"};

        // Build player.
#if UNITY_WEBGL
        BuildPipeline.BuildPlayer(levels, path, BuildTarget.WebGL, BuildOptions.None);
#elif UNITY_WP_8_1
        BuildPipeline.BuildPlayer(levels, path, BuildTarget.WP8Player, BuildOptions.None);
#endif
        Debug.Log(AssetDatabase.DeleteAsset("Assets/Resources/LevelFiles"));
        AssetDatabase.Refresh();

        //// Run the game (Process class from System.Diagnostics).
        //Process proc = new Process();
        //proc.StartInfo.FileName = path + "BuiltGame.exe";
        //proc.Start();
    }


}
