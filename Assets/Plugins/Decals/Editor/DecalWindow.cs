using UnityEngine;
using UnityEditor;

public class DecalWindow : EditorWindow
{

    static DecalWindow win;

    string btnCreate = "Add Decal To Scene";
    
    Sprite decal;


    [MenuItem("Window/Decals")]
    public static void ShowWindow() {
        win = GetWindow<DecalWindow>();
        win.Show();
    }
    
    void OnGUI()
    {

        float marginLeft = this.position.width / 2 - 100 / 2;
        decal = (Sprite)EditorGUI.ObjectField(new Rect(marginLeft, 5, 100, 100), decal, typeof(Sprite), true);

        GUILayout.BeginArea(new Rect(5, 125, this.position.width - 10, 32));
        if (GUILayout.Button(btnCreate))
            Button_CreateDecal_Click();
        GUILayout.EndArea();
                
    }
    

    private void Button_CreateDecal_Click()
    {        
        GameObject go = new GameObject("Decal");
        go.AddComponent<MeshFilter>();
        go.AddComponent<MeshRenderer>();
        var decalObj = go.GetComponent<DecalObject>() ?? go.AddComponent<DecalObject>();
        
        decalObj.image = decal;
        decalObj.BuildDecal();

        Selection.activeGameObject = go;
    }
}
