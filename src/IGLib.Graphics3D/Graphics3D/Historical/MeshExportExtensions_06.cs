using System;
using System.IO;
using System.Linq;
using System.Globalization;

using IG.Num;
using System.Text;

namespace IGLib.Gr3D
{

    public static class MeshExportExtensions_06
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
        public static void ExportMeshToObj(StructuredMesh3D mesh,
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
        public static void ExportMeshSurfaceToObj(StructuredMesh3D mesh,
            string objFilePath,
            string mtlFileName,
            string surfaceMaterialName = "SurfaceMaterial") =>
            mesh.ExportMeshToObj(objFilePath, mtlFileName, exportSurfaces: true, exportWireframe: false, surfaceMaterialName, "WireframeMaterial");

        /// <summary>
        /// Exports only the wireframe of the mesh to an OBJ file.
        /// </summary>
        public static void ExportMeshWireframeToObj(StructuredMesh3D mesh,
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
    }

}