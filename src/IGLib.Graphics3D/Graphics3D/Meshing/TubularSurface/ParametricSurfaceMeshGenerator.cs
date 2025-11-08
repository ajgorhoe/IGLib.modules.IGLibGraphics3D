
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

using IG.Num;

namespace IGLib.Gr3D
{


    using System;

    namespace IGLib.Gr3D
    {

        /// <summary>Generates regular structured meshes of parametric surfaces in 3D space.</summary>
        public class ParametricSurfaceMeshGenerator : IParametricSurfaceMeshGenerator
        {
            protected static Lazy<ParametricSurfaceMeshGenerator> _global = new Lazy<ParametricSurfaceMeshGenerator>(() => new ParametricSurfaceMeshGenerator());

            /// <summary>Global lazily initialized instance of the mesh generator.</summary>
            public static ParametricSurfaceMeshGenerator Global => _global.Value;

            /// <summary>
            /// Numerically computes the partial derivative of a 3D function with respect to a parameter.
            /// </summary>
            protected vec3 NumericalDerivative(Func<double, double, vec3> f, double u, double v, double h, bool respectFirst, double uMin, double uMax, double vMin, double vMax, bool restrictToInterval)
            {
                if (respectFirst)
                {
                    double u1 = u - h, u2 = u + h;
                    if (restrictToInterval)
                    {
                        if (u1 < uMin) u1 = u;
                        if (u2 > uMax) u2 = u;
                    }
                    return (f(u2, v) - f(u1, v)) * (1.0 / (u2 - u1));
                }
                else
                {
                    double v1 = v - h, v2 = v + h;
                    if (restrictToInterval)
                    {
                        if (v1 < vMin) v1 = v;
                        if (v2 > vMax) v2 = v;
                    }
                    return (f(u, v2) - f(u, v1)) * (1.0 / (v2 - v1));
                }
            }

            /// <inheritdoc/>
            public StructuredSurfaceMesh3D GenerateMesh(
                Func<double, double, vec3> surface,
                double tStart1,
                double tEnd1,
                double tStart2,
                double tEnd2,
                int numPoints1,
                int numPoints2,
                double hrel1 = 1e-2,
                bool restrictToInterval = false)
            {
                double du = (tEnd1 - tStart1) / (numPoints1 - 1);
                double dv = (tEnd2 - tStart2) / (numPoints2 - 1);
                double hU = hrel1 * du;
                double hV = hrel1 * dv;

                var mesh = new StructuredSurfaceMesh3D(numPoints1, numPoints2);

                for (int i = 0; i < numPoints1; i++)
                {
                    double u = tStart1 + i * du;
                    mesh.Params1[i] = u;

                    for (int j = 0; j < numPoints2; j++)
                    {
                        double v = tStart2 + j * dv;
                        mesh.Params2[j] = v;

                        vec3 p = surface(u, v);
                        vec3 Su = NumericalDerivative(surface, u, v, hU, true, tStart1, tEnd1, tStart2, tEnd2, restrictToInterval);
                        vec3 Sv = NumericalDerivative(surface, u, v, hV, false, tStart1, tEnd1, tStart2, tEnd2, restrictToInterval);
                        vec3 normal = vec3.Cross(Su, Sv).Normalize();

                        mesh.Vertices[i][j] = p;
                        mesh.VertexNormals[i][j] = normal;
                    }
                }

                return mesh;
            }

