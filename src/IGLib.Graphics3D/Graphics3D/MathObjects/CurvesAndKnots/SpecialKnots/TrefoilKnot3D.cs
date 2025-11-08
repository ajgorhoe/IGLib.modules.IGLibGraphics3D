
#nullable disable

using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>The trefoil knot is the simplest example of a nontrivial knot. It is chiral.
    /// The (2,3)- or (3-2)- torus knot is also a trefoil knot.
    /// Derivative calculation is provided by this class.
    /// <para>Basis for implementation: <see href="https://en.wikipedia.org/wiki/Trefoil_knot"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Trefoil_knot">
    /// Trefoil knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/noeuds/noeuddetrefle.shtml">
    /// Trefoill knot (MathCurve)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class TrefoilKnot3D : ICurve3DParameterizationWithBounds
    {


        /// <summary>Empty constructor.</summary>
        public TrefoilKnot3D()
        {  }


        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                Sin(t) + 2 * Sin(2 * t),
                Cos(t) - 2 * Cos(2 * t),
                - Sin(3 * t) );

        /// <inheritdoc/>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                Cos(t) + 4 * Cos(2 * t),
                - Sin(t) + 4 * Sin(2 * t),
                -3 * Cos(3 * t) );
        
        /// <inheritdoc/>
        public bool HasDerivative => true;

        /// <inheritdoc/>
        public double StartParameter { get; } = 0;

        /// <inheritdoc/>
        public double EndParameter { get; } = 2 * PI;

    }
}
