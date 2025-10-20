using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Cylindre : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // draw a primitive circle mesh
        int NbSegments = 10;
        float radius = 1f;
        vertices.Add(new Vector3(0, 0, 0)); // center vertex
        for (int i = 0; i <= NbSegments; i++)
        {
            float angle = i * 2 * Mathf.PI / NbSegments;
            float x = radius * Mathf.Cos(angle);
            float y = radius * Mathf.Sin(angle);
            vertices.Add(new Vector3(x, y, 0));
        }
        for (int i = 1; i <= NbSegments; i++)
        {
            triangles.Add(0); // center vertex
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        // extrude the circle to create a cylinder
        int baseVertexCount = vertices.Count;
        for (int i = 0; i < baseVertexCount; i++)
        {
            Vector3 v = vertices[i];
            vertices.Add(new Vector3(v.x, v.y, 1)); // extrude along z-axis
        }
        for (int i = 1; i <= NbSegments; i++)
        {
            int currentBase = i;
            int nextBase = (i % NbSegments) + 1;
            int currentTop = currentBase + baseVertexCount;
            int nextTop = nextBase + baseVertexCount;
            // side triangles
            triangles.Add(currentBase);
            triangles.Add(nextBase);
            triangles.Add(currentTop);
            triangles.Add(nextBase);
            triangles.Add(nextTop);
            triangles.Add(currentTop);
        }


        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }


    // Update is called once per frame
    void Update()
    {

    }

    /*
    private void OnDrawGizmos()
    {
        vertices = new List<Vector3>();

        foreach (var t in vertices)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(tag, 0.1f);
        }
    }*/
}
