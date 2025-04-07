using static System.Math;

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>A 3D parametric curve with bounds - the conical logarithmic spiral.
    /// <para>This conical spiral is based on the Logarithmic spiral r = r(φ) = a * exp(k * φ).</para>
    /// <para>Parameters: <see cref="a"/>, <see cref="k"/>, <see cref="ConicalCurve3DParameterizationFromPolarWithBounds.alpha"/></para>
    /// <para>Basis for implementation: <see href="https://en.wikipedia.org/wiki/Conical_spiral"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Logarithmic_spiral">
    /// Cylindrical Billiard Knot.</seealso></para></summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class ConicalSpiralLogarithmic3D : ConicalCurve3DParameterizationFromPolarWithBounds
    {

        /// <summary>Constructor.</summary>
        /// <param name="alpha">Slope of the cone linew with respect to the x-y plane, specifies the property <see cref="ConicalCurve3DParameterizationFromPolarWithBounds.alpha"/>.</param>
        /// <param name="a">Coefficient of the Logarithmic spiral, defines the property <see cref="a"/>.</param>
        public ConicalSpiralLogarithmic3D(double alpha, double a, double k) : 
            base(alpha, isDefinedForNegativePhi: false)
        {
            this.a = a;
            this.k = k;
        }

        /// <summary>Parameter of the Logarithmic spiral, coefficient in its equation r(φ) = a * exp(k * φ).</summary>
        public double a { get; }

        /// <summary>Parameter of the Logarithmic spiral, exponent in its equation r(φ) = a * exp(k * φ).</summary>
        public double k { get; }


        #region ICurve2DPolarParameterization

        /// <inheritdoc/>
        public override double CurvePolar(double t) => a * Exp(k * t);

        /// <inheritdoc/>
        public override double CurveDerivativePolar(double t) => a * k * Exp(k * t);

        /// <inheritdoc/>
        public override bool HasDerivativePolar => true;

        /// <inheritdoc/>
        public override double StartParameter => - 8 * PI;

        /// <inheritdoc/>
        public override double EndParameter => 8 * PI;

        #endregion ICurve2DPolarParameterization

    }
}
