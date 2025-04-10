using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using IGLib.Gr3D;
using IG.Num;

namespace IGLib.Gr3D
{
    public class ExampesGraphics3D
    {

        public static void ExportHelixTubularMeshToGLTF()
        {
            // Curve definition: 3D helix
            Func<double, vec3> helix = t => new vec3(Math.Cos(t), Math.Sin(t), 0.1 * t);

            double tStart = 0;
            double tEnd = 6 * Math.PI;
            int numT = 100;
            int numTheta = 40;

            // Generate mesh
            var mesh = TubularMeshGenerator.Global.GenerateMesh(helix, tStart, tEnd, 0.1, numT, numTheta);

            // Assign vertex colors
            mesh.VertexColors = new ColorRGBA[numT][];
            for (int i = 0; i < numT; i++)
            {
                mesh.VertexColors[i] = new ColorRGBA[numTheta];
                double tVal = tStart + i * (tEnd - tStart) / (numT - 1);

                // Calculate smooth gradient from t
                var color = GetColorFromT(tVal, tStart, tEnd);

                for (int j = 0; j < numTheta; j++)
                {
                    mesh.VertexColors[i][j] = color;
                }
            }

            // Define material
            var material = new MaterialProperties
            {
                Name = "RainbowSurface",
                DiffuseColor = new vec3(1, 1, 1),  // Base color won't affect per-vertex colors
                SpecularColor = new vec3(1, 1, 1),
                AmbientColor = new vec3(0.1, 0.1, 0.1),
                Shininess = 50,
                Transparency = 1
            };

            // Optionally define light source
            var light = new LightSource(LightType.Point)
            {
                Color = new vec3(1, 1, 1),
                Intensity = 1.0,
                Position = new vec3(3, 3, 3),
                Direction = new vec3(0, 0, -1)
            };

            // Export to GLTF
            string path = "rainbow_helix.gltf";
            mesh.ExportToGltf(path, material, new[] { light });

            Console.WriteLine("Rainbow helix exported to: " + path);
        }

        /// <summary>
        /// Returns RGBA color for a normalized parameter t.
        /// Gradient: blue → green → red.
        /// </summary>
        public static ColorRGBA GetColorFromT(double t, double tMin, double tMax)
        {
            double u = (t - tMin) / (tMax - tMin); // Normalize [0, 1]
            float r = 0, g = 0, b = 0;

            if (u < 0.5)
            {
                double v = u * 2; // [0, 1]
                r = 0f;
                g = (float)v;
                b = (float)(1 - v);
            }
            else
            {
                double v = (u - 0.5) * 2; // [0, 1]
                r = (float)v;
                g = (float)(1 - v);
                b = 0f;
            }

            return new ColorRGBA(r, g, b, 1.0f);
        }
    }


}
