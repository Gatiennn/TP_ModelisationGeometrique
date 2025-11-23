using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cylindre : MonoBehaviour
{
    // Exposé pour réglages rapides dans l'inspecteur
    [SerializeField] private float radius = 1f;
    [SerializeField] private float height = 2f;
    [SerializeField] private int meridians = 36;

    void Start()
    {
        DrawCylindre(radius, height, meridians);
    }

    public void DrawCylindre(float radius, float height, int meridians)
    {
        if (meridians < 3) meridians = 3;
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        int ringCount = meridians + 1; 
        Vector3[] vertices = new Vector3[ringCount * 2 + 2];

        int bottomStart = 0;
        int topStart = ringCount;
        int bottomCenterIndex = ringCount * 2;
        int topCenterIndex = bottomCenterIndex + 1;

        for (int i = 0; i < ringCount; i++)
        {
            float angle = i * 2f * Mathf.PI / meridians;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            vertices[bottomStart + i] = new Vector3(x, y, 0f);         
            vertices[topStart + i]    = new Vector3(x, y, height);     
        }

        vertices[bottomCenterIndex] = new Vector3(0f, 0f, 0f);
        vertices[topCenterIndex]    = new Vector3(0f, 0f, height);
        int[] triangles = new int[meridians * 12]; // meridians * (6 côtés + 3 bas + 3 haut)
        int ti = 0;

        // côtés
        for (int i = 0; i < meridians; i++)
        {
            int b0 = bottomStart + i;
            int b1 = bottomStart + i + 1;
            int t0 = topStart + i;
            int t1 = topStart + i + 1;

            // premier triangle du quad
            triangles[ti++] = b0;
            triangles[ti++] = t0;
            triangles[ti++] = b1;

            // second triangle du quad
            triangles[ti++] = b1;
            triangles[ti++] = t0;
            triangles[ti++] = t1;
        }

        // disque inférieur (face vers -Z)
        for (int i = 0; i < meridians; i++)
        {
            int b0 = bottomStart + i;
            int b1 = bottomStart + i + 1;
            triangles[ti++] = bottomCenterIndex;
            triangles[ti++] = b1;
            triangles[ti++] = b0;
        }

        // disque supérieur (face vers +Z)
        for (int i = 0; i < meridians; i++)
        {
            int t0 = topStart + i;
            int t1 = topStart + i + 1;
            triangles[ti++] = topCenterIndex;
            triangles[ti++] = t0;
            triangles[ti++] = t1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void Update() { }
}
