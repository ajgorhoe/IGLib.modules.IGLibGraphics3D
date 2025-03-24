using System;
using static System.Math;

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>Cartesion parameterization of a cylindrical billiard knot.</summary>
    public class CylindricalBilliardKnot: Curve3DParameterizationFromCylindricalWithBounds,
        ICurve3DCylindricalParameterization, ICurve3DParameterizationWithBounds
    {

        #region ICurve3DCylindricalParameterization

        /// <inheritdoc/>
        public override (double rho, double phi, double z) CurveCylindrical(double t)
        {
            // ToDo: implement this!
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override (double rhoDerivative, double phiDerivative, double zDerivative) CurveDerivativeCylindrical(double t)
        {
            // ToDo: implement this!
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override bool HasDerivativeCylindrical => true;

        /// <inheritdoc/>
        public override double StartParameter => 0;

        /// <inheritdoc/>
        public override double EndParameter => 2 * PI;

        #endregion ICurve3DCylindricalParameterization

    }

}
