using UnityEngine;
using Assets.Plugins.Decals.Scripts;


////////////////////////////////////////////////////////////////////
//  
//                      Beskrivelse
//  
//  ##!! Bruges ikke mere da vi fandt ud af at have                 !!##
//  ##!! formange decals/gameobject fik spillet til at lagge        !!##
//  ##!! vi har løs problemet ved at lave en shader i stedet for    !!##
//
//  Dette script gør kun noget i editor mode.
//  Og det den gør er at den laver en Quad hvor der bliver sat et
//  sprite på. 
//
//  Dette script bruger også en CustomEditor (DecalObjectEditor.cs)
//          
////////////////////////////////////////////////////////////////////
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class DecalObject : MonoBehaviour
{

    ///////////////////////////////
    //      Public Fields
    ///////////////////////////////
    #region

    public RaycastHit hit;
    public Sprite image;
    public float offset = 0.01f;

    #endregion

    ///////////////////////////////
    //      Private Fields
    ///////////////////////////////
    #region

    private MeshFilter filter;
    private new MeshRenderer renderer; //new fjerner en warning 

    #endregion

    ///////////////////////////////
    //      Unity Event
    ///////////////////////////////
    #region

    /// <summary>
    /// Opdatere decal når man starter unity editor.
    /// 
    /// Der er en fjel der gør at den også bliver kaldt
    /// Når man bare testet spillet
    /// </summary>
    void OnEnable()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            BuildDecal();
        }
    }

    #endregion

    ///////////////////////////////
    //      Public Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Her laver vi gameobject om til en decal med billede
    /// Der bliver ikke brugt shared material (kan være en grund til at det lagger)
    /// 
    /// hvis den er tæt på et andet gameobject vil den tegne billede på det andet gameobject
    /// (Der er et problem med størrelsesforholdet)
    /// 
    /// Denne metode bliver kaldt fra et andet scripts(DecalWindow.cs, DecalObjectEditor.cs)
    /// </summary>
    public void BuildDecal()
    {
        //Hent commponents og hvis den ikke har dem add dem
        filter = gameObject.GetComponent<MeshFilter>() != null ? gameObject.GetComponent<MeshFilter>() : gameObject.AddComponent<MeshFilter>();
        renderer = gameObject.GetComponent<MeshRenderer>() != null ? gameObject.GetComponent<MeshRenderer>() : gameObject.AddComponent<MeshRenderer>();

        //Lav en firkant så vi kan se billedet
        filter.mesh = CreateMesh(transform.localScale.x, transform.localScale.y);

        // lavet nyt material der har samme størrelse som decal billedet
        var tempMaterial = new Material(Shader.Find("Transparent/Diffuse")); //Går den kan være usynlig nogle steder
        var croppedTexture = new Texture2D((int)image.rect.width, (int)image.rect.height);
        var pixels = image.texture.GetPixels((int)image.textureRect.x,
                                            (int)image.textureRect.y,
                                            (int)image.textureRect.width,
                                            (int)image.textureRect.height);

        //Tilføj billede i materialet
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();//decal

        //Hvis den er "tæt"(skal klikke på et gameobject) på et gameobject
        //vil den tegne på det gameobject materiale
        if (hit.collider != null)
        {
            var o = (Texture2D)hit.collider.GetComponent<Renderer>().material.mainTexture;
            o.Blit(croppedTexture, hit.textureCoord);
        }

        //Giv decal gameobject materialet
        //tempMaterial.shader = Shader.Find("Transparent/Diffuse");
        tempMaterial.mainTexture = croppedTexture;
        renderer.sharedMaterial = tempMaterial;
    }

    #endregion

    ///////////////////////////////
    //      Private Methods
    ///////////////////////////////
    #region

    /// <summary>
    /// Laver en firkantet mesh der gør det muligt at se gameobjectets materiale.
    /// </summary>
    private Mesh CreateMesh(float width, float height)
    {
        //vertices starter fra midten derfor skal vi kun bruge halvdelen
        width = width / 2;
        height = height / 2;

        Mesh m = new Mesh();
        m.name = "Decal";
        //definere hjørnerne
        m.vertices = new Vector3[] {
        new Vector3(width, height, offset),
        new Vector3(-width, height,offset),
        new Vector3(-width, -height, offset),
        new Vector3(width, -height, offset)
    };
        // hvordan materialet skal sidde
        m.uv = new Vector2[] {
        new Vector2 (0, 1),
        new Vector2 (1, 1),
        new Vector2(1, 0),
        new Vector2 (0, 0)
    };

        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        return m;
    }

    #endregion
}
