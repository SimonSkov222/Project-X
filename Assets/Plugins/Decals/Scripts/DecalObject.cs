using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class DecalObject : MonoBehaviour {

    private MeshFilter filter;
    private new MeshRenderer renderer;

    public Sprite image;
    public float offset = 0.01f;

    #if (UNITY_EDITOR)
    void OnEnable()
    {
        if (Application.isEditor && !Application.isPlaying)
        {
            BuildDecal();
        }
    }
    #endif


    public void BuildDecal()
    {

        filter = gameObject.GetComponent<MeshFilter>() != null ? gameObject.GetComponent<MeshFilter>() : gameObject.AddComponent<MeshFilter>();
        renderer = gameObject.GetComponent<MeshRenderer>() != null ? gameObject.GetComponent<MeshRenderer>() : gameObject.AddComponent<MeshRenderer>();

        filter.mesh = CreateMesh(transform.localScale.x, transform.localScale.y);

        var tempMaterial = new Material(Shader.Find("Transparent/Diffuse"));
        var croppedTexture = new Texture2D((int)image.rect.width, (int)image.rect.height);
        var pixels = image.texture.GetPixels((int)image.textureRect.x,
                                            (int)image.textureRect.y,
                                            (int)image.textureRect.width,
                                            (int)image.textureRect.height);

        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();

        //tempMaterial.shader = Shader.Find("Transparent/Diffuse");
        tempMaterial.mainTexture = croppedTexture;
        renderer.sharedMaterial = tempMaterial;
    }

    Mesh CreateMesh(float width, float height)
    {
        width = width / 2;
        height = height / 2;

        Mesh m = new Mesh();
        m.name = "Decal";
        m.vertices = new Vector3[] {
         new Vector3(width, height, offset),            //Top right
         new Vector3(-width, height,offset),        //Bottom right
         new Vector3(-width, -height, offset),     //bottom left
         new Vector3(width, -height, offset)     //top left 
     };
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
}
