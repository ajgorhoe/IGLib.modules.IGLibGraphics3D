
#nullable disable

using System;
using System.IO;
using System.Linq;
using System.Globalization;

using IG.Num;

namespace IGLib.Gr3D
{

    public static class MeshExportExtensions_02
    {
        /// <summary>
        /// Exports the mesh to an OBJ file with an optional MTL file for materials.
        /// </summary>
        public static void ExportToObj(this StructuredMesh3D_02 mesh, string objFilePath, string materialName = "defaultMaterial", string mtlFilePath = null)
        {
            using (StreamWriter writer = new StreamWriter(objFilePath))
            {
                writer.WriteLine("# Exported OBJ file");
                if (!string.IsNullOrEmpty(mtlFilePath))
                {
                    writer.WriteLine($"mtllib {Path.GetFileName(mtlFilePath)}"); // Link to material file
                }

                // Write vertices
                foreach (var row in mesh.Nodes)
                    foreach (var v in row)
                        writer.WriteLine($"v {v.x.ToString(CultureInfo.InvariantCulture)} {v.y.ToString(CultureInfo.InvariantCulture)} {v.z.ToString(CultureInfo.InvariantCulture)}");

                // Write faces (quads converted to triangles)
                int numCols = mesh.NumPoints2;
                for (int i = 0; i < mesh.NumPoints1 - 1; i++)
                {
                    for (int j = 0; j < mesh.NumPoints2 - 1; j++)
                    {
                        int v1 = i * numCols + j + 1;
                        int v2 = (i + 1) * numCols + j + 1;
                        int v3 = (i + 1) * numCols + (j + 1) + 1;
                        int v4 = i * numCols + (j + 1) + 1;

                        // Two triangles per quadrilateral
                        writer.WriteLine($"f {v1} {v2} {v3}");
                        writer.WriteLine($"f {v1} {v3} {v4}");
                    }
                }

                writer.WriteLine("# End of file");
            }

            // Export MTL file if needed
            if (!string.IsNullOrEmpty(mtlFilePath))
            {
                ExportMaterialFile(mtlFilePath, materialName);
            }
        }

        /// <summary>
        /// Creates a simple MTL file for OBJ materials.
        /// </summary>
        private static void ExportMaterialFile(string mtlFilePath, string materialName)
        {
            using (StreamWriter writer = new StreamWriter(mtlFilePath))
            {
                writer.WriteLine($"newmtl {materialName}");
                writer.WriteLine("Ka 0.2 0.2 0.2"); // Ambient color
                writer.WriteLine("Kd 0.8 0.5 0.3"); // Diffuse color (modifiable)
                writer.WriteLine("Ks 1.0 1.0 1.0"); // Specular color
                writer.WriteLine("Ns 50.0");       // Shininess
                writer.WriteLine("d 1.0");         // Transparency (1 = opaque)
                writer.WriteLine("illum 2");       // Lighting model
            }
        }

        /// <summary>
        /// Exports the mesh to a binary STL file.
        /// </summary>
        public static void ExportToStl(this StructuredMesh3D_02 mesh, string stlFilePath)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(stlFilePath, FileMode.Create)))
            {
                writer.Write(new byte[80]); // 80-byte STL header
                int numTriangles = (mesh.NumPoints1 - 1) * (mesh.NumPoints2 - 1) * 2;
                writer.Write(numTriangles);

                for (int i = 0; i < mesh.NumPoints1 - 1; i++)
                {
                    for (int j = 0; j < mesh.NumPoints2 - 1; j++)
                    {
                        vec3[] quad = mesh.GetQuadrilateral(i, j);

                        // Triangle 1
                        WriteTriangle(writer, quad[0], quad[1], quad[2]);

                        // Triangle 2
                        WriteTriangle(writer, quad[0], quad[2], quad[3]);
                    }
                }
            }
        }

        /// <summary>
        /// Writes a single triangle to the STL file.
        /// </summary>
        private static void WriteTriangle(BinaryWriter writer, vec3 v1, vec3 v2, vec3 v3)
        {
            vec3 normal = vec3.Cross(v2 - v1, v3 - v1).Normalize(); // Compute normal

            writer.Write((float)normal.x);
            writer.Write((float)normal.y);
            writer.Write((float)normal.z);

            writer.Write((float)v1.x);
            writer.Write((float)v1.y);
            writer.Write((float)v1.z);

            writer.Write((float)v2.x);
            writer.Write((float)v2.y);
            writer.Write((float)v2.z);

            writer.Write((float)v3.x);
            writer.Write((float)v3.y);
            writer.Write((float)v3.z);

            writer.Write((ushort)0); // Attribute byte count
        }
    }

}