using UnityEngine;
using UnityEditor;

////////////////////////////////////////////////////////////////////////////////
//                      Beskrivelse
//  
//  ##!! Bruges ikke mere. Se DecalObject.cs beskrivelse                    !!##
//
//  Vi har lavet dettet CustomEditor så vi nemt kan flytte decal (CTRL + Klik)
//  og så vi se et billedet af decalet og at vi kan ændre flere på en gang
//
////////////////////////////////////////////////////////////////////////////////
[CanEditMultipleObjects]
[CustomEditor(typeof(DecalObject))]
public class DecalObjectEditor : Editor
{
    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region

    private SerializedProperty imageProp;
    private SerializedProperty offsetProp;

    #endregion

    ///////////////////////////////
    //      Unity Event
    ///////////////////////////////
    #region

    /// <summary>
    /// Når scriptet bliver aktiv finder vi de properties
    /// der skal kunne blive ændres og gemmer dem så vi ikke 
    /// skal bruge resources på at findes dem hele tiden.
    /// </summary>
    void OnEnable()
    {
        // Setup the SerializedProperties.
        imageProp = serializedObject.FindProperty("image");
        offsetProp = serializedObject.FindProperty("offset");
    }

    /// <summary>
    /// # Flyt decal #
    /// Vælg gameobjecet med dettet script(DecalObject), hold CTRL nede og klik et sted i secene view hvorefter
    /// gameobjecet vil flytte sig hent til der hvor man klikket
    /// </summary>
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
                moveMe.GetComponent<DecalObject>().hit = hit;
                EditorUtility.SetDirty(moveMe);

            }
        }
        
    }

    /// <summary>
    /// Her ændre vi udseende for DecalObject scripet i Inspector vinduet
    /// Vi gør dette i et CustomEditor scirpet så vi kan se billedet af
    /// decalet.
    /// </summary>
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
                EditorUtility.SetDirty(((DecalObject)item));
            }

        }

    }
    
    #endregion
}
