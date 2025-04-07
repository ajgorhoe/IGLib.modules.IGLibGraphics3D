using System;
using static System.Math;

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>Cartesion parameterization of a curve in 3D. Beside the <see cref="Curve"/> and 
    /// (optional) <see cref="CurveDerivative"/>, it also contains bounds on parameter, <see cref="StartParameter"/>
    /// and <see cref="EndParameter"/> as part of the class.
    /// <para><see cref="StartParameter"/> and <see cref="EndParameter"/> usually denote bounds on curve
    /// parameter that are used by default when the curve is tabulated, plotted, for meshes or for
    /// numerical calculations (i.e. lengths or urfaces or volumes where the curve is involved). Sometimes,
    /// these properties can also represent the range of parameter values for which the curve is defined,
    /// though rarely. More often, they represent some characteristic or natural range for the curve in 
    /// question, and sometimes theey are not used at all (the convention is to set both to 0 in such cases).
    /// For mathematical knots (slosed ones), these properties are usually set to such values that the
    /// represented curves exactly closes eginning with its end when parameter runs from 
    /// <see cref="StartParameter"/> and <see cref="EndParameter"/>.</para></summary>
    public abstract class Curve3DParameterizationFromCylindricalWithBounds :
        ICurve3DCylindricalParameterization, ICurve3DParameterizationWithBounds
    {

        #region ICurve3DCylindricalParameterization

        /// <inheritdoc/>
        public abstract (double rho, double phi, double z) CurveCylindrical(double t);

        /// <inheritdoc/>
        public abstract (double rhoDerivative, double phiDerivative, double zDerivative) CurveDerivativeCylindrical(double t);

        /// <inheritdoc/>
        public abstract bool HasDerivativeCylindrical { get; }

        /// <inheritdoc/>
        public abstract double StartParameter { get; }

        /// <inheritdoc/>
        public abstract double EndParameter { get; }


        #endregion ICurve3DCylindricalParameterization

        #region ICurve3DParameterizationWithBounds

        /// <inheritdoc/>
        public virtual vec3 Curve(double t)
        {
            (double rho, double phi, double z) = CurveCylindrical(t);
            return new vec3(
                rho * Cos(phi),
                rho * Sin(phi),
                z );
        }

        /// <inheritdoc/>
        public virtual vec3 CurveDerivative(double t)
        {
            if (!HasDerivative)
            {
                return new vec3(0, 0, 0);
            }
            (double rho, double phi, double z) = CurveCylindrical(t);
            (double rhoDerivative, double phiDerivative, double zDerivative)
                = CurveDerivativeCylindrical(t);
            // Use chain rule, derivative of cartesian coordinates with respect to cylindrical ones,
            // and derivatives with respect to parameter t in cylindrical coordinates to calculate
            // derivatives in cartesian coordinates:
            return new vec3(
                Cos(phi) * rhoDerivative - rho * Sin(phi) * phiDerivative,
                Sin(phi) * rhoDerivative + rho * Cos(phi) * phiDerivative,
                zDerivative);
        }

        /// <inheritdoc/>
        public virtual bool HasDerivative => HasDerivativeCylindrical;

        #endregion ICurve3DParameterizationWithBounds

    }

}
