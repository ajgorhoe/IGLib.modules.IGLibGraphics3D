
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

using IG.Num;

namespace IGLib.Gr3D
{


    // Class for tubular surface generation
    public static class TubularMeshGenerator_02
    {
        // Function to generate a tubular mesh
        public static StructuredMesh3D_02 GenerateTube(
            Func<double, vec3> curve, double tStart, double tEnd, double radius,
            int numDivsT, int numDivsTheta)
        {
            // Generate parameter values
            double[] tValues = Enumerable.Range(0, numDivsT).Select(i => tStart + i * (tEnd - tStart) / (numDivsT - 1)).ToArray();
            double[] thetaValues = Enumerable.Range(0, numDivsTheta).Select(i => i * 2 * Math.PI / (numDivsTheta - 1)).ToArray();

            // Create the mesh storage
            StructuredMesh3D_02 mesh = new StructuredMesh3D_02(numDivsT, numDivsTheta, tValues, thetaValues);

            // Compute parallel transport frame
            List<(vec3 T, vec3 N, vec3 B)> frames = ComputeFrames(tValues, curve);

            // Generate tubular surface
            for (int i = 0; i < numDivsT; i++)
            {
                vec3 C_t = curve(tValues[i]);  // Spine curve point
                vec3 N = frames[i].N;
                vec3 B = frames[i].B;

                for (int j = 0; j < numDivsTheta; j++)
                {
                    double theta = thetaValues[j];
                    mesh.Nodes[i][j] = C_t + radius * (Math.Cos(theta) * N + Math.Sin(theta) * B);
                }
            }

            return mesh;
        }

        // Computes stable parallel transport frames
        private static List<(vec3 T, vec3 N, vec3 B)> ComputeFrames(double[] tValues, Func<double, vec3> curve)
        {
            List<(vec3 T, vec3 N, vec3 B)> frames = new List<(vec3 T, vec3 N, vec3 B)>();
            vec3 N0 = new vec3(1, 0, 0); // Initial normal
            vec3 prevT = new vec3(0, 0, 0);

            foreach (double t in tValues)
            {
                vec3 T = (curve(t + 1e-6) - curve(t)).Normalize();
                vec3 N = (frames.Count == 0) ? (N0 - T * vec3.Dot(N0, T)).Normalize() : (frames.Last().N - (T - prevT) * vec3.Dot(frames.Last().N, (T - prevT))).Normalize();
                vec3 B = vec3.Cross(T, N);
                frames.Add((T, N, B));
                prevT = T;
            }

            return frames;
        }
    }


}

