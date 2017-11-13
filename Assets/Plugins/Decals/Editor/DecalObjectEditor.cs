using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(DecalObject))]
public class DecalObjectEditor : Editor
{
    SerializedProperty imageProp;
    SerializedProperty offsetProp;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        imageProp = serializedObject.FindProperty("image");
        offsetProp = serializedObject.FindProperty("offset");
    }


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //DecalObject decal = (DecalObject)target;
        //decal.image = (Sprite)EditorGUILayout.ObjectField("Image", decal.image, typeof(Sprite), false);        
        //decal.offset = EditorGUILayout.Slider("Offset", decal.offset, 0.01f, 0.1f);

        imageProp.objectReferenceValue = EditorGUILayout.ObjectField("Image", imageProp.objectReferenceValue, typeof(Sprite), false);
        EditorGUILayout.Slider(offsetProp, 0.01f, 0.1f, new GUIContent("Offset"));

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            foreach (var item in serializedObject.targetObjects)
            {
                ((DecalObject)item).BuildDecal();
            }
        }

    }
    

    void OnSceneGUI()
    {

        // Gør at vi ikke vælger nyt object når vi holder ctrl nede og klikker på noget
        if (Event.current.control)
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

        //Flyt decal hent til det vi trykket på og vend den rigtigt
        if (Event.current.control  && Event.current.type == EventType.MouseDown)
        {
            DecalObject decal = (DecalObject)target;
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50))
            {
                GameObject moveMe = decal.gameObject;

                //Kopir valgt decal og flyt den hent til hvor vi trykker.
                if (Event.current.shift)
                {
                    moveMe = Instantiate<GameObject>(decal.gameObject);
                    moveMe.name = decal.name;
                }

                //Sæt position
                moveMe.transform.position = hit.point;
                moveMe.transform.forward = hit.normal;
            }
        }
        
    }

   
}
