using UnityEngine;
using UnityEditor;

////////////////////////////////////////////////////////////////////////////////
//                      Beskrivelse
//  
//  ##!! Bruges ikke mere. Se DecalObject.cs beskrivelse                    !!##
//  
//  Et editor vindue der gør det nemt at oprette nye decals
//
////////////////////////////////////////////////////////////////////////////////
public class DecalWindow : EditorWindow
{

    ///////////////////////////////
    //      Public Static Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Åbner vores vindue
    /// </summary>
    [MenuItem("Window/Decals")]
    public static void ShowWindow()
    {
        GetWindow<DecalWindow>().Show();
    }

    #endregion

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region

    private string btnCreate = "Add Decal To Scene";
    private Sprite decal;

    #endregion

    ///////////////////////////////
    //      Unity Event
    ///////////////////////////////
    #region

    /// <summary>
    /// Udseende for vores vindue
    /// </summary>
    void OnGUI()
    {
        float marginLeft = this.position.width / 2 - 100 / 2;
        decal = (Sprite)EditorGUI.ObjectField(new Rect(marginLeft, 5, 100, 100), decal, typeof(Sprite), true);

        GUILayout.BeginArea(new Rect(5, 125, this.position.width - 10, 32));
        if (GUILayout.Button(btnCreate))
            Button_CreateDecal_Click();
        GUILayout.EndArea();

    }

    #endregion

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////    
    #region

    /// <summary>
    /// Når man klikker på "Add Decal To Scene" knappen
    /// laver vi et nyt gameobject giver den de Component den skal have
    /// og kalder .BuildDecal() fra DecalObject.cs
    /// </summary>
    private void Button_CreateDecal_Click()
    {
        GameObject go = new GameObject("Decal");
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        var decalObj = go.GetComponent<DecalObject>() ?? go.AddComponent<DecalObject>();

        decalObj.image = decal;
        decalObj.BuildDecal();

        decalObj.offset = 1f;

        Selection.activeGameObject = go;
    }

    #endregion

}
