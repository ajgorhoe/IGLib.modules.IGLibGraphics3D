using System;
using System.IO;
using System.Linq;
using System.Globalization;

using IG.Num;
using System.Text;

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
        public static void ExportMeshToObj(this StructuredMesh3D mesh,
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
                        writer.WriteLine($"vn {mesh.NodeNormals[i][j].x} {mesh.NodeNormals[i][j].y} {mesh.NodeNormals[i][j].z}");

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
        public static void ExportMeshSurfaceToObj(this StructuredMesh3D mesh,
            string objFilePath,
            string mtlFileName,
            string surfaceMaterialName = "SurfaceMaterial") =>
            mesh.ExportMeshToObj(objFilePath, mtlFileName, exportSurfaces: true, exportWireframe: false, surfaceMaterialName, "WireframeMaterial");

        /// <summary>
        /// Exports only the wireframe of the mesh to an OBJ file.
        /// </summary>
        public static void ExportMeshWireframeToObj(this StructuredMesh3D mesh,
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
        public static void ExportToStl(this StructuredMesh3D mesh, string stlFilePath)
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




        public static void ExportToGltf(this StructuredMesh3D mesh, string gltfPath, MaterialProperties material, LightingDefinition[] lights = null)
        {
            using StreamWriter writer = new StreamWriter(gltfPath);
            writer.WriteLine("{");
            writer.WriteLine("\"asset\": { \"version\": \"2.0\" },");

            // Nodes (only geometry, can add transform/camera/light as needed)
            writer.WriteLine("\"nodes\": [ { \"mesh\": 0 } ],");

            // Mesh data placeholder (in practice needs buffer/indices)
            writer.WriteLine("\"meshes\": [");
            writer.WriteLine("{");
            writer.WriteLine("\"primitives\": [");
            writer.WriteLine("{ \"attributes\": { \"POSITION\": 0 }, \"material\": 0 }");
            writer.WriteLine("]");
            writer.WriteLine("}");
            writer.WriteLine("],");

            // Materials
            writer.WriteLine("\"materials\": [");
            writer.WriteLine("{");
            writer.WriteLine($"\"name\": \"{material.Name}\",");
            writer.WriteLine("\"pbrMetallicRoughness\": {");
            writer.WriteLine($"\"baseColorFactor\": [{material.DiffuseColor.x}, {material.DiffuseColor.y}, {material.DiffuseColor.z}, {material.Transparency}]");
            writer.WriteLine("}");
            writer.WriteLine("}");
            writer.WriteLine("],");

            // Lights (KHR extension)
            if (lights != null && lights.Length > 0)
            {
                writer.WriteLine("\"extensions\": {");
                writer.WriteLine("\"KHR_lights_punctual\": {");
                writer.WriteLine("\"lights\": [");
                for (int i = 0; i < lights.Length; i++)
                {
                    writer.Write(lights[i].ToGltfJson(i));
                    if (i < lights.Length - 1)
                        writer.WriteLine(",");
                    else
                        writer.WriteLine();
                }
                writer.WriteLine("]");
                writer.WriteLine("}");
                writer.WriteLine("},");
            }

            writer.WriteLine("\"scene\": 0");
            writer.WriteLine("}");
        }




    }

}