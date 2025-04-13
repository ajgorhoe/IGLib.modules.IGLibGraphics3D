using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>Generates regular (one set of parameter values for each independent variable) structured (nodes
    /// can be indexed by 2 indices) meshes of parametric surfaces defined by 2 parameters with specified bounds.
    /// </summary>
    public class ParametricSurfaceMeshGenerator
    {


        protected static Lazy<ParametricSurfaceMeshGenerator> _global = new Lazy<ParametricSurfaceMeshGenerator>(
            () => new ParametricSurfaceMeshGenerator());

        /// <summary>Global lazily initialized instance of the mesh generator.</summary>
        public static ParametricSurfaceMeshGenerator Global => _global.Value;


        /// <summary>Generates a mesh of the parametric surface using numerically calculated tangent vectors.</summary>
        /// <param name="surface">Definition of the parametric surface used to generate the mesh.</param>
        /// <param name="tStart1">Starting value of surface parameter 1.</param>
        /// <param name="tEnd1">End value of surface parameter 1.</param>
        /// <param name="tStart2">Starting value of surface parameter 2.</param>
        /// <param name="tEnd2">End value of surface parameter 2.</param>
        /// <param name="numPoints1">Number of mesh points along the first parameter.</param>
        /// <param name="numPoints2">Number of mesh points along the second parameter.</param>
        /// <param name="hrel1">Specification of step for numerical differentiation, relative to the 
        /// meshing step (difference in parameter between meshing points).</param>
        /// <param name="eps">Specifies how close to parallel to a tangent can a unit vector be at most,
        /// to still be eligible to generate the first normal to the tangent via orthogonalization.
        /// Should be less than 0.5; normally it should be less than 0.2, also 0.05 could be 
        /// a good value. If scalar product of the tangential unit vector and the candidate unit 
        /// vector minus 1.0 is less than <paramref name="eps"/> then the candidate vector is not taken 
        /// into account because it is too parallel to the tangent.</param>
        /// <param name="normalizeFromPrevious">If true then the second normal is calculated from previous
        /// normal by orthogonalization. Otherwise, it is calculated by a vector product of the tangent
        /// and the first normal. Default is false.</param>
        /// <param name="restrictToInterval">Whether evaluations of the curve points must be limited 
        /// to the meshing interval. If true then the step for numerical differentiation must be taken
        /// in such a way that all vurve evaluations fall within the meshing interval, end ponts 
        /// inclusive. Ths is useful when the function has a singularity or sidcontinuity just
        /// outside the meshing interval.</param>
        public StructuredSurfaceMesh3D GenerateMesh(
            Func<double, vec3> surface,
            double tStart1,
            double tEnd1,
            double radius,
            int numPoints1,
            int numPoints2,
            double hrel1 = 1e-2,
            double eps = 0.1,
            bool normalizeFromPrevious = false,
            bool restrictToInterval = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>Generates a tubular mesh using the Parallel Transport Frame (PTF) and analytical 
        /// tangent vectors.</summary>
        /// <param name="tangent">Function that defines derivative of <paramref name="curve"/> with
        /// respect to parameter.</param>
        /// <remarks>For undocumented parameters, see 
        /// <see cref="GenerateMesh(Func{double, vec3}, double, double, double, int, int, double, double, bool, bool)"/>.
        /// </remarks>
        public StructuredSurfaceMesh3D GenerateMesh(
            Func<double, vec3> curve,
            Func<double, vec3> tangent,
            double tStart,
            double tEnd,
            double radius,
            int numCurvePoints,
            int numCirclePoints,
            double eps = 0.1,
            bool normalizeFromPrevious = false)
        {
            throw new NotSupportedException();
        }

    }

}

