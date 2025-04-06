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
    public class TubularMeshGenerator
    {
        /// <summary>
        /// Global lazy-initialized instance of the mesh generator.
        /// </summary>
        public static TubularMeshGenerator Global { get; } = new TubularMeshGenerator();

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

        /// <summary>
        /// Generates a tubular mesh using the Parallel Transport Frame (PTF) and numerical tangent vectors.
        /// </summary>
        public StructuredMesh3D GeneratePTF(
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
            return GeneratePTF(curve, tangent, tStart, tEnd, radius, numCurvePoints, numCirclePoints, eps, normalizeFromPrevious);
        }

        /// <summary>
        /// Generates a tubular mesh using the Parallel Transport Frame (PTF) and analytical tangent vectors.
        /// </summary>
        public StructuredMesh3D GeneratePTF(
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
                return GeneratePTF(curve, tStart, tEnd, radius, numCurvePoints, numCirclePoints, hrel: 1e-2, eps: eps, normalizeFromPrevious: normalizeFromPrevious, restrictToInterval: true);
            }

            var mesh = new StructuredMesh3D(numCurvePoints, numCirclePoints);

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
                    mesh.Nodes[i][j] = point + radius * radial;
                    mesh.NodeNormals[i][j] = radial;
                }
            }

            return mesh;
        }

        #endregion

        #region Frenet Frame

        /// <summary>
        /// Generates a tubular mesh using the Frenet frame and numerical tangent vector.
        /// </summary>
        public StructuredMesh3D GenerateFrenet(
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
            return GenerateFrenet(curve, tangent, tStart, tEnd, radius, numCurvePoints, numCirclePoints, h);
        }

        /// <summary>
        /// Generates a tubular mesh using the Frenet frame and analytical or fallback tangent function.
        /// </summary>
        public StructuredMesh3D GenerateFrenet(
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

            var mesh = new StructuredMesh3D(numCurvePoints, numCirclePoints);

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
                    mesh.Nodes[i][j] = p + radius * radial;
                    mesh.NodeNormals[i][j] = radial;
                }
            }

            return mesh;
        }

        #endregion
    }

}

