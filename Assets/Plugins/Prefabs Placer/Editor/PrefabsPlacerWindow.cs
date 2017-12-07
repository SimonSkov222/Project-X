using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//////////////////////////////////////////////////////
//      Beskrivelse
//  
//  Dette er et plugin til unity som ikke har noget med spilet at gøre.
//  Denne klasse gør at vi kan placere mange 
//  prefabs på en engang istedet for at placer 1 af gangen
//  Når man holder musen nede bliver der placeret 
//  prefabs inden for den radius man har valgt
//  Man vælger selv prefabs
//  
//////////////////////////////////////////////////////

public class PrefabsPlacerWindow : EditorWindow {
    

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    private int radius = 25;
    private float space = 4f;
    private bool isEnabled = false;
    private bool isMouseDown = false;
    private GameObject parent;
    private GameObject ground;
    private GameObject ground2;
    private List<Bounds?> prefabsBounds = new List<Bounds?>();
    private List<Vector3?> prefabsPlus = new List<Vector3?>();
    private int prefabsSize = 1;
    private GameObject[] prefabs;
    private GameObject undoParent;


    ///////////////////////////////
    //      Public Static Methods
    ///////////////////////////////


    /// <summary>
    /// MenuItem gør at man kan finde den i værktøjslinien.
    /// Methoden åbener fanen.
    /// </summary>
    [MenuItem("Window/Prefabs Placer")]
    public static void ShowWindow()
    {
        GetWindow<PrefabsPlacerWindow>().Show();

    }

    ///////////////////////////////
    //      Unity Events
    ///////////////////////////////


    /// <summary>
    /// Tilføjer tingene der bliver vist i editor siden
    /// </summary>
    void OnGUI()
    {
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        isEnabled = EditorGUILayout.Toggle("Is Enabled", isEnabled);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        radius = EditorGUILayout.IntSlider("Radius", radius, 5, 100);
        space = EditorGUILayout.Slider("Space Between Prefabs", space, 4f, radius);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        ground2 = (GameObject)EditorGUILayout.ObjectField("Ground", ground2, typeof(GameObject), true);

        parent = (GameObject)EditorGUILayout.ObjectField("Container", parent, typeof(GameObject), true);

        EditorGUILayout.Separator();

        prefabsSize = EditorGUILayout.IntSlider("Number Of Prefabs" ,prefabsSize, 1, 25);
        prefabs = ObjectFieldArray<GameObject>("Prefab", prefabsSize, prefabs);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        EditorGUILayout.HelpBox("Prefabs need a collider", MessageType.Info);


        if (GUI.Button(new Rect(5,5, 100, 32), "Reset Ground"))
        {
            ground2 = null;
        }
        
        if (GUI.changed)
        {
            //ground = null;
            ValidatePrefabs();
        }
    }


    void OnFocus()
    {
        SceneView.onSceneGUIDelegate += OnSceneView;
    }
    

