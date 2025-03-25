using System;
using static System.Math;

using IG.Num;

namespace IGLib.Gr3D
{



    /// <summary>Cartesian and cylindrical parameterization of a cylindrical billiard knot in 3D.
    /// <para>IMPORTANT: This parameterization is WRONG and needs to be corrected.</para>
    /// <para>Basis for implementation: <see href="https://mathcurve.com/courbes3d.gb/billardcylindrique/billardcylindrique.shtml"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/billardcylindrique/billardcylindrique.shtml">
    /// Cylindrical Billiard Knot.</seealso></para>
    /// 
    /// </summary>
    public class CylindricalBilliardKnot : Curve3DParameterizationFromCylindricalWithBounds,
        ICurve3DCylindricalParameterization, ICurve3DParameterizationWithBounds
    {
        public int N { get; }
        public int P { get; }
        public double A { get; }

        public CylindricalBilliardKnot(int n, int p, double a = 0.3)
        {
            if (n <= 0 || p <= 0)
                throw new ArgumentException("N and P must be positive integers.");
            if (a < 0)
                throw new ArgumentException("A must be non-negative.");
            N = n;
            P = p;
            A = a;
        }

        /// <inheritdoc/>
        public override (double rho, double phi, double z) CurveCylindrical(double t)
        {
            double rho = 1 + A * Cos(N * P * t);
            double phi = N * t;
            double z = Sin(P * t);
            return (rho, phi, z);
        }

        /// <inheritdoc/>
        public override (double rhoDerivative, double phiDerivative, double zDerivative) CurveDerivativeCylindrical(double t)
        {
            double rhoDeriv = -A * N * P * Sin(N * P * t);
            double phiDeriv = N;
            double zDeriv = P * Cos(P * t);
            return (rhoDeriv, phiDeriv, zDeriv);
        }

        /// <inheritdoc/>
        public override bool HasDerivativeCylindrical => true;

        /// <inheritdoc/>
        public override double StartParameter => 0;

        /// <inheritdoc/>
        public override double EndParameter => 2 * PI;
    }



    /// <summary>Cartesian and cylindrical parameterization of a cylindrical billiard knot in 3D.
    /// <para>IMPORTANT: This parameterization is WRONG, but it WILL BE KEPT because it produces
    /// interesting shapes. The other method, <see cref="CylindricalBilliardKnot"/>, should be corrected.</para></summary>
    /// <seealso cref="CylindricalBilliardKnot"/>
    public class CylindricalBilliardKnot_WrongParameterization : Curve3DParameterizationFromCylindricalWithBounds,
        ICurve3DCylindricalParameterization, ICurve3DParameterizationWithBounds
    {
        public int N { get; }
        public int P { get; }
        public double A { get; }

        public CylindricalBilliardKnot_WrongParameterization(int n, int p, double a = 0.3)
        {
            if (n <= 0 || p <= 0)
                throw new ArgumentException("N and P must be positive integers.");
            if (a < 0)
                throw new ArgumentException("A must be non-negative.");
            N = n;
            P = p;
            A = a;
        }

        /// <inheritdoc/>
        public override (double rho, double phi, double z) CurveCylindrical(double t)
        {
            double rho = 1 + A * Cos(N * P * t);
            double phi = N * t;
            double z = Sin(P * t);
            return (rho, phi, z);
        }

        /// <inheritdoc/>
        public override (double rhoDerivative, double phiDerivative, double zDerivative) CurveDerivativeCylindrical(double t)
        {
            double rhoDeriv = -A * N * P * Sin(N * P * t);
            double phiDeriv = N;
            double zDeriv = P * Cos(P * t);
            return (rhoDeriv, phiDeriv, zDeriv);
        }

        /// <inheritdoc/>
        public override bool HasDerivativeCylindrical => true;

        /// <inheritdoc/>
        public override double StartParameter => 0;

        /// <inheritdoc/>
        public override double EndParameter => 2 * PI;
    }
    



}
