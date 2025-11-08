
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

using IG.Num;

namespace IGLib.Gr3D
{


    // Class for a structured 3D quadrilateral mesh
    public class StructuredMesh3D_02
    {
        public vec3[][] Nodes { get; private set; } // 2D jagged array of nodes
        public double[] Params1 { get; private set; } // First parametric direction
        public double[] Params2 { get; private set; } // Second parametric direction
        public int NumPoints1 { get; private set; }  // Number of points along the curve
        public int NumPoints2 { get; private set; }  // Number of points along the circumference
        public bool IsRegular { get; private set; } = true; // Indicates structured nature

        public StructuredMesh3D_02(int numPoints1, int numPoints2, double[] params1, double[] params2)
        {
            NumPoints1 = numPoints1;
            NumPoints2 = numPoints2;
            Params1 = params1;
            Params2 = params2;
            Nodes = new vec3[numPoints1][];
            for (int i = 0; i < numPoints1; i++)
                Nodes[i] = new vec3[numPoints2];
        }

        // Indexer for accessing nodes
        public vec3 this[int i, int j] => Nodes[i][j];

        // Returns the four nodes forming the (i,j)-th quadrilateral
        public vec3[] GetQuadrilateral(int i, int j)
        {
            if (i < NumPoints1 - 1 && j < NumPoints2 - 1)
            {
                return new vec3[]
                {
                Nodes[i][j],
                Nodes[i + 1][j],
                Nodes[i + 1][j + 1],
                Nodes[i][j + 1]
                };
            }
            throw new IndexOutOfRangeException("Quadrilateral index out of bounds.");
        }
    }

}

