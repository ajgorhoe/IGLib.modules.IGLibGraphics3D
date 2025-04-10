using System;
using System.IO;
using System.Linq;
using System.Globalization;

using IG.Num;
using System.Text;
using System.Collections.Generic;

namespace IGLib.Gr3D
{

    public static class MeshExportExtensions
    {

        /// <summary>
        /// Exports a structured mesh to OBJ format, including vertex normals, surfaces, and optional wireframe.
        /// </summary>
        /// <param name="mesh">The structured mesh to export.</param>
        /// <param name="objFilePath">Path to the OBJ file.</param>
        /// <param name="mtlFileName">Name of the material file referenced from the OBJ.</param>
        /// <param name="exportSurfaces">If true, surface faces are exported.</param>
        /// <param name="exportWireframe">If true, wireframe lines are exported.</param>
        /// <param name="surfaceMaterialName">Material name for surfaces.</param>
        /// <param name="wireframeMaterialName">Material name for wireframe.</param>
        public static void ExportMeshToObj(this StructuredSurfaceMesh3D mesh,
            string objFilePath,
            string mtlFileName,
            bool exportSurfaces = true,
            bool exportWireframe = true,
            string surfaceMaterialName = "SurfaceMaterial",
            string wireframeMaterialName = "WireframeMaterial")
        {
            using (StreamWriter writer = new StreamWriter(objFilePath))
            {
                writer.WriteLine($"mtllib {mtlFileName}");

                int numRows = mesh.NumPoints1;
                int numCols = mesh.NumPoints2;

                // Vertices
                for (int i = 0; i < numRows; i++)
                    for (int j = 0; j < numCols; j++)
                        writer.WriteLine($"v {mesh[i, j].x} {mesh[i, j].y} {mesh[i, j].z}");

                // Normals
                for (int i = 0; i < numRows; i++)
                    for (int j = 0; j < numCols; j++)
                        writer.WriteLine($"vn {mesh.VertexNormals[i][j].x} {mesh.VertexNormals[i][j].y} {mesh.VertexNormals[i][j].z}");

                // Surfaces
                if (exportSurfaces)
                {
                    writer.WriteLine("o MeshSurface");
                    writer.WriteLine($"usemtl {surfaceMaterialName}");

                    for (int i = 0; i < numRows - 1; i++)
                    {
                        for (int j = 0; j < numCols - 1; j++)
                        {
                            int v1 = i * numCols + j + 1;
                            int v2 = v1 + 1;
                            int v3 = v1 + numCols;
                            int v4 = v3 + 1;

                            writer.WriteLine($"f {v1}//{v1} {v2}//{v2} {v4}//{v4}");
                            writer.WriteLine($"f {v1}//{v1} {v4}//{v4} {v3}//{v3}");
                        }
                    }
                }

                // Wireframe
                if (exportWireframe)
                {
                    writer.WriteLine("o Wireframe");
                    writer.WriteLine($"usemtl {wireframeMaterialName}");
                    writer.WriteLine("# Wireframe edges");

                    for (int i = 0; i < numRows; i++)
                        for (int j = 0; j < numCols - 1; j++)
                            writer.WriteLine($"l {i * numCols + j + 1} {i * numCols + j + 2}");

                    for (int j = 0; j < numCols; j++)
                        for (int i = 0; i < numRows - 1; i++)
                            writer.WriteLine($"l {i * numCols + j + 1} {(i + 1) * numCols + j + 1}");
                }
            }
        }

        /// <summary>
        /// Exports only the surface mesh to an OBJ file.
        /// </summary>
        public static void ExportMeshSurfaceToObj(this StructuredSurfaceMesh3D mesh,
            string objFilePath,
            string mtlFileName,
            string surfaceMaterialName = "SurfaceMaterial") =>
            mesh.ExportMeshToObj(objFilePath, mtlFileName, exportSurfaces: true, exportWireframe: false, surfaceMaterialName, "WireframeMaterial");

        /// <summary>
        /// Exports only the wireframe of the mesh to an OBJ file.
        /// </summary>
        public static void ExportMeshWireframeToObj(this StructuredSurfaceMesh3D mesh,
            string objFilePath,
            string mtlFileName,
            string wireframeMaterialName = "WireframeMaterial") =>
            mesh.ExportMeshToObj(objFilePath, mtlFileName, exportSurfaces: false, exportWireframe: true, "SurfaceMaterial", wireframeMaterialName);

