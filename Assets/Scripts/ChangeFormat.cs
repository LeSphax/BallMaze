
using System.IO;
using UnityEngine;

public class ChangeFormat : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log(Application.dataPath);
        LoadAndSave(Paths.DIR_2D);
        LoadAndSave(Paths.DIR_3D);
        //string[] files = Directory.GetFiles(Application.dataPath + "/Resources/LevelFiles/3D");
        //foreach (string file in files)
        //{
        //    string[] split3 = file.Split('.');
        //    string extension = split3[split3.Length - 1];
        //    if (extension != "meta")
        //    {
        //        FileInfo fileInfo = new FileInfo(file);
        //        fileInfo.CopyTo(split3[0]+ ".txt");
        //    }
        //}
    }

    private static void LoadAndSave(string dir)
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Resources/LevelFiles/" + dir);
        foreach (string file in files)
        {
            string[] split1 = file.Split('/');
            string[] split2 = split1[split1.Length - 1].Split('\\');
            string[] split3 = split2[split2.Length - 1].Split('.');
            string extension = split3[split3.Length - 1];
            if (extension != "meta" && extension != "level")
            {
                string fileName = dir + split3[0];
                string serializedData = File.ReadAllText(file);

                LevelData levelData = LevelData.Parse(fileName, serializedData);
                Debug.Log("Create " + Application.dataPath + "/Resources/LevelFiles/" + fileName + ".txt");
                FileInfo fileInfo = new FileInfo(Application.dataPath + "/Resources/LevelFiles/" + fileName + ".txt");

                fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.
                File.WriteAllText(fileInfo.FullName, levelData.Serialize());
            }

        }
    }
}
