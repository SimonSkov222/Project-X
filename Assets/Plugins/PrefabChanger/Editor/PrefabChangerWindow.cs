using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

public class PrefabChangerWindow : EditorWindow {


    private List<PrefabOption> prefabList = new List<PrefabOption>();
    private GameObject prefab;
    private Vector2 imageSize = new Vector2(100, 100);
    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Window/Prefab Changer")]
    public static void ShowWindow()
    {
        GetWindow<PrefabChangerWindow>("Prefab Changer").Show();
    }


    void OnGUI()
    {
        if (prefabList.Count == 0 || prefabList[prefabList.Count - 1].prefab != null)
        {
            InsertPrefabOption();
        }

        var clearBtn = new ButtonInfo(-1, "Reset", new Vector2(5, 5), (s) => { prefabList.Clear(); });
        clearBtn.Display(position.width);

        EditorGUI.HelpBox(new Rect(5,27, position.width- 10, 30), "Click 'Apply' to change selected Gameobjects.", MessageType.Info);
        EditorGUI.HelpBox(new Rect(5, 59, position.width - 10, 30), "Components will also be changed", MessageType.Warning);

        for (int i = 0; i < prefabList.Count; i++)
        {
            prefabList[i].margin = new Vector2(5, 100 * i);
        }

        var tempPrefabsList = new List<PrefabOption>();
        tempPrefabsList.AddRange(prefabList);

        scrollPosition = GUI.BeginScrollView(new Rect(0, 90, position.width, position.height-90), scrollPosition, new Rect(0, 0, 220, 100* prefabList.Count));

        foreach (var item in tempPrefabsList)
        {
            item.Display(position.width);
        }
        GUI.EndScrollView();
        
    }

    private void Button_OnMakeChange(object sender)
    {
        if (prefabList.Count(m => m.index == ((ButtonInfo)sender).index) > 0)
        {
            GameObject replacement = prefabList.First(m => m.index == ((ButtonInfo)sender).index).prefab;
            //Selection.activeGameObject = GameObject.Instantiate<GameObject>(replacement);
            for (int i = Selection.gameObjects.Length - 1; i >= 0; i--)
            {
                var oldTransform = Selection.gameObjects[i].transform;

                var oldName         = Selection.gameObjects[i].name;
               var tfParent         = oldTransform.parent;
               var tfPosition       = oldTransform.position;
               var tfRotation       = oldTransform.rotation;
               var tfLocalScale     = oldTransform.localScale;

                var oldComponents = Selection.gameObjects[i].GetComponents<Component>();
                //    Selection.gameObjects[i] = GameObject.Instantiate<GameObject>(replacement);
                GameObject.DestroyImmediate(Selection.gameObjects[i]);
                var go = GameObject.Instantiate<GameObject>(replacement);
                go.name = oldName;
                go.transform.parent = tfParent;
                go.transform.position = tfPosition;
                go.transform.rotation = tfRotation;
                go.transform.localScale = tfLocalScale;
                    
            }
        }
    }
    private void Button_OnRemove(object sender)
    {
        if (prefabList.Count(m => m.index == ((ButtonInfo)sender).index) > 0)
        {
            prefabList.Remove(prefabList.First(m => m.index == ((ButtonInfo)sender).index));
        }
    }


    private void InsertPrefabOption()
    {

        prefabList.Add(new PrefabOption(Button_OnMakeChange, Button_OnRemove));
    }

    

    class PrefabOption
    {
        private static int indexCounter = 0;
        public int index;
        public GameObject prefab;

        private ButtonInfo makeChange;
        private ButtonInfo remove;

        public Vector2 margin;


        public PrefabOption(ButtonInfo.OnClickDelegate onMakeChange = null, ButtonInfo.OnClickDelegate onRemove = null)
        {
            index = ++indexCounter;
            this.margin = Vector2.zero;
            this.prefab = null;
            this.makeChange = new ButtonInfo(index, "Apply", new Vector2(115, 55), onMakeChange);
            this.remove = new ButtonInfo(index,"Remove", new Vector2(115, 80), onRemove);
        }

        public void Display(float width)
        {
            float widthWithoutMargin = width - margin.x * 2;
            GUI.BeginGroup(new Rect(margin, new Vector2(widthWithoutMargin, 100)));

            prefab = (GameObject)EditorGUI.ObjectField(new Rect(115, 5, widthWithoutMargin - 115, 18), prefab, typeof(GameObject), true);
            var picRect = new Rect(5, 5, 100, 100);
            if (prefab != null)
            {
                EditorGUI.DrawPreviewTexture(picRect, AssetPreview.GetAssetPreview(prefab));
            }
            else
            {
                EditorGUI.DrawRect(picRect, Color.white); ;
            }
            //this.makeChange.isEnabled = prefab != null;
            //this.remove.isEnabled = prefab != null;

            this.makeChange.Display(widthWithoutMargin - 115);
            this.remove.Display(widthWithoutMargin - 115);
            GUI.EndGroup();
        }
    }

    public class ButtonInfo
    {
        public delegate void OnClickDelegate(object sender);
        public int index;
        public bool isEnabled;
        public string title;
        public Vector2 margin;
        public OnClickDelegate onClick;


        public ButtonInfo(int id, string title, Vector2 margin, OnClickDelegate onClick)
        {
            this.index = id;
            this.title = title;
            this.margin = margin;
            this.onClick = onClick;
            this.isEnabled = true;
        }


        public void Display(float width)
        {
            if (isEnabled)
            {
                if (GUI.Button(new Rect(margin, new Vector2(width - 10, 20)), title))
                {
                    onClick(this);
                }
            }
        }
    }
}
