using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class Plan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int NbLignes = 1;
        int NbColonnes = 1;

        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(1, 0, 0));
        vertices.Add(new Vector3(0, 1, 0));
        vertices.Add(new Vector3(1, 1, 0));

        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        triangles.Add(1);
        triangles.Add(3);
        triangles.Add(2);


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
