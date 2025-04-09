using IG.Num;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

// using IG.Num;

namespace IGLib.Gr3D
{

    public class TubularSurface_01
    {
        // Define the 3D curve: Helix (change this for other shapes)
        public static vec3 Curve(double t) => new vec3(Math.Cos(t), Math.Sin(t), t / 5);

        // Compute the tangent vector to the curve
        static vec3 Tangent(double t) => new vec3(-Math.Sin(t), Math.Cos(t), 1 / 5).Normalize();

        // Compute a numerically stable parallel transport frame along the curve
        public static List<(vec3 T, vec3 N, vec3 B)> ComputeFrames(double[] tValues)
        {
            List<(vec3 T, vec3 N, vec3 B)> frames = new List<(vec3 T, vec3 N, vec3 B)>();

            vec3 N0 = new vec3(1, 0, 0); // Initial normal vector (arbitrary)
            vec3 prevT = new vec3(0, 0, 0); // Store previous tangent

            foreach (double t in tValues)
            {
                vec3 T = Tangent(t); // Compute current tangent

                vec3 N;
                if (frames.Count == 0)  // First frame
                {
                    N = N0 - T * vec3.Dot(N0, T);
                }
                else  // Parallel transport
                {
                    vec3 dT = T - prevT;
                    N = frames.Last().N - dT * vec3.Dot(frames.Last().N, dT);
                }

                N = N.Normalize();
                vec3 B = vec3.Cross(T, N); // Compute binormal

                frames.Add((T, N, B));
                prevT = T;
            }

            return frames;
        }

    }

}

