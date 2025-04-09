using static System.Math;

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>A 3D parametric curve with bounds - the conical Fermat's spiral.
    /// <para>This conical spiral is based on the Fermat's spiral r(φ) = a * Sqrt(φ).</para>
    /// <para>Remark: implementation is adapted a little bit, such that the nnegative branch of square
    /// root is taken for negative parameteers (polar angle φ).</para>
    /// <para>Parameters: <see cref="a"/>, <see cref="ConicalCurve3DParameterizationFromPolarWithBounds.alpha"/></para>
    /// <para>Basis for implementation: <see href="https://en.wikipedia.org/wiki/Conical_spiral"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Fermat%27s_spiral">
    /// Cylindrical Billiard Knot.</seealso></para></summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class ConicalSpiralFermats3D : ConicalCurve3DParameterizationFromPolarWithBounds
    {

        /// <summary>Constructor.</summary>
        /// <param name="alpha">Slope of the cone linew with respect to the x-y plane, specifies the property <see cref="ConicalCurve3DParameterizationFromPolarWithBounds.alpha"/>.</param>
        /// <param name="a">Coefficient of the Fermat's spiral, defines the property <see cref="a"/>.</param>
        public ConicalSpiralFermats3D(double alpha, double a) :
            base(alpha, isDefinedForNegativePhi: true)
        {
            this.a = a;
        }

        /// <summary>Parameter of the Fermat's spiral, coefficient in its equation r(φ) = a * Sqrt(φ).</summary>
        public double a { get; }


        #region ICurve2DPolarParameterization

        /// <inheritdoc/>
        public override double CurvePolar(double t)
        {
            if (t < 0)
            {
                return -CurvePolar(-t);
            }
            return a * Sqrt(t);
        }

        /// <inheritdoc/>
        public override double CurveDerivativePolar(double t)
        {
            if (t < 0)
            {
                return - CurveDerivativePolar(-t);
            }
            double epsilon = 1e-12;
            if (t < 1e-10)
            {
                t = 1e-10;
            }
            if (t < epsilon)
            {
                return 1.0 / (2.0 * Sqrt(t) + epsilon);
            }
            return 1.0 / (2.0 * Sqrt(t));
        }

        /// <inheritdoc/>
        public override bool HasDerivativePolar => true;

        /// <inheritdoc/>
        public override double StartParameter => -10 * PI;

        /// <inheritdoc/>
        public override double EndParameter => 10 * PI;

        #endregion ICurve2DPolarParameterization

    }
}
