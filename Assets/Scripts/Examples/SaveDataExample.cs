using UnityEngine;

////////////////////////////////////////////////////////////////////
//      Beskrivelse
//
//  Dette er kun et eksempel på hvordan man kan bruge SaveDataHelper
//
////////////////////////////////////////////////////////////////////
public class SaveDataExample : MonoBehaviour {


    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    public string playerName = "Simon Skov";
    public int level = 21;
    public string description = "Professional Programmer";


    ///////////////////////////////
    //      Unity Methods
    ///////////////////////////////

    /// <summary>
    /// Laver nogle input når man startet spillet
    /// og viser hvordan SaveDataHelper bruges.
    /// </summary>
    void OnGUI()
    {
        GUILayout.Space(5f);
        GUILayout.Label(" Save Folder:");
        GUILayout.TextField(Application.persistentDataPath);

        GUILayout.Space(10f);
        GUILayout.Label(" Player Name:");
        playerName = GUILayout.TextField(playerName);
        GUILayout.Label(" Player Level:");
        string levelStr = GUILayout.TextField(level.ToString());
        GUILayout.Label(" Player Description:");
        description = GUILayout.TextField(description);
        
        int.TryParse(levelStr, out level);

        GUILayout.Space(10f);


        //Hvordan SaveDataHelper kan bruges
        if (GUILayout.Button("Save"))
        {
            SaveDataHelper.PutValue("SaveDataExample", "Name", playerName);
            SaveDataHelper.PutValue("SaveDataExample", "Level", level);
            SaveDataHelper.PutValue("SaveDataExample", "Description", description);
            SaveDataHelper.SaveFile("SaveDataExample");
        }
        if (GUILayout.Button("Load"))
        {
            playerName = SaveDataHelper.GetValue<string>("SaveDataExample", "Name");
            level = SaveDataHelper.GetValue<int>("SaveDataExample", "Level");
            description = SaveDataHelper.GetValue<string>("SaveDataExample", "Description");
        }
    }
}
