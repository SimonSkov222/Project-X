using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrefabsPlacerWindow : EditorWindow {

    int radius = 25;
    float prefabsFill = 0.50f;
    bool isEnabled = false;
    bool isMouseDown = false;
    List<int> prefabIds = new List<int>();
    GameObject parent;

    int prefabsSize = 1;
    GameObject[] prefabs;


    [MenuItem("Window/Prefabs Placer")]
    public static void ShowWindow()
    {
        GetWindow<PrefabsPlacerWindow>().Show();
        //SceneView.onSceneGUIDelegate += 

    }

    void OnGUI()
    {
        EditorGUILayout.Separator();
        isEnabled = EditorGUILayout.Toggle("Is Enabled", isEnabled);

        //radius = GUILayout.HorizontalSlider(radius, 25, 100);
        //Debug.Log(radius);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        radius = EditorGUILayout.IntSlider("Radius", radius, 5, 100);
        prefabsFill = EditorGUILayout.Slider("Fill", prefabsFill, 0.01f, radius);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        
        prefabsSize = EditorGUILayout.IntSlider("Number Of Prefabs" ,prefabsSize, 1, 25);
        prefabs = ObjectFieldArray<GameObject>(prefabsSize, prefabs);

        parent = (GameObject)EditorGUILayout.ObjectField("Parent", parent, typeof(GameObject), true);

    }

    bool debug = false;
    private void OnSceneView(SceneView scene)
    {
        if (isEnabled)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            if (Event.current.button == 0 && Event.current.type == EventType.MouseDown)
            {
                isMouseDown = true;
                debug = true;
            }

            if ((Event.current.button == 0 && Event.current.type == EventType.MouseUp) || Event.current.type == EventType.Ignore)
            {
                prefabIds.Clear();
                isMouseDown = false;
            }

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit[] hits = Physics.RaycastAll(ray);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.tag != "pp")
                {
                    Handles.DrawWireDisc(hits[i].point, Vector3.up, radius);
                    HandleUtility.Repaint();

                    if (isMouseDown)
                    {
                        test(hits[i].point);
                        debug = false;
                    }

                    break;
                }
            }
        }
    }

    private void test(Vector3 center)
    {
        
        
        Vector3 plus;
        Bounds? bounds = GetBounds(prefabs[0], out plus, null);
        
        Vector2 g = new Vector2(center.x, center.z);
        if (parent == null)
        {
            parent = new GameObject("prefab");
        }
        PlacePrefab(prefabs[0], bounds.Value, center, plus);
        float radius = prefabsFill;
        while (radius < this.radius)
        {
            int? degrees = GetDegree(center, radius, prefabsFill);

            if (degrees.HasValue)
            {
                for (int i = 0; i < 360; i += degrees.Value)
                {
                    PlacePrefab(prefabs[0], bounds.Value, GetPosition(center, radius, i), plus);
                }
            }
            radius += prefabsFill;
        }
        
        
        
    }


    private void PlacePrefab(GameObject prefab, Bounds bounds ,Vector3 position, Vector3 plus)
    {

        RaycastHit hit;
        bool canCreate = true;
        float maxSize = bounds.size.x > bounds.size.z ? bounds.size.x : bounds.size.z;

        if (Physics.Raycast(position, Vector3.down, out hit, 50))
        {
            if (!prefabIds.Contains(hit.collider.gameObject.GetInstanceID()))
            {
                position.y = hit.point.y;
                canCreate = true;
            }
            else
            {
                canCreate = false;
            }
        }
        else if (Physics.Raycast(position, Vector3.up, out hit, 50))
        {
            if (!prefabIds.Contains(hit.collider.gameObject.GetInstanceID()))
            {
                position.y = hit.point.y;
                canCreate = true;
            }
            else
            {
                canCreate = false;
            }
        }
        else
        {
            canCreate = false;
        }

        if (canCreate)
        {
            Collider[] cols = Physics.OverlapSphere(position, prefabsFill - maxSize / 2 - 2);
            foreach (var item in cols)
            {
                if (prefabIds.Contains(item.gameObject.GetInstanceID()))
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

            go2.transform.parent = parent.transform;
            prefabIds.Add(go2.GetInstanceID());
        }

        Handles.color = Color.red;
        Handles.DrawWireDisc(position, Vector3.up, prefabsFill - maxSize / 2 - 2);

        HandleUtility.Repaint();
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


    void OnFocus()
    {
        SceneView.onSceneGUIDelegate += OnSceneView;
    }


    public static T[] ObjectFieldArray<T>(int size, T[] objs) where T : UnityEngine.Object
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

                objs[i] = EditorGUILayout.ObjectField("Element " + i, objs[i], typeof(T), true) as T;

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
