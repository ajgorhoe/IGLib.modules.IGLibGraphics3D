using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Figure-eight knot is the only knot with 4 crossings. It is achiral, it is equal to is image by
    /// reflection. It is an alternating prime knot.
    /// <para>Basis for implementation: <see href="https://en.wikipedia.org/wiki/Figure-eight_knot_(mathematics)"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Figure-eight_knot_(mathematics)">
    /// Figure-eight knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/noeuds/noeudenhuit.shtml">
    /// Figure-Eight knot (MathCurve)</seealso></para>
    /// <para><seealso href="https://virtualmathmuseum.org/docs/Figure_8_Knot.pdf">
    /// Figure 8 Knot, Granny Knot, Square Knot (Virtual Math Museum)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class FigureEightKnot3D : ICurve3DParameterizationWithBounds
    {


        /// <summary>Constructor.</summary>
        /// <param name="epsilon">Specifies with what amplitude will the curve oscillate in z-direction.</param>
        public FigureEightKnot3D(double epsilon = 0.3)
        { this.epsilon = epsilon; }

        double epsilon { get; }

        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                Sin(2 * t),
                Sin(t) * Cos(2 * t),
                epsilon * Cos(3 * t) );

        /// <inheritdoc/>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                0,
                0,
                0 );
        
        /// <inheritdoc/>
        public bool HasDerivative => false;

        /// <inheritdoc/>
        public double StartParameter { get; } = 0;

        /// <inheritdoc/>
        public double EndParameter { get; } = 2 * PI;

    }
}