            /// <inheritdoc/>
            public StructuredSurfaceMesh3D GenerateMesh(
                Func<double, double, vec3> surface,
                Func<double, double, vec3> tangent1,
                Func<double, double, vec3> tangent2,
                double tStart1,
                double tEnd1,
                double tStart2,
                double tEnd2,
                int numPoints1,
                int numPoints2)
            {
                if (tangent1 == null || tangent2 == null)
                {
                    throw new ArgumentNullException("Both tangent1 and tangent2 must be provided for analytical mode.");
                }

                var mesh = new StructuredSurfaceMesh3D(numPoints1, numPoints2);
                double du = (tEnd1 - tStart1) / (numPoints1 - 1);
                double dv = (tEnd2 - tStart2) / (numPoints2 - 1);

                for (int i = 0; i < numPoints1; i++)
                {
                    double u = tStart1 + i * du;
                    mesh.Params1[i] = u;

                    for (int j = 0; j < numPoints2; j++)
                    {
                        double v = tStart2 + j * dv;
                        mesh.Params2[j] = v;

                        vec3 p = surface(u, v);
                        vec3 Su = tangent1(u, v);
                        vec3 Sv = tangent2(u, v);
                        vec3 normal = vec3.Cross(Su, Sv).Normalize();

                        mesh.Vertices[i][j] = p;
                        mesh.VertexNormals[i][j] = normal;
                    }
                }

                return mesh;
            }
        }
    }




    /// <summary>Generates regular (one set of parameter values for each independent variable) structured (nodes
    /// can be indexed by 2 indices) meshes of parametric surfaces defined by 3D vector functions of 2 parameters,
    /// with specified bounds.
    /// </summary>
    public class ParametricSurfaceMeshGeneratorNotImplemented : IParametricSurfaceMeshGenerator
    {


        protected static Lazy<ParametricSurfaceMeshGeneratorNotImplemented> _global = new Lazy<ParametricSurfaceMeshGeneratorNotImplemented>(
            () => new ParametricSurfaceMeshGeneratorNotImplemented());

        /// <summary>Global lazily initialized instance of the mesh generator.</summary>
        public static ParametricSurfaceMeshGeneratorNotImplemented Global => _global.Value;


        /// <summary>Generates a mesh of the parametric surface using numerically calculated tangent vectors.</summary>
        /// <param name="surface">Definition of the parametric surface used to generate the mesh.</param>
        /// <param name="tStart1">Starting value of surface parameter 1.</param>
        /// <param name="tEnd1">End value of surface parameter 1.</param>
        /// <param name="tStart2">Starting value of surface parameter 2.</param>
        /// <param name="tEnd2">End value of surface parameter 2.</param>
        /// <param name="numPoints1">Number of mesh points along the first parameter.</param>
        /// <param name="numPoints2">Number of mesh points along the second parameter.</param>
        /// <param name="hrel1">Specification of step for numerical differentiation, relative to the 
        /// meshing step (difference in parameter between successive meshing points).</param>
        /// <param name="restrictToInterval">Whether evaluations of the curve points must be limited 
        /// to the meshing interval. If true then the step for numerical differentiation must be taken
        /// in such a way that all surface evaluations fall within the meshing interval, end ponts 
        /// inclusive. Ths is useful when the function has a singularity or discontinuity just
        /// outside the meshing interval.</param>
        public StructuredSurfaceMesh3D GenerateMesh(
            Func<double, double, vec3> surface,
            double tStart1,
            double tEnd1,
            double tStart2,
            double tEnd2,
            int numPoints1,
            int numPoints2,
            double hrel1 = 1e-2,
            bool restrictToInterval = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>Generates a tubular mesh using the Parallel Transport Frame (PTF) and analytical 
        /// tangent vectors.</summary>
        /// <param name="tangent1">Function that defines derivative of <paramref name="surface"/> with
        /// respect to parameter.</param>
        /// <remarks>For undocumented parameters, see 
        /// <see cref="GenerateMesh(Func{double, double, vec3}, double, double, double, double, int, int, double, bool)"/>.
        /// </remarks>
        public StructuredSurfaceMesh3D GenerateMesh(
            Func<double, double, vec3> surface,
            Func<double, double, vec3> tangent1,
            Func<double, double, vec3> tangent2,
            double tStart1,
            double tEnd1,
            double tStart2,
            double tEnd2,
            int numPoints1,
            int numPoints2)
        {
            throw new NotSupportedException();
        }

    }

}

