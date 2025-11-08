
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>
    /// Represents a structured quadrilateral mesh in 3D space.
    /// </summary>
    public class StructuredMesh3D_03
    {
        public vec3[][] Nodes { get; private set; }
        public vec3[][] NodeNormals { get; private set; }
        public int NumPoints1 { get; private set; }
        public int NumPoints2 { get; private set; }
        public double[] Params1 { get; private set; }
        public double[] Params2 { get; private set; }

        public bool IsRegular => Params1 != null && Params2 != null;

        public StructuredMesh3D_03(int numPoints1, int numPoints2)
        {
            NumPoints1 = numPoints1;
            NumPoints2 = numPoints2;
            Nodes = new vec3[numPoints1][];
            NodeNormals = new vec3[numPoints1][];
            for (int i = 0; i < numPoints1; i++)
            {
                Nodes[i] = new vec3[numPoints2];
                NodeNormals[i] = new vec3[numPoints2];
            }
            Params1 = new double[numPoints1];
            Params2 = new double[numPoints2];
        }
        public vec3 this[int i, int j] => Nodes[i][j];

        public (vec3, vec3, vec3, vec3) GetQuadrilateral(int i, int j)
        {
            return (Nodes[i][j], Nodes[i + 1][j], Nodes[i + 1][j + 1], Nodes[i][j + 1]);
        }
    }

}

