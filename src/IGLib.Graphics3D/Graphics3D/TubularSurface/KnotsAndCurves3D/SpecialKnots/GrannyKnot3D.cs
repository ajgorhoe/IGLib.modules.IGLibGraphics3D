using IG.Num;
using System;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>The granny knot is  a composite knot obtained by taking the connected 
    /// sum of two identical trefoil knots. It is closely related to the square knot, which 
    /// can also be described as a connected sum of two trefoils. The granny knot and 
    /// the square knot are the simplest composite knots.
    /// Derivative calculation IS provided by this class.
    /// <para>Basis for implementation: <see href="https://virtualmathmuseum.org/docs/Figure_8_Knot.pdf"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Granny_knot_(mathematics)">
    /// Square knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/plat.vache/plat_vache.shtml">
    /// Square and Granny knot (MathCurve)</seealso></para>
    /// <para><seealso href="https://virtualmathmuseum.org/docs/Figure_8_Knot.pdf">
    /// Figure 8 Knot, Granny Knot, Square Knot (Virtual Math Museum)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class GrannyKnot3D : ICurve3DParameterizationWithBounds
    {

        /// <summary>Empty constructor.</summary>
        public GrannyKnot3D()
        {  }


        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                (-22 * Cos(t) - 128 * Sin(t) - 44 * Cos(3 * t) - 78 * Sin(3 * t)) / 80,
                (-10 * Cos(2 * t) - 27 * Sin(2 * t) + 38 * Cos(4 * t) + 46 * Sin(4 * t)) / 80,
                (70 * Cos(3 * t) - 40 * Sin(3 * t)) / 100 );

        /// <inheritdoc/>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                (132 * Sin(3 * t) - 234 * Cos(3 * t) + 22 * Sin(t) - 128 * Cos(t))/80,
                (-152 * Sin(4 * t) + 184 * Cos(4 * t) + 20 * Sin(2 *  t) - 54 * Cos(2 * t)) /80,
                (- 210 * Sin(3 * t) - 120 * Cos(3 * t)) /100 );
        
        /// <inheritdoc/>
        public bool HasDerivative => true;

        /// <inheritdoc/>
        public double StartParameter { get; } = 0;

        /// <inheritdoc/>
        public double EndParameter { get; } = 2 * PI;

    }
}
