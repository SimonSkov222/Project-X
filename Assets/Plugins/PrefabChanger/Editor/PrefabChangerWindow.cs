using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class PrefabChangerWindow : EditorWindow {


    private List<PrefabOption> prefabList = new List<PrefabOption>();
    private GameObject prefab;
    private Vector2 imageSize = new Vector2(100,100);


    [MenuItem("Window/Prefab Changer")]
    public static void ShowWindow()
    {
        GetWindow<PrefabChangerWindow>("Prefab Changer").Show();
    }


    void OnGUI()
    {
        //prefab = (GameObject)EditorGUI.ObjectField(new Rect(new Vector2(5, 150), new Vector2(100, 100)), prefab, typeof(GameObject), true);
        prefabList.Add(new PrefabOption());


        foreach (var item in prefabList)
        {
            item.Display();
        }

        InsertPrefabOption(0, prefab);
    }

    private void InsertPrefabOption(int positionId, GameObject prefab)
    {
        //prefab = (GameObject)EditorGUI.ObjectField(new Rect(new Vector2(5, 150), new Vector2(100, 100)), prefab, typeof(GameObject), true);
        //EditorGUI.DrawPreviewTexture(new Rect(new Vector2(5, 5), new Vector2(100, 100)), AssetPreview.GetAssetPreview(prefab));
    }


    class PrefabOption
    {
        public GameObject prefab;

        private ButtonInfo makeChange;
        private ButtonInfo remove;


        public PrefabOption(GameObject prefab = null)
        {
            this.prefab = prefab;
            this.makeChange = new ButtonInfo("Switch", Rect.zero, null);
            this.remove = new ButtonInfo("Remove", Rect.zero, null);
        }

        public void Display()
        {
            prefab = (GameObject)EditorGUI.ObjectField(new Rect(new Vector2(5, 150), new Vector2(100, 100)), prefab, typeof(GameObject), true);
            EditorGUI.DrawPreviewTexture(new Rect(new Vector2(5, 5), new Vector2(100, 100)), AssetPreview.GetAssetPreview(prefab));

            this.makeChange.isEnabled = prefab != null;
            this.remove.isEnabled = prefab != null;

            this.makeChange.Display();
            this.remove.Display();

        }
    }

    class ButtonInfo
    {
        public delegate void OnClickDelegate(object sender);

        public bool isEnabled;
        public string title;
        public Rect position;
        public OnClickDelegate onClick;


        public ButtonInfo(string title, Rect position, OnClickDelegate onClick)
        {
            this.title = title;
            this.position = position;
            this.onClick = onClick;
            this.isEnabled = true;
        }


        public void Display()
        {
            if (isEnabled)
            {
                if (GUI.Button(position, title))
                {
                    onClick(this);
                }
            }
        }
    }
}