    ///////////////////////////////
    //      Private Event
    ///////////////////////////////
    private void OnSceneView(SceneView scene)
    {
        if (isEnabled)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (Event.current.button == 0 && Event.current.type == EventType.MouseDown && !isMouseDown)
            {
                undoParent = new GameObject("Prefabs Group");
                
                if (parent == null)
                {
                    parent = new GameObject("Prefabs");
                    Undo.RegisterCreatedObjectUndo(parent, "Undo Prefabs");
                }
                else
                {
                    Undo.RegisterCreatedObjectUndo(undoParent, "Undo Prefabs Group");
                }
                undoParent.transform.parent = parent.transform;
                isMouseDown = true;
            }

            if ((Event.current.button == 0 && Event.current.type == EventType.MouseUp) || Event.current.type == EventType.Ignore)
            {
                ground = null;
                isMouseDown = false;

                if (undoParent != null && undoParent.transform.childCount == 0)
                {
                    DestroyImmediate(undoParent);
                }

            }

            RaycastHit hit;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Handles.DrawWireDisc(hit.point, Vector3.up, radius);
                HandleUtility.Repaint();
                
                if (isMouseDown)
                {
                    if (ground == null)
                    {
                        ground = hit.collider.gameObject;
                    }
                    BegindInstantiateGameObject(hit.point);
                }
            }
        }
    }
    

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    private bool HasCollider(GameObject item)
    {
        if (item.GetComponent<Collider>() != null)
            return true;

        for (int i = 0; i < item.transform.childCount; i++)
            if (HasCollider(item.transform.GetChild(i).gameObject))
                return true;

        return false;
    }

    private void BegindInstantiateGameObject(Vector3 center)
    {

        var prefabList = prefabs.Where(m => m != null).ToArray();
        var plusList = prefabsPlus.Where(m => m != null).ToArray();
        var boundsList = prefabsBounds.Where(m => m != null).ToArray();

        int id = Random.Range(0, prefabList.Length);
        
        Vector2 g = new Vector2(center.x, center.z);
        
        PlacePrefab(prefabList[id], boundsList[id].Value, center, plusList[id].Value);
        float radius = space;
        while (radius < this.radius)
        {
            int? degrees = GetDegree(center, radius, space);

            if (degrees.HasValue)
            {
                for (int i = 0; i < 360; i += degrees.Value)
                {
                    id = Random.Range(0, prefabList.Length);
                    PlacePrefab(prefabList[id], boundsList[id].Value, GetPosition(center, radius, i), plusList[id].Value);
                }
            }
            radius += space;
        }
    }


    private void ValidatePrefabs()
    {
        prefabsPlus.Clear();
        prefabsBounds.Clear();
        for (int i = 0; i < prefabs.Length; i++)
        {
            GameObject item = prefabs[i];
            if (item != null && HasCollider(item))
            {
                Vector3 plus;
                prefabsBounds.Add(GetBounds(item, out plus, null));
                prefabsPlus.Add(plus);
            }
            else
            {
                prefabs[i] = null;
                prefabsBounds.Add(null);
                prefabsPlus.Add(null);
            }
        }
    }


    private void PlacePrefab(GameObject prefab, Bounds bounds ,Vector3 position, Vector3 plus)
    {
        GameObject mainGround = ground2 != null ? ground2 : ground;

        RaycastHit hit;
        bool canCreate = true;
        float maxSize = bounds.size.x > bounds.size.z ? bounds.size.x : bounds.size.z;
        if (Physics.Raycast(position + Vector3.up, Vector3.down, out hit, 50))
        {
            position.y = hit.point.y;
            canCreate = hit.collider.gameObject.GetInstanceID() == mainGround.GetInstanceID();
        }
        else if (Physics.Raycast(position + Vector3.down, Vector3.up, out hit, 50))
        {
            position.y = hit.point.y;
            canCreate = hit.collider.gameObject.GetInstanceID() == mainGround.GetInstanceID();
        }
        else
        {
            canCreate = false;
        }

        if (canCreate)
        {
            Collider[] cols = Physics.OverlapSphere(position, space - maxSize / 2 - 2);
            foreach (var item in cols)
            {
                if (item.gameObject.GetInstanceID() != mainGround.GetInstanceID())
                {
                    canCreate = false;
                    break;
                }
            }
        }

        
        if (canCreate)
        {
            GameObject go2 = Instantiate(prefab, position + plus, Quaternion.identity);
            var rotation = go2.transform.rotation;
            rotation *= Quaternion.Euler(0, Random.Range(0, 359), 0); // this adds a 90 degrees Y rotation
            go2.transform.rotation = rotation;
            go2.transform.parent = undoParent.transform;
        }
    }
    
    private Vector3 GetPosition(Vector3 center, float radius, float degree)
    {
        float a = degree * Mathf.PI / 180;

        float x = center.x + radius * Mathf.Cos(a);
        float z = center.z + radius * Mathf.Sin(a);

        return new Vector3(x, center.y ,z); 
    }


    private int? GetDegree(Vector3 center, float radius, float space)
    {
        Vector3 start = GetPosition(center, radius, 0);

        for (int i = 0; i < 360; i++)
        {
            Vector3 check = GetPosition(center, radius, i);
            if (Vector3.Distance(start, check) >= space)
            {
                return i;
            }
        }
        return null;
    }


    private Bounds? GetBounds(GameObject main, out Vector3 plus, Vector3? worldPosition)
    {
        bool isFirst = !worldPosition.HasValue;

        Vector3 position = main.transform.position;
        worldPosition = position;
        
        Vector3 max = new Vector3();
        Vector3 min = new Vector3();

        Bounds? bounds = null;
        Renderer renderer = main.GetComponent<Renderer>();

        bool hasRenderer = renderer != null;
        bool isMaxMinSet = false;

        if (hasRenderer)
        {
            max = renderer.bounds.max;
            min = renderer.bounds.min;
            isMaxMinSet = true;
        }

        for (int i = 0; i < main.transform.childCount; i++)
        {
            Vector3 unUse;
            Bounds? childBounds = GetBounds(main.transform.GetChild(i).gameObject, out unUse, worldPosition);
            if (childBounds.HasValue)
            {
                if (!hasRenderer && !isMaxMinSet)
                {
                    max = childBounds.Value.max;
                    min = childBounds.Value.min;

                    isMaxMinSet = true;
                }
                if (childBounds.Value.max.x > max.x) max.x = childBounds.Value.max.x;
                if (childBounds.Value.max.y > max.y) max.y = childBounds.Value.max.y;
                if (childBounds.Value.max.z > max.z) max.z = childBounds.Value.max.z;
                if (childBounds.Value.min.x < min.x) min.x = childBounds.Value.min.x;
                if (childBounds.Value.min.y < min.y) min.y = childBounds.Value.min.y;
                if (childBounds.Value.min.z < min.z) min.z = childBounds.Value.min.z;
            }
        }
        
        if (isMaxMinSet)
        {
            Bounds defineBounds = new Bounds();
            defineBounds.max = max;
            defineBounds.min = min;
            bounds = defineBounds;
        }
        if (isFirst)
        {
            plus = new Vector3(0, position.y - bounds.Value.min.y, 0);
        }
        else
        {
            plus = Vector3.zero;
        }
        return bounds;
    }


    


    public static T[] ObjectFieldArray<T>(string label, int size, T[] objs) where T : UnityEngine.Object
    {


        if (size > 0)
        {
            if (objs == null)
                objs = new T[size];


            if (size != objs.Length)//resize the array
            {
                //Assuming array isn't enormous in size, just copying the entire array each time should be fine
                T[] newArray = new T[size];
                for (int i = 0; i < size; i++)//Copy whatever we can
                {
                    newArray[i] = i < objs.Length ? objs[i] : null;
                }

                objs = new T[size];
                for (int i = 0; i < size; i++)//Copy back whatever we can
                {
                    objs[i] = newArray[i];
                }
            }


            EditorGUI.indentLevel++;
            for (int i = 0; i < size; i++)
            {

                objs[i] = EditorGUILayout.ObjectField(label+ " " + i, objs[i], typeof(T), true) as T;

            }
            EditorGUI.indentLevel--;

        }
        else
        {
            return null; //Don't return anything
        }

        return objs;
    }


}
