using UnityEngine;
// using UnityGoogleDrive;
public class Server_Manager : MonoBehaviour
{

    // public static void Upload_json(string json, string filename)
    // {
    //     var content = System.Text.Encoding.UTF8.GetBytes(json);
    //     var file = new UnityGoogleDrive.Data.File { Name = filename, Content = content };
    //     var request = GoogleDriveFiles.Create(file);
    //     request.Fields = new System.Collections.Generic.List<string> { "id", "name", "size", "createdTime" };
    //     request.Send().OnDone += PrintResult;
    // }
    //
    // private static void PrintResult(UnityGoogleDrive.Data.File file)
    // {
    //     Debug.Log(string.Format("Name: {0} Size: {1:0.00}MB Created: {2:dd.MM.yyyy HH:MM:ss}\nID: {3}",
    //         file.Name,
    //         file.Size * .000001f,
    //         file.CreatedTime,
    //         file.Id));
    // }
    // Update is called once per frame
    void Update()
    {
        
    }
}