        /// <summary>
        /// Exports a material file based on the provided material properties.
        /// </summary>
        /// <param name="filePath">Output file path (.mtl)</param>
        /// <param name="material">Material properties.</param>
        public static void ExportMaterial(string filePath, MaterialProperties material)
        {
            material.SaveToMaterialFile(filePath);
        }


        /// <summary>Exports the mesh surface to an ASCII STL file.</summary>
        public static void ExportToStl(this StructuredSurfaceMesh3D mesh, string stlFilePath)
        {
            using StreamWriter writer = new StreamWriter(stlFilePath);
            writer.WriteLine("solid mesh");

            for (int i = 0; i < mesh.NumPoints1 - 1; i++)
            {
                for (int j = 0; j < mesh.NumPoints2 - 1; j++)
                {
                    vec3 v1 = mesh[i, j];
                    vec3 v2 = mesh[i, j + 1];
                    vec3 v3 = mesh[i + 1, j + 1];
                    vec3 v4 = mesh[i + 1, j];
                    WriteFacet(writer, v1, v2, v3);
                    WriteFacet(writer, v1, v3, v4);
                }
            }

            writer.WriteLine("endsolid mesh");
        }

        /// <summary>Auxiliary method for <see cref="ExportToStl(StructuredSurfaceMesh3D, string)"/>,
        /// writes a single facet to the stream.</summary>
        private static void WriteFacet(StreamWriter writer, vec3 v1, vec3 v2, vec3 v3)
        {
            vec3 normal = vec3.Cross(v2 - v1, v3 - v1).Normalize();
            writer.WriteLine($"facet normal {normal.x} {normal.y} {normal.z}");
            writer.WriteLine("  outer loop");
            writer.WriteLine($"    vertex {v1.x} {v1.y} {v1.z}");
            writer.WriteLine($"    vertex {v2.x} {v2.y} {v2.z}");
            writer.WriteLine($"    vertex {v3.x} {v3.y} {v3.z}");
            writer.WriteLine("  endloop");
            writer.WriteLine("endfacet");
        }


        /// <summary>Exports the mesh to GLF format.
        /// <para>Features:</para>
        /// <para>* Embedded base64 buffer (ASCII .gltf)</para>
        /// <para>* Vertex positions</para>
        /// <para>* Triangle indices</para>
        /// <para>* Normals (optional, from VertexNormals)</para>
        /// <para>* Vertex colors (optional, from VertexColors)</para>
        /// <para>* Material reference</para>
        /// <para>* Optional lights (LightSource)</para>
        /// <para>* </para>
        /// <para>* </para>
        /// </summary>
        /// <param name="mesh">Mesh to be exported.</param>
        /// <param name="gltfPath">Name of the export file.</param>
        /// <param name="material">Material to be assigned to the surface.</param>
        /// <param name="lights">Lights used in rendering the surface, will be written to
        /// the file if specified.</param>
        public static void ExportToGltf(this StructuredSurfaceMesh3D mesh, string gltfPath,
            MaterialProperties material, LightSource[] lights = null)
        {
            var positions = new List<float>();
            var normals = new List<float>();
            var colors = new List<float>();
            var indices = new List<ushort>();

            int rows = mesh.NumPoints1;
            int cols = mesh.NumPoints2;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var v = mesh.Vertices[i][j];
                    positions.Add((float)v.x);
                    positions.Add((float)v.y);
                    positions.Add((float)v.z);

                    if (mesh.VertexNormals != null)
                    {
                        var n = mesh.VertexNormals[i][j];
                        normals.Add((float)n.x);
                        normals.Add((float)n.y);
                        normals.Add((float)n.z);
                    }

                    if (mesh.VertexColors != null)
                    {
                        var c = mesh.VertexColors[i][j];
                        colors.Add(c.R);
                        colors.Add(c.G);
                        colors.Add(c.B);
                        colors.Add(c.A);
                    }
                }
            }

