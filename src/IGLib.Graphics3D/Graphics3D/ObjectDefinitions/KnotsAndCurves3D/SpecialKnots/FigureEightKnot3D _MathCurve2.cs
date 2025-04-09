using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Figure-eight knot is with alternative parameterization (the second 
    /// one from MathCurve). Derivative is not available.
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
    public class FigureEightKnot3D_MathCurve2 : ICurve3DParameterizationWithBounds
    {


        /// <summary>Empty constructor.</summary>
        public FigureEightKnot3D_MathCurve2()
        {  }


        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                3 * Cos(t) + 5 * Cos(3 * t),
                3 * Sin(t) + 5 * Sin(3 * t),
                Sin(5 * t / 2) * Sin(3 * t) + Sin(4 * t) - Sin(6 * t) );

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
