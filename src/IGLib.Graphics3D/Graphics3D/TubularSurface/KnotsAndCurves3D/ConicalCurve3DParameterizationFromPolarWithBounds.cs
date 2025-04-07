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
    public abstract class ConicalCurve3DParameterizationFromPolarWithBounds :
        ICurve2DPolarParameterization, ICurve3DParameterizationWithBounds
    {

        public ConicalCurve3DParameterizationFromPolarWithBounds(double alpha, bool isDefinedForNegativePhi = false)
        {
            this.alpha = alpha;
            IsNegativePhiOnMirrorCone = isDefinedForNegativePhi;
        }

        /// <summary>Slope of the cone around which the conical curve is wound (tangent of the angle between
        /// the slope of the conee and the XY plane).</summary>
        public double alpha { get; init; }

        /// <summary>If true then the curve for negative azimuth angles φ extends to wrap arround the mirror 
        /// cone of the original cone. This is true e.g. for Archimedean and Fermat's spirals, but not for the
        /// logarithmic or hypervolic spirals as the base curve.</summary>
        public bool IsNegativePhiOnMirrorCone { get; init; }

        #region ICurve2DPolarParameterization

        /// <inheritdoc/>
        public abstract double CurvePolar(double t);

        /// <inheritdoc/>
        public abstract double CurveDerivativePolar(double t);

        /// <inheritdoc/>
        public abstract bool HasDerivativePolar{ get; }

        /// <inheritdoc/>
        public abstract double StartParameter { get; }

        /// <inheritdoc/>
        public abstract double EndParameter { get; }

        #endregion ICurve2DPolarParameterization


        #region ICurve3DParameterizationWithBounds

        /// <inheritdoc/>
        public virtual vec3 Curve(double t)
        {
            double r = CurvePolar(t);
            if (t < 0 && IsNegativePhiOnMirrorCone)
            {

                vec3 positiveCurve = Curve(-t);
                return new vec3(
                    positiveCurve.x,
                    positiveCurve.y,
                    -positiveCurve.z );
            }
            return new vec3(
                r * Cos(t),
                r * Sin(t),
                0 + alpha * r );
        }

        /// <inheritdoc/>
        public virtual vec3 CurveDerivative(double t)
        {
            if (!HasDerivative)
            {
                return new vec3(0, 0, 0);
            }
            double r = CurvePolar(t);
            double rDerivative = CurveDerivativePolar(t);
            // Use chain rule, derivative of r with respect to phi of the planar curve in polar coordinates,
            // and derivatives with respect to parameter t of cylindrical coordinates to calculate
            // derivatives in cartesian coordinates:
            return new vec3(
                - r * Sin(t) + rDerivative * Cos(t),
                r * Cos(t) + rDerivative * Sin(t),
                alpha * rDerivative );
        }

        /// <inheritdoc/>
        public virtual bool HasDerivative => HasDerivativePolar;

        #endregion ICurve3DParameterizationWithBounds

    }

}