            for (int i = 0; i < rows - 1; i++)
            {
                for (int j = 0; j < cols - 1; j++)
                {
                    int v0 = i * cols + j;
                    int v1 = v0 + 1;
                    int v2 = v0 + cols;
                    int v3 = v2 + 1;

                    indices.Add((ushort)v0);
                    indices.Add((ushort)v1);
                    indices.Add((ushort)v3);

                    indices.Add((ushort)v0);
                    indices.Add((ushort)v3);
                    indices.Add((ushort)v2);
                }
            }

            string base64Positions = Convert.ToBase64String(ToBytes(positions));
            string base64Normals = normals.Count > 0 ? Convert.ToBase64String(ToBytes(normals)) : null;
            string base64Colors = colors.Count > 0 ? Convert.ToBase64String(ToBytes(colors)) : null;
            string base64Indices = Convert.ToBase64String(ToBytes(indices));

            using StreamWriter writer = new StreamWriter(gltfPath);
            writer.WriteLine("{");
            writer.WriteLine("\"asset\": { \"version\": \"2.0\" },");
            writer.WriteLine("\"scene\": 0,");
            writer.WriteLine("\"scenes\": [{ \"nodes\": [0] }],");
            writer.WriteLine("\"nodes\": [{ \"mesh\": 0 }],");
            writer.WriteLine("\"buffers\": [");

            writer.WriteLine("{ \"uri\": \"data:application/octet-stream;base64," + base64Positions + "\",");
            writer.WriteLine($"\"byteLength\": {positions.Count * 4} }}");

            if (base64Normals != null)
            {
                writer.WriteLine(",{ \"uri\": \"data:application/octet-stream;base64," + base64Normals + "\",");
                writer.WriteLine($"\"byteLength\": {normals.Count * 4} }}");
            }

            if (base64Colors != null)
            {
                writer.WriteLine(",{ \"uri\": \"data:application/octet-stream;base64," + base64Colors + "\",");
                writer.WriteLine($"\"byteLength\": {colors.Count * 4} }}");
            }

            writer.WriteLine(",{ \"uri\": \"data:application/octet-stream;base64," + base64Indices + "\",");
            writer.WriteLine($"\"byteLength\": {indices.Count * 2} }}");

            writer.WriteLine("],");

            // Mesh and materials
            writer.WriteLine("\"meshes\": [{");
            writer.WriteLine("\"primitives\": [{");
            writer.WriteLine("\"attributes\": {");
            writer.WriteLine("\"POSITION\": 0,");

            if (base64Normals != null)
                writer.WriteLine("\"NORMAL\": 1,");
            if (base64Colors != null)
                writer.WriteLine("\"COLOR_0\": 2,");

            writer.WriteLine("\"indices\": 3");
            writer.WriteLine("},");
            writer.WriteLine("\"material\": 0 }]}],");

            // Materials
            writer.WriteLine("\"materials\": [");
            writer.WriteLine("{");
            writer.WriteLine($"\"name\": \"{material.Name}\",");
            writer.WriteLine("\"pbrMetallicRoughness\": {");
            writer.WriteLine($"\"baseColorFactor\": [{material.DiffuseColor.x}, {material.DiffuseColor.y}, {material.DiffuseColor.z}, {material.Transparency}],");
            writer.WriteLine("\"metallicFactor\": 0.1, \"roughnessFactor\": 0.6");
            writer.WriteLine("}");
            writer.WriteLine("}");
            writer.WriteLine("]");

            if (lights != null && lights.Length > 0)
            {
                writer.WriteLine(",\"extensions\": {");
                writer.WriteLine("\"KHR_lights_punctual\": { \"lights\": [");
                for (int i = 0; i < lights.Length; i++)
                {
                    writer.Write(lights[i].ToGltfJson(i));
                    if (i < lights.Length - 1) writer.WriteLine(",");
                }
                writer.WriteLine("] } }");
            }

            writer.WriteLine("}");
        }

        private static byte[] ToBytes(List<float> list)
        {
            byte[] bytes = new byte[list.Count * 4];
            Buffer.BlockCopy(list.ToArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private static byte[] ToBytes(List<ushort> list)
        {
            byte[] bytes = new byte[list.Count * 2];
            Buffer.BlockCopy(list.ToArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }


    }

}