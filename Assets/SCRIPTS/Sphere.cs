using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Sphere : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private int latitudeSegments = 10;   // parallèles
    [SerializeField] private int longitudeSegments = 10;  // méridiens

    void Start()
    {
        DrawSphere(radius, latitudeSegments, longitudeSegments);
    }

    public void DrawSphere(float radius, int latitudeSegments, int longitudeSegments)
    {
        if (latitudeSegments < 2) latitudeSegments = 2;
        if (longitudeSegments < 3) longitudeSegments = 3;

        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        Vector3[] vertices = new Vector3[(latitudeSegments + 1) * (longitudeSegments + 1)];
        int[] triangles = new int[latitudeSegments * longitudeSegments * 6];

        // vertices
        for (int lat = 0; lat <= latitudeSegments; lat++)
        {
            float theta = lat * Mathf.PI / latitudeSegments;
            float sinTheta = Mathf.Sin(theta);
            float cosTheta = Mathf.Cos(theta);

            for (int lon = 0; lon <= longitudeSegments; lon++)
            {
                float phi = lon * 2f * Mathf.PI / longitudeSegments;
                float sinPhi = Mathf.Sin(phi);
                float cosPhi = Mathf.Cos(phi);

                int index = lat * (longitudeSegments + 1) + lon;
                vertices[index] = new Vector3(
                    radius * sinTheta * cosPhi,
                    radius * sinTheta * sinPhi,
                    radius * cosTheta
                );
            }
        }

        int triIndex = 0;
        for (int lat = 0; lat < latitudeSegments; lat++)
        {
            for (int lon = 0; lon < longitudeSegments; lon++)
            {
                int current = lat * (longitudeSegments + 1) + lon;
                int next = current + longitudeSegments + 1;

                triangles[triIndex++] = current;
                triangles[triIndex++] = next;
                triangles[triIndex++] = current + 1;

                triangles[triIndex++] = current + 1;
                triangles[triIndex++] = next;
                triangles[triIndex++] = next + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    void Update() { }
}
