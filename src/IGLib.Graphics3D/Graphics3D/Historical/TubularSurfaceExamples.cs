using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using IG.Num;

namespace IGLib.Gr3D
{
    class TubularSurfaceExamples
    {


        public static void Example_06()
        {
            // Helix curve definition
            Func<double, vec3> helix = t => new vec3(Math.Cos(t), Math.Sin(t), 0.1 * t);

            // Create tubular mesh
            var mesh = TubularMeshGenerator_06.Global.GenerateMesh(helix, 0, 6 * Math.PI, 0.1, 100, 40);

            // File paths
            string objPath = "helix.obj";
            string mtlPath = "helix.mtl";
            string mtlFileName = "helix.mtl";

            // Surface material: LightSkyBlue (#87CEFA)
            var surfaceMaterial = new MaterialProperties
            {
                Name = "SurfaceLightSkyBlue",
                DiffuseColor = new vec3(135 / 255.0, 206 / 255.0, 250 / 255.0),
                AmbientColor = new vec3(0.2, 0.2, 0.3),
                SpecularColor = new vec3(1, 1, 1),
                Shininess = 80,
                Transparency = 1
            };

            // Wireframe material: DarkRed (#8B0000)
            var wireframeMaterial = new MaterialProperties
            {
                Name = "WireframeDarkRed",
                DiffuseColor = new vec3(139 / 255.0, 0, 0),
                AmbientColor = new vec3(0.1, 0.0, 0.0),
                SpecularColor = new vec3(0.5, 0.5, 0.5),
                Shininess = 10,
                Transparency = 1
            };

            // Export OBJ with wireframe and surfaces
            mesh.ExportMeshToObj(objPath, mtlFileName,
                exportSurfaces: true,
                exportWireframe: true,
                surfaceMaterialName: surfaceMaterial.Name,
                wireframeMaterialName: wireframeMaterial.Name);

            // Save both materials into same MTL file
            using (var writer = new StreamWriter(mtlPath))
            {
                writer.Write(surfaceMaterial.ToStringMtl());
                writer.WriteLine();
                writer.Write(wireframeMaterial.ToStringMtl());
            }

            Console.WriteLine("Helix mesh with surface and wireframe exported.");
        }
        

        public static void Example_03()
        {

            // Define a 3D helical curve
            Func<double, vec3> helix = t => new vec3(Math.Cos(t), Math.Sin(t), t / 5.0);

            // Generate the tubular mesh
            var mesh = TubularMeshGenerator_03.Generate(helix, 0, 6 * Math.PI, 0.1, 100, 20);

            // Export mesh and material file
            string objFile = "helix.obj";
            string mtlFile = "helix.mtl";

            mesh.ExportToObj(objFile, mtlFile);
            MeshExportExtensions_03.ExportMaterial(mtlFile, new vec3(0, 0, 1)); // Blue color

            Console.WriteLine("Helix mesh exported successfully!");

        }

        public static void Example_02()
        {
            Func<double, vec3> helix = t => new vec3(Math.Cos(t), Math.Sin(t), t / 5);
            StructuredMesh3D_02 tubeMesh = TubularMeshGenerator_02.GenerateTube(helix, 0, 10, 0.2, 100, 30);

            // Output a sample quadrilateral
            vec3[] quad = tubeMesh.GetQuadrilateral(10, 5);
            Console.WriteLine($"Quadrilateral at (10,5): {string.Join(", ", quad.Select(p => p.ToString()))}");
        }

        public static void Example_Export_02()
        {
            // Define a parametric helix curve
            Func<double, vec3> helix = t => new vec3(Math.Cos(t), Math.Sin(t), t / 5);

            // Generate the tubular mesh
            StructuredMesh3D_02 tubeMesh = TubularMeshGenerator_02.GenerateTube(helix, 0, 10, 0.2, 100, 30);

            // Export to OBJ (with an optional material file)
            tubeMesh.ExportToObj("tube.obj", "HelixMaterial", "tube.mtl");

            // Export to STL
            tubeMesh.ExportToStl("tube.stl");

            Console.WriteLine("Export complete: tube.obj and tube.stl");
        }

        public static void Example_01()
        {
            // Define parameters
            int numT = 100;  // Points along curve
            int numTheta = 30; // Points around cross-section
            double r = 0.2;  // Tube radius

            double[] tValues = Enumerable.Range(0, numT).Select(i => i * (10.0 / numT)).ToArray();
            double[] thetaValues = Enumerable.Range(0, numTheta).Select(i => i * (2 * Math.PI / numTheta)).ToArray();

            // Compute the parallel transport frame along the curve
            List<(vec3 T, vec3 N, vec3 B)> frames = TubularSurface_01.ComputeFrames(tValues);

            // Store points for the tubular surface
            List<vec3> tubePoints = new List<vec3>();

            for (int i = 0; i < tValues.Length; i++)
            {
                vec3 C_t = TubularSurface_01.Curve(tValues[i]); // Get curve position
                vec3 N = frames[i].N;
                vec3 B = frames[i].B;

                foreach (double theta in thetaValues)
                {
                    // Compute point on the circular cross-section
                    vec3 point = C_t + r * (Math.Cos(theta) * N + Math.Sin(theta) * B);
                    tubePoints.Add(point);
                }
            }

            // Output points to console for visualization (you can export this to a file or render it)
            foreach (var p in tubePoints)
            {
                Console.WriteLine($"({p.x:F3}, {p.y:F3}, {p.z:F3})");
            }

            Console.WriteLine("Tubular surface computed successfully!");
        }



    }

}

