using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Figure-eight knot, alternative parameterization ( the second one
    /// from MathCurve). Derivative is not available.
    /// <para>Basis for implementation: <see href="https://mathcurve.com/courbes3d.gb/noeuds/noeudenhuit.shtml"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Figure-eight_knot_(mathematics)">
    /// Figure-eight knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/noeuds/noeudenhuit.shtml">
    /// Figure-Eight knot (MathCurve)</seealso></para>
    /// <para><seealso href="https://virtualmathmuseum.org/docs/Figure_8_Knot.pdf">
    /// Figure 8 Knot, Granny Knot, Square Knot (Virtual Math Museum)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    class FigureEightKnot3D_MathCurve1 : ICurve3DParameterizationWithBounds
    {

        /// <summary>Empty constructor.</summary>
        public FigureEightKnot3D_MathCurve1()
        {  }


        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                Sin(t) + t/10,
                Sin(t) * Cos(t) / 2,
                Sin(2 * t) * Sin(t/2) / 4 );

        /// <inheritdoc/>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                0,
                0,
                0);
        
        /// <inheritdoc/>
        public bool HasDerivative => false;

        /// <inheritdoc/>
        public double StartParameter { get; } = 0;

        /// <inheritdoc/>
        public double EndParameter { get; } = 2 * PI;

    }
}
