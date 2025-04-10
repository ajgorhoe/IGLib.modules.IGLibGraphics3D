using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;  // For Math functions

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>
    /// Generates tubular surface meshes around space curves using Frenet or Parallel Transport Frames.
    /// </summary>
    public class TubularMeshGenerator_06
    {
        /// <summary>
        /// Global lazy-initialized instance of the mesh generator.
        /// </summary>
        public static TubularMeshGenerator_06 Global { get; } = new TubularMeshGenerator_06();

        /// <summary>
        /// Numerically computes the derivative of a curve using central differences.
        /// </summary>
        public vec3 NumericalDerivative(Func<double, vec3> curve, double t, double tMin, double tMax, double h, bool restrictToInterval)
        {
            double t1 = t - h, t2 = t + h;
            if (restrictToInterval)
            {
                if (t1 < tMin) t1 = t;
                if (t2 > tMax) t2 = t;
            }
            return (curve(t2) - curve(t1)) * (1.0 / (t2 - t1));
        }

        #region Parallel Transport Frame

        /// <summary>Generates a tubular mesh using the Parallel Transport Frame (PTF) and 
        /// numerical tangent vectors.</summary>
        /// <param name="curve">Definition of the parametric curve used to generate the tubular surface.</param>
        /// <param name="tStart">Starting parameter.</param>
        /// <param name="tEnd">Ending parameter.</param>
        /// <param name="radius">Radius of the circular tube.</param>
        /// <param name="numCurvePoints">Number of mesh points along the curve.</param>
        /// <param name="numCirclePoints">Number of mesh points around the circumference of the tube.</param>
        /// <param name="hrel">Specification of step for numerical differentiation, relative to the 
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
            Func<double, vec3> curve,
            double tStart,
            double tEnd,
            double radius,
            int numCurvePoints,
            int numCirclePoints,
            double hrel = 1e-2,
            double eps = 0.1,
            bool normalizeFromPrevious = false,
            bool restrictToInterval = false)
        {
            double step = (tEnd - tStart) / (numCurvePoints - 1);
            double h = hrel * step;

            Func<double, vec3> tangent = t => NumericalDerivative(curve, t, tStart, tEnd, h, restrictToInterval);
            return GenerateMesh(curve, tangent, tStart, tEnd, radius, numCurvePoints, numCirclePoints, eps, normalizeFromPrevious);
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
            if (tangent == null)
            {
                double step = (tEnd - tStart) / (numCurvePoints - 1);
                return GenerateMesh(curve, tStart, tEnd, radius, numCurvePoints, numCirclePoints, hrel: 1e-2, eps: eps, normalizeFromPrevious: normalizeFromPrevious, restrictToInterval: true);
            }

            var mesh = new StructuredSurfaceMesh3D(numCurvePoints, numCirclePoints);

            vec3[] unitAxes = {
            new vec3(1, 0, 0),
            new vec3(0, 1, 0),
            new vec3(0, 0, 1)
        };

            vec3 prevN = new vec3();
            vec3 prevB = new vec3();

            for (int i = 0; i < numCurvePoints; ++i)
            {
                double t = tStart + i * (tEnd - tStart) / (numCurvePoints - 1);
                mesh.Params1[i] = t;

                double angleStep = 2 * Math.PI / (numCirclePoints - 1);
                vec3 point = curve(t);
                vec3 T = tangent(t).Normalize();

                vec3 N, B;

                if (i == 0)
                {
                    vec3 refAxis = vec3.Dot(T, unitAxes[2]) < (1.0 - eps) ? unitAxes[2] : unitAxes[1];
                    N = (refAxis - T * vec3.Dot(refAxis, T)).Normalize();
                    B = vec3.Cross(T, N).Normalize();
                    prevN = N;
                    prevB = B;
                }
                else
                {
                    N = (prevN - T * vec3.Dot(prevN, T)).Normalize();
                    B = normalizeFromPrevious
                        ? (prevB - T * vec3.Dot(prevB, T) - N * vec3.Dot(prevB, N)).Normalize()
                        : vec3.Cross(T, N).Normalize();

                    prevN = N;
                    prevB = B;
                }

                for (int j = 0; j < numCirclePoints; ++j)
                {
                    double theta = j * angleStep;
                    mesh.Params2[j] = theta;

                    vec3 radial = Math.Cos(theta) * N + Math.Sin(theta) * B;
                    mesh.Vertices[i][j] = point + radius * radial;
                    mesh.VertexNormals[i][j] = radial;
                }
            }

            return mesh;
        }

        #endregion

        #region Frenet Frame

        /// <summary>Generates a tubular mesh from the specified parametric curve using the Frenet frame 
        /// and numerically calculated tangent vector.
        /// <para>This method of generating the mesh will lead to a "pinched pipe mesh" in ponts where the
        /// swcond derivative changes direction because the normals that define meshning around circumberence
        /// change direections by full angle (180 degrees).</para>
        /// </summary>
        /// <remarks>For parameter descriprions, see 
        /// <see cref="GenerateMesh(Func{double, vec3}, double, double, double, int, int, double, double, bool, bool)"/>.</remarks>
        public StructuredSurfaceMesh3D GenerateFrenet(
            Func<double, vec3> curve,
            double tStart,
            double tEnd,
            double radius,
            int numCurvePoints,
            int numCirclePoints,
            double hrel = 1e-3,
            bool restrictToInterval = false)
        {
            double step = (tEnd - tStart) / (numCurvePoints - 1);
            double h = hrel * step;
            Func<double, vec3> tangent = t => NumericalDerivative(curve, t, tStart, tEnd, h, restrictToInterval);
            return GenerateMeshByFrenet(curve, tangent, tStart, tEnd, radius, numCurvePoints, numCirclePoints, h);
        }

        /// <summary>Generates a tubular mesh using the Frenet frame and analytical or fallback tangent function.
        /// <para>The same as <see cref="GenerateMeshByFrenet(Func{double, vec3}, Func{double, vec3}, double, double, double, int, int, double)"/>,
        /// except that analytical curve derivative (tangent) is used (specified by <paramref name="tangent"/>) and we
        /// don't need to calculate derivatives numerically.</para></summary>
        /// <remarks>For parameter descriprions, see 
        /// <see cref="GenerateMesh(Func{double, vec3}, double, double, double, int, int, double, double, bool, bool)"/>.</remarks>
        public StructuredSurfaceMesh3D GenerateMeshByFrenet(
            Func<double, vec3> curve,
            Func<double, vec3> tangent,
            double tStart,
            double tEnd,
            double radius,
            int numCurvePoints,
            int numCirclePoints,
            double h = 1e-3)
        {
            if (tangent == null)
            {
                double step = (tEnd - tStart) / (numCurvePoints - 1);
                double hrel = h / step;
                return GenerateFrenet(curve, tStart, tEnd, radius, numCurvePoints, numCirclePoints, hrel, restrictToInterval: true);
            }

            var mesh = new StructuredSurfaceMesh3D(numCurvePoints, numCirclePoints);

            for (int i = 0; i < numCurvePoints; ++i)
            {
                double t = tStart + i * (tEnd - tStart) / (numCurvePoints - 1);
                mesh.Params1[i] = t;

                double angleStep = 2 * Math.PI / (numCirclePoints - 1);
                vec3 p = curve(t);
                vec3 T = tangent(t).Normalize();
                vec3 T2 = NumericalDerivative(tangent, t, tStart, tEnd, h, true);
                vec3 N = (T2 - T * vec3.Dot(T2, T)).Normalize();
                vec3 B = vec3.Cross(T, N).Normalize();

                for (int j = 0; j < numCirclePoints; ++j)
                {
                    double theta = j * angleStep;
                    mesh.Params2[j] = theta;

                    vec3 radial = Math.Cos(theta) * N + Math.Sin(theta) * B;
                    mesh.Vertices[i][j] = p + radius * radial;
                    mesh.VertexNormals[i][j] = radial;
                }
            }

            return mesh;
        }

        #endregion
    
    }

}

