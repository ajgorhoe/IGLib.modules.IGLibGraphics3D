using static System.Math;

using IG.Num;
using System.Collections.Generic;

namespace IGLib.Gr3D
{

    /// <summary>A 3D parametric curve with bounds - the conical hyperbolic spiral.
    /// <para>This conical spiral is based on the hyperbolic spiral r = r(φ) = a / φ.</para>
    /// <para>Parameters: <see cref="a"/>, <see cref="k"/>, <see cref="ConicalCurve3DParameterizationFromPolarWithBounds.alpha"/></para>
    /// <para>Basis for implementation: <see href="https://en.wikipedia.org/wiki/Conical_spiral"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Hyperbolic_spiral">
    /// Cylindrical Billiard Knot.</seealso></para></summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class ConicalSpiralHyperbolic3D : ConicalCurve3DParameterizationFromPolarWithBounds
    {

        /// <summary>Constructor.</summary>
        /// <param name="alpha">Slope of the cone linew with respect to the x-y plane, specifies the property <see cref="ConicalCurve3DParameterizationFromPolarWithBounds.alpha"/>.</param>
        /// <param name="a">Coefficient of the Hyperbolic spiral, defines the property <see cref="a"/>.</param>
        public ConicalSpiralHyperbolic3D(double alpha, double a) : 
            base(alpha, isDefinedForNegativePhi: true)
        {
            this.a = a;
        }

        /// <summary>Parameter of the Logarithmic spiral, coefficient in its equation r(φ) = a / φ.</summary>
        public double a { get; }


        #region ICurve2DPolarParameterization

        /// <inheritdoc/>
        public override double CurvePolar(double t) => a / t;

        /// <inheritdoc/>
        public override double CurveDerivativePolar(double t) => -a / (t*t);

        /// <inheritdoc/>
        public override bool HasDerivativePolar => true;

        /// <inheritdoc/>
        public override double StartParameter => 1.0 / (1 * PI);

        /// <inheritdoc/>
        public override double EndParameter => 10 * PI;

        #endregion ICurve2DPolarParameterization

    }
}
