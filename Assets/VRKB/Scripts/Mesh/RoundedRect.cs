/// \file
/// <summary>
/// Procedural mesh for a cube with rounded corners. (Used for keyboard keys.)
/// </summary>

using System;
using System.Collections.Generic;
using UnityEngine;

namespace VRKB
{
// See https://docs.unity3d.com/Manual/UpgradeGuide20183.html
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(BoxCollider))]
    public class RoundedRect : MonoBehaviour
    {
        public Vector3 Size = Vector3.one;
        public float CornerRadius = 0.25f;
        public int VerticesPerCorner = 2;

        protected Mesh _mesh;

        public void OnEnable()
        {
            UpdateMesh();
            UpdateBoxCollider();
        }

        public void UpdateBoxCollider()
        {
            BoxCollider collider = GetComponent<BoxCollider>();
            collider.size = Size;
            collider.isTrigger = true;

            // force refresh in Prefab Mode
            collider.enabled = false; collider.enabled = true;
        }

        public IEnumerable<Vector3> CornerVertices(
            Vector3 center, float radius, int numVertices,
            Func<float, float> radiansToXOffset,
            Func<float, float> radiansToYOffset)
        {
            for (int i = 0; i < numVertices; ++i) {
                float radians = (float)i / (numVertices - 1) * Mathf.PI / 2.0f;
                yield return new Vector3(
                    center.x + radiansToXOffset(radians) * radius,
                    center.y + radiansToYOffset(radians) * radius,
                    center.z);
            }
        }

        public IEnumerable<Vector3> LayerVertices(float z,
            Vector3 size, float cornerRadius, int verticesPerCorner)
        {
            Vector3 min = -size / 2.0f;
            Vector3 max = size / 2.0f;

            // top right corner (+x, +y)

            foreach (var v in
                CornerVertices(
                    new Vector3(max.x - cornerRadius, max.y - cornerRadius, z),
                    cornerRadius,
                    verticesPerCorner,
                    radians => +Mathf.Sin(radians),
                    radians => +Mathf.Cos(radians)))
            {
                    yield return v;
            }

            // bottom right corner (+x, -y)

            foreach (var v in
                CornerVertices(
                    new Vector3(max.x - cornerRadius, min.y + cornerRadius, z),
                    cornerRadius,
                    verticesPerCorner,
                    radians => +Mathf.Cos(radians),
                    radians => -Mathf.Sin(radians)))
            {
                    yield return v;
            }

            // bottom left corner (-x, -y)

            foreach (var v in
                CornerVertices(
                    new Vector3(min.x + cornerRadius, min.y + cornerRadius, z),
                    cornerRadius,
                    verticesPerCorner,
                    radians => -Mathf.Sin(radians),
                    radians => -Mathf.Cos(radians)))
            {
                    yield return v;
            }

            // top left corner (-x, +y)

            foreach (var v in
                CornerVertices(
                    new Vector3(min.x + cornerRadius, max.y - cornerRadius, z),
                    cornerRadius,
                    verticesPerCorner,
                    radians => -Mathf.Cos(radians),
                    radians => +Mathf.Sin(radians)))
            {
                    yield return v;
            }
        }

        public IEnumerable<Vector3> Vertices(Vector3 size,
            float cornerRadius, int verticesPerCorner)
        {
            // Front layer (z = min.z).
            // Vertices are repeated to create hard normals (sharp edges).

            foreach(var v in LayerVertices(-size.z / 2.0f, size, cornerRadius, verticesPerCorner))
                yield return v;
            foreach(var v in LayerVertices(-size.z / 2.0f, size, cornerRadius, verticesPerCorner))
                yield return v;

            // Back layer (z = max.z).
            // Vertices are repeated to create hard normals (sharp edges).

            foreach(var v in LayerVertices(size.z / 2.0f, size, cornerRadius, verticesPerCorner))
                yield return v;
            foreach(var v in LayerVertices(size.z / 2.0f, size, cornerRadius, verticesPerCorner))
                yield return v;

        }

        public void UpdateMesh()
        {
            if (_mesh == null) {
                _mesh = new Mesh();
                _mesh.name = "RoundedRect";
                // don't serialize mesh data when saving scene/prefab
                _mesh.hideFlags = HideFlags.HideAndDontSave;
            }

            GetComponent<MeshFilter>().mesh = _mesh;

            if (VerticesPerCorner < 2)
                VerticesPerCorner = 2;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();
            List<Vector3> normals = new List<Vector3>();

#if comment
            Vector3 min = -Size / 2.0f;
            Vector3 max = Size / 2.0f;
#endif

            // create vertices

            foreach(var v in Vertices(Size, CornerRadius, VerticesPerCorner)) {
                vertices.Add(v);
                normals.Add(Vector3.zero);
                uvs.Add(Vector2.zero);
            }

            // create triangles/normals for front layer (z = min.z)

            int verticesPerLayer = VerticesPerCorner * 4;
            for (int i = 0; i < verticesPerLayer - 2; ++i) {
                triangles.Add(0);
                triangles.Add(i + 1);
                triangles.Add(i + 2);
                normals[i] = new Vector3(0.0f, 0.0f, -1.0f);
            }

            // create triangles/normals for sides

            int offset = verticesPerLayer;
            for (int i = 0; i < verticesPerLayer; ++i) {

                // Clockwise vertex indices for quad
                // Note: `(i + 1) % verticesPerLayer` is
                // needed here so that `(i + 1)` will wrap
                // around to 0 for the last quad.

                int i0 = offset + i;
                int i1 = offset + i + verticesPerLayer;
                int i2 = offset + (i + 1) % verticesPerLayer + verticesPerLayer;
                int i3 = offset + (i + 1) % verticesPerLayer;

                triangles.Add(i0);
                triangles.Add(i1);
                triangles.Add(i2);

                triangles.Add(i0);
                triangles.Add(i2);
                triangles.Add(i3);

                Vector3 v1 = vertices[i3] - vertices[i0];
                Vector3 v2 = vertices[i2] - vertices[i0];
                Vector3 normal = Vector3.Cross(v1, v2).normalized;

                normals[i0] = normal;
                normals[i1] = normal;
                normals[i2] = normal;
                normals[i3] = normal;

            }

            // create triangles/normals for back layer (z = max.z)

            offset = verticesPerLayer * 3;
            for (int i = 0; i < verticesPerLayer - 2; ++i) {
                triangles.Add(offset);
                triangles.Add(offset + i + 2);
                triangles.Add(offset + i + 1);
                normals[offset + i] = new Vector3(0.0f, 0.0f, 1.0f);
            }

            // assign vertex/triangle data and recalculate bounds/normals

            _mesh.Clear();
            _mesh.SetVertices(vertices);
            _mesh.SetUVs(0, uvs);
            _mesh.SetTriangles(triangles, 0);
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
        }
    }
}
