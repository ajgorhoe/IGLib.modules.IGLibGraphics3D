using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

using IG.Num;

namespace IGLib.Gr3D
{

    public static class TubularMeshGenerator_03
    {
        public static StructuredMesh3D_03 Generate(Func<double, vec3> curve, double tStart, double tEnd, double radius, int numT, int numTheta)
        {
            StructuredMesh3D_03 mesh = new StructuredMesh3D_03(numT, numTheta);
            double dt = (tEnd - tStart) / (numT - 1);
            double dTheta = 2 * Math.PI / (numTheta - 1);

            for (int i = 0; i < numT; i++)
            {
                double t = tStart + i * dt;
                vec3 p = curve(t);
                vec3 tangent = (curve(t + 0.0001) - p).Normalize();
                vec3 normal = (Math.Abs(tangent.z) < 0.9 ? new vec3(0, 0, 1) : new vec3(1, 0, 0)).Cross(tangent).Normalize();
                vec3 binormal = tangent.Cross(normal);

                mesh.Params1[i] = t;
                for (int j = 0; j < numTheta; j++)
                {
                    double theta = j * dTheta;
                    mesh.Params2[j] = theta;
                    vec3 point = p + normal * (radius * Math.Cos(theta)) + binormal * (radius * Math.Sin(theta));
                    mesh.Nodes[i][j] = point;
                    mesh.NodeNormals[i][j] = (normal * Math.Cos(theta) + binormal * Math.Sin(theta)).Normalize();
                }
            }
            return mesh;
        }
    }

}

