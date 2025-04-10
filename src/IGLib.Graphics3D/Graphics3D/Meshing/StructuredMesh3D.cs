using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>Represents a 2D regular quadrilateral mesh in 3D space.</summary>
    public class StructuredMesh3D
    {

        /// <summary>Contains nodes of the mesh.</summary>
        public vec3[][] Nodes { get; private set; }

        /// <summary>Contains node normals of the mesh.</summary>
        public vec3[][] NodeNormals { get; private set; }

        /// <summary>Optional per-vertex RGBA colors (if defined, exported to GLTF as COLOR_0).
        /// Dimensions must match Nodes array.</summary>
        public ColorRGBA[][] VertexColors { get; set; }

        /// <summary>Number of nodes in the first direction.</summary>
        public int NumPoints1 { get; private set; }

        /// <summary>Number of nodes in the second direction.</summary>
        public int NumPoints2 { get; private set; }

        /// <summary>Parameter values in the first coordinate direction of the mesh only
        /// defined if the mesh is regular, i.e., if <see cref="IsRegular"/> = true).</summary>
        public double[] Params1 { get; private set; }

        /// <summary>Parameter values in the second coordinate direction of the mesh (only
        /// defined if the mesh is regular, i.e., if <see cref="IsRegular"/> = true).</summary>
        public double[] Params2 { get; private set; }

        /// <summary>True if the mesh is regular, i.e., 2D parameters of the mesh are arranged in
        /// a 2D array with equal spacinds.</summary>
        public bool IsRegular { get; private set; }

        public StructuredMesh3D(int numPoints1, int numPoints2, bool isRegular = true)
        {
            IsRegular = isRegular;
            NumPoints1 = numPoints1;
            NumPoints2 = numPoints2;
            AllocateNodes();
        }

        /// <summary>Allocate space for <see cref="Params1"/> and <see cref="Params2"/> , if not yet
        /// allocated, according to mesh sizes (<see cref="NumPoints1"/> and <see cref="NumPoints2"/>), 
        /// but only if <see cref="IsRegular"/> = true (otherwise, these arrays do not have any meaning).</summary>
        public void AllocateParams()
        {
            if (IsRegular)
            {
                if (Params1 == null)
                {
                    Params1 = new double[NumPoints1];
                }
                if (Params2 == null)
                {
                    Params2 = new double[NumPoints2];
                }
            }
        }

        /// <summary>Allocate space for <see cref="Nodes"/>, if not yet allocated, according
        /// to mesh sizes (<see cref="NumPoints1"/> and <see cref="NumPoints2"/>).</summary>
        public void AllocateNodes()
        {
            if (Nodes == null)
            {
                Nodes = new vec3[NumPoints1][];
                for (int i = 0; i < NumPoints1; i++)
                {
                    Nodes[i] = new vec3[NumPoints2];
                }
            }
            AllocateParams();
        }

        /// <summary>Allocate space for <see cref="NodeNormals"/>, if not yet allocated, according
        /// to mesh sizes (<see cref="NumPoints1"/> and <see cref="NumPoints2"/>).</summary>
        public void AllocateNodeNormals()
        {
            if (NodeNormals == null)
            {
                NodeNormals = new vec3[NumPoints1][];
                for (int i = 0; i < NumPoints1; i++)
                {
                    NodeNormals[i] = new vec3[NumPoints2];
                }
            }
            AllocateParams();
        }


        /// <summary>Allocate space for <see cref="VertexColors"/>, if not yet allocated, according
        /// to mesh sizes (<see cref="NumPoints1"/> and <see cref="NumPoints2"/>).</summary>
        public void AllocateVertexColors()
        {
            if (VertexColors == null)
            {
                VertexColors = new ColorRGBA[NumPoints1][];
                for (int i = 0; i < NumPoints1; i++)
                {
                    VertexColors[i] = new ColorRGBA[NumPoints2];
                }
            }
            AllocateParams();
        }

        public vec3 this[int i, int j] => Nodes[i][j];

        /// <summary>Returns the specified quadrilateral of the mesh, as array of 4 3D vectors
        /// containing coordinates of its vertices.</summary>
        /// <param name="i">Row index of the requested quatrilateral.</param>
        /// <param name="j">Column index of the requested quadrilateral.</param>
        /// <returns></returns>
        public (vec3, vec3, vec3, vec3) GetQuadrilateral(int i, int j)
        {
            return (Nodes[i][j], Nodes[i + 1][j], Nodes[i + 1][j + 1], Nodes[i][j + 1]);
        }

    }

}

