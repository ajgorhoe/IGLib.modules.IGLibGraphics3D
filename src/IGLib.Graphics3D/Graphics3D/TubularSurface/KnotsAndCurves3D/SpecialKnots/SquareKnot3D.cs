using IG.Num;
using System;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>The square (or reef) knot is a composite knot obtained by taking the
    /// connected sum of a trefoil knot with its reflection. It is closely related to the
    /// granny knot, which is also a connected sum of two trefoils. Because the trefoil
    /// knot is the simplest nontrivial knot, the square knot and the granny knot are
    /// the simplest of all composite knots.
    /// The square knot is the mathematical version of the common reef knot.
    /// Derivative calculation is NOT provided by this class.
    /// <para>Basis for implementation: <see href="https://virtualmathmuseum.org/docs/Figure_8_Knot.pdf"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Square_knot_(mathematics)">
    /// Square knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/plat.vache/plat_vache.shtml">
    /// Square and Granny knot (MathCurve)</seealso></para>
    /// <para><seealso href="https://virtualmathmuseum.org/docs/Figure_8_Knot.pdf">
    /// Figure 8 Knot, Granny Knot, Square Knot (Virtual Math Museum)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class SquareKnot3D : ICurve3DParameterizationWithBounds
    {


        /// <summary>Empty constructor.</summary>
        public SquareKnot3D()
        {  }


        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                (-22 * Cos(t) - 128 * Sin(t) - 44 * Cos(3 * t) - 78 * Sin(3 * t)) / 100,
                (11 * Cos(t) - 43 * Sin(3 * t) + 34 * Cos(5 * t) - 39 * Sin(5 * t))/100,
                (70 * Cos(3 * t) - 40 * Sin(3 * t) + 18 * Cos(5 * t) - 9 * Sin(5 * t)) / 100 );

        /// <inheritdoc/>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                (-78 * Sin(3 * t) - 44 * Cos(3 * t) - 128 * Sin(t) - 22 * Cos(t)) / 100,
                (-170 * Sin(5 * t) - 195 * Cos(5 * t) - 129 * Cos(3 * t) - 11 * Sin(t)) / 100,
                (-90 * Sin(5 * t) - 45 * Cos(5 * t) - 210 * Sin(3 * t) - 120 * Cos(3 * t)) / 100 );
        
        /// <inheritdoc/>
        public bool HasDerivative => true;

        /// <inheritdoc/>
        public double StartParameter { get; } = 0;

        /// <inheritdoc/>
        public double EndParameter { get; } = 2 * PI;

    }
}
