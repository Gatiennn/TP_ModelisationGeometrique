using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cone : MonoBehaviour
{
    [SerializeField] private float baseRadius = 1f;
    [SerializeField] private float height = 1f;
    [SerializeField] private float truncateHeight = 1f; // hauteur (depuis la base) à laquelle on tronque ; = height => cone plein
    [SerializeField] private int meridians = 50;

    void Start()
    {
        DrawCone(baseRadius, height, truncateHeight, meridians);
    }

    public void DrawCone(float baseRadius, float height, float truncateHeight, int meridians)
    {
        if (meridians < 3) meridians = 3;
        if (height <= 0f) height = 0.0001f;

        truncateHeight = Mathf.Clamp(truncateHeight, 0f, height);
        float topRadius = baseRadius * Mathf.Max(0f, 1f - (truncateHeight / height));

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        if (topRadius <= Mathf.Epsilon)
        {
            int vertexCount = meridians + 2; 
            Vector3[] vertices = new Vector3[meridians + 2]; 
            int apexIndex = 0;
            // apex
            vertices[apexIndex] = new Vector3(0f, 0f, height);

            for (int i = 0; i <= meridians; i++)
            {
                float angle = i * 2f * Mathf.PI / meridians;
                float x = baseRadius * Mathf.Cos(angle);
                float y = baseRadius * Mathf.Sin(angle);
                vertices[i + 1] = new Vector3(x, y, 0f);
            }

            // ici vertices length est meridians+2, le dernier élément est la duplicate du premier ring si i==meridians,donc on doit redimensionner pour ajouter center.
            Vector3[] verts = new Vector3[meridians + 3];
            verts[0] = vertices[0];
            for (int i = 0; i <= meridians; i++) verts[i + 1] = vertices[i + 1];
            int baseCenterIndex = meridians + 2;
            verts[baseCenterIndex] = new Vector3(0f, 0f, 0f);
            vertices = verts;

            int[] triangles = new int[meridians * 6];
            int ti = 0;
            for (int i = 0; i < meridians; i++)
            {
                int b0 = 1 + i;
                int b1 = 1 + i + 1;

                triangles[ti++] = apexIndex;
                triangles[ti++] = b0;
                triangles[ti++] = b1;
            }

            for (int i = 0; i < meridians; i++)
            {
                int b0 = 1 + i;
                int b1 = 1 + i + 1;
                triangles[ti++] = baseCenterIndex;
                triangles[ti++] = b1;
                triangles[ti++] = b0;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            return;
        }

        
        int ringCount = meridians + 1; 
        Vector3[] ringVertices = new Vector3[ringCount * 2 + 2]; 

        int bottomStart = 0;
        int topStart = ringCount;
        int bottomCenterIndex = ringCount * 2;
        int topCenterIndex = bottomCenterIndex + 1;

        for (int i = 0; i < ringCount; i++)
        {
            float angle = i * 2f * Mathf.PI / meridians;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);
            float bx = baseRadius * cos;
            float by = baseRadius * sin;
            float tx = topRadius * cos;
            float ty = topRadius * sin;
            ringVertices[bottomStart + i] = new Vector3(bx, by, 0f);        
            ringVertices[topStart + i]    = new Vector3(tx, ty, truncateHeight); 
        }

        ringVertices[bottomCenterIndex] = new Vector3(0f, 0f, 0f);
        ringVertices[topCenterIndex]    = new Vector3(0f, 0f, truncateHeight);

        int[] tris = new int[meridians * 12];
        int t = 0;
        for (int i = 0; i < meridians; i++)
        {
            int b0 = bottomStart + i;
            int b1 = bottomStart + i + 1;
            int t0 = topStart + i;
            int t1 = topStart + i + 1;

            // premier triangle du quad
            tris[t++] = b0;
            tris[t++] = t0;
            tris[t++] = b1;

            // second triangle du quad
            tris[t++] = b1;
            tris[t++] = t0;
            tris[t++] = t1;
        }

        // disque inférieur (face vers -Z)
        for (int i = 0; i < meridians; i++)
        {
            int b0 = bottomStart + i;
            int b1 = bottomStart + i + 1;
            tris[t++] = bottomCenterIndex;
            tris[t++] = b1;
            tris[t++] = b0;
        }

        // disque supérieur (face vers +Z)
        for (int i = 0; i < meridians; i++)
        {
            int t0 = topStart + i;
            int t1 = topStart + i + 1;
            tris[t++] = topCenterIndex;
            tris[t++] = t0;
            tris[t++] = t1;
        }

        mesh.vertices = ringVertices;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void Update() { }
}
