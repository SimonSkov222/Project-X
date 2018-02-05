using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Assets.Plugins.PrefabChanger.Scripts;

////////////////////////////////////////////////////////////////////
//                  Beskrivelse
//
//      Med dette kan vi s�tte mange prefabs ind i en liste
//      hvorefter vi s� kan v�lge en eller flere gameobjecter
//      ogs� sige at de skal skifte ud med det valgte prefab
//      i listen
//
////////////////////////////////////////////////////////////////////
public class PrefabChangerWindow : EditorWindow {


    ///////////////////////////////
    //      Public Static Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// �bner vores vindue
    /// </summary>
    [MenuItem("Window/Prefab Changer")]
    public static void ShowWindow()
    {
        GetWindow<PrefabChangerWindow>("Prefab Changer").Show();
    }

    #endregion

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    #region

    private List<PrefabOption> prefabList = new List<PrefabOption>();
    private GameObject prefab;
    private Vector2 imageSize = new Vector2(100, 100);
    private Vector2 scrollPosition = Vector2.zero;

    #endregion

    ///////////////////////////////
    //      Unity Event
    ///////////////////////////////
    #region

    /// <summary>
    /// Vores vindue udseende
    /// </summary>
    void OnGUI()
    {
        // Skal have en tom plads tilovers 
        if (prefabList.Count == 0 || prefabList[prefabList.Count - 1].prefab != null)
        {
            InsertPrefabOption();
        }

        // Knap til at fjerne alle options
        var clearBtn = new ButtonInfo(-1, "Reset", new Vector2(5, 5), (s) => { prefabList.Clear(); });
        clearBtn.Display(position.width);

        //information der skal ses i vinduet
        EditorGUI.HelpBox(new Rect(5,27, position.width- 10, 30), "Click 'Apply' to change selected Gameobjects.", MessageType.Info);
        EditorGUI.HelpBox(new Rect(5, 59, position.width - 10, 30), "Components will also be changed", MessageType.Warning);

        //G�r at de st�r lige efter hinanden ogs� hvis man fjerner en i midten
        for (int i = 0; i < prefabList.Count; i++)
        {
            prefabList[i].margin = new Vector2(5, 100 * i);
        }

        //Start scroll box
        scrollPosition = GUI.BeginScrollView(new Rect(0, 90, position.width, position.height-90), scrollPosition, new Rect(0, 0, 220, 100* prefabList.Count));

        // Lav en midlertidig liste at da den rigtige kan midste sine option ved at de bliver fjernet
        // hvilket kan f� et for loop til at g� i stykker
        var tempPrefabsList = new List<PrefabOption>();
        tempPrefabsList.AddRange(prefabList);
        
        foreach (var item in tempPrefabsList)
        {
            item.Display(position.width);
        }

        GUI.EndScrollView();
        
    }

    #endregion

    ///////////////////////////////
    //      Private Metods
    ///////////////////////////////
    #region

    /// <summary>
    /// Apply knap. Her udskifter vi de valgte gameobjecter
    /// </summary>
    private void Button_OnMakeChange(object sender)
    {
        //v�r sikker p� at denne knap ikke er tom(prefab)
        if (prefabList.Count(m => m.index == ((ButtonInfo)sender).index) > 0)
        {
            //Hent prefab
            GameObject replacement = prefabList.First(m => m.index == ((ButtonInfo)sender).index).prefab;
            //Selection.activeGameObject = GameObject.Instantiate<GameObject>(replacement);

            //Udskift alle gameobjecterne
            for (int i = Selection.gameObjects.Length - 1; i >= 0; i--)
            {
                //Gamle v�rdier der skal tilf�jes til det nye gameobject(prefab)
                var oldTransform = Selection.gameObjects[i].transform;

                var oldName         = Selection.gameObjects[i].name;
                var tfParent         = oldTransform.parent;
                var tfPosition       = oldTransform.position;
                var tfRotation       = oldTransform.rotation;
                var tfLocalScale     = oldTransform.localScale;

                // var oldComponents = Selection.gameObjects[i].GetComponents<Component>();
                //    Selection.gameObjects[i] = GameObject.Instantiate<GameObject>(replacement);

                //Fjern det gamle gameobject
                GameObject.DestroyImmediate(Selection.gameObjects[i]);
                //Opret nyt gameobject
                var go = GameObject.Instantiate<GameObject>(replacement);

                //Giv det nye gameobject v�rdier fra det gamle
                go.name = oldName;
                go.transform.parent = tfParent;
                go.transform.position = tfPosition;
                go.transform.rotation = tfRotation;
                go.transform.localScale = tfLocalScale;
                    
            }
        }
    }

    /// <summary>
    /// Fjerner valgte option fra listen
    /// </summary>
    private void Button_OnRemove(object sender)
    {
        if (prefabList.Count(m => m.index == ((ButtonInfo)sender).index) > 0)
        {
            prefabList.Remove(prefabList.First(m => m.index == ((ButtonInfo)sender).index));
        }
    }

    /// <summary>
    /// Tilf�jer en ny option til listen
    /// </summary>
    private void InsertPrefabOption()
    {
        prefabList.Add(new PrefabOption(Button_OnMakeChange, Button_OnRemove));
    }

    #endregion
}
