using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RadarChart : MonoBehaviour
{
    Mesh mesh;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    [ContextMenu("Set Chart")]
    public void SetChart(List<CourseMetric> attributes)
    {
        Destroy(mesh);
        mesh = new Mesh();

        //set verts
        List<Vector3> verts = new List<Vector3>();
        List<int> triangles = new List<int>();
        int triangleCounter = 0;

        float angleStep = 360f / attributes.Count;

        for(int i = 0; i < attributes.Count; i++)
        {
            float avgScale = (attributes[i].normalizedScore + attributes[(i + 1) % attributes.Count].normalizedScore) / 2;
            verts.Add(attributes[i].normalizedScore * ( new Vector3(Mathf.Sin(i * angleStep), Mathf.Cos(i * angleStep) ) ) );
            verts.Add(avgScale * ( new Vector3(Mathf.Sin(i * angleStep + angleStep / 2),
                                    Mathf.Cos(i * angleStep + angleStep / 2))) );
            verts.Add(Vector2.zero);

            triangles.Add(triangleCounter++);
            triangles.Add(triangleCounter++);
            triangles.Add(triangleCounter++);
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(triangles, 0);

        List<Vector2> uvs = new List<Vector2>();

        for (int i = 0; i < uvs.Count; i++)
        {
            uvs.Add(new Vector2(verts[i].x, verts[i].z));
        }

        mesh.SetUVs(0, uvs);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}