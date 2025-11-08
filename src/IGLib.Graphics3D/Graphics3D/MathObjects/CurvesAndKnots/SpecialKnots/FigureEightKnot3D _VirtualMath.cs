
#nullable disable

using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Figure-eight knot with alternative parameterization, obtained from Virtual Math Museum.
    /// Derivative is not calculated.
    /// <para>Basis for implementation: <see href="https://virtualmathmuseum.org/docs/Figure_8_Knot.pdf"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Figure-eight_knot_(mathematics)">
    /// Figure-eight knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/noeuds/noeudenhuit.shtml">
    /// Figure-Eight knot (MathCurve)</seealso></para>
    /// <para><seealso href="https://virtualmathmuseum.org/docs/Figure_8_Knot.pdf">
    /// Figure 8 Knot, Granny Knot, Square Knot (Virtual Math Museum)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class FigureEightKnot3D_VirtualMath : ICurve3DParameterizationWithBounds
    {


        /// <summary>Empty constructor.</summary>
        public FigureEightKnot3D_VirtualMath()
        {  }


        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                (32 * Cos(t) - 51 * Sin(t) - 104 * Cos(2 * t) - 34 * Sin(2 * t) + 104 * Cos(3 * t) - 91 * Sin(3 * t)) / 100,
                (94 * Cos(t) + 41 * Sin(t) + 113 * Cos(2 * t) - 68 * Cos(3 * t) - 124 * Sin(3 * t)) / 140,
                (16 * Sin(t) - 138 * Cos(2 * t) - 39 * Sin(2 * t) - 99 * Cos(3 * t) - 21 * Sin(3 * t)) / 70 );

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
