using System;
using System.IO;
using System.Linq;
using System.Globalization;

using IG.Num;
using System.Text;

namespace IGLib.Gr3D
{

    using System.IO;
    using System.Globalization;

    public static class MeshExportExtensions
    {
        public static void ExportToObj(this StructuredMesh3D mesh, string filePath, string materialFileName)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"mtllib {materialFileName}");

                // Write vertices
                for (int i = 0; i < mesh.NumPoints1; i++)
                    for (int j = 0; j < mesh.NumPoints2; j++)
                        writer.WriteLine($"v {mesh[i, j].x} {mesh[i, j].y} {mesh[i, j].z}");

                // Write normals
                for (int i = 0; i < mesh.NumPoints1; i++)
                    for (int j = 0; j < mesh.NumPoints2; j++)
                        writer.WriteLine($"vn {mesh.NodeNormals[i][j].x} {mesh.NodeNormals[i][j].y} {mesh.NodeNormals[i][j].z}");

                // Write faces
                for (int i = 0; i < mesh.NumPoints1 - 1; i++)
                {
                    for (int j = 0; j < mesh.NumPoints2 - 1; j++)
                    {
                        int v1 = i * mesh.NumPoints2 + j + 1;
                        int v2 = v1 + 1;
                        int v3 = v1 + mesh.NumPoints2;
                        int v4 = v3 + 1;

                        writer.WriteLine($"f {v1}//{v1} {v2}//{v2} {v4}//{v4}");
                        writer.WriteLine($"f {v1}//{v1} {v4}//{v4} {v3}//{v3}");
                    }
                }
            }
        }

        public static void ExportMaterial(string filePath, vec3 color)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("newmtl TubeMaterial");
                writer.WriteLine($"Kd {color.x} {color.y} {color.z}");
                writer.WriteLine("Ka 0.2 0.2 0.2");
                writer.WriteLine("Ks 1.0 1.0 1.0");
                writer.WriteLine("Ns 100");
            }
        }
    }


}