using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Alternative parameterization of the trefoil knot - third parameterization
    /// from MathCurve (see the link below).
    /// Derivativee calculatiion is provided by this class.
    /// <para>Basis for implementation: <see href="https://mathcurve.com/courbes3d.gb/noeuds/noeuddetrefle.shtml"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Trefoil_knot">
    /// Trefoil knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/noeuds/noeuddetrefle.shtml">
    /// Trefoill knot (MathCurve)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    class TrefoilKnot3D_MathCurve3 : ICurve3DParameterizationWithBounds
    {


        /// <summary>Constructor. Initializes additional parrameters.</summary>
        /// <param name="epsilon">Determine the amplitude of oscillation of the 
        /// parameterization in z-axis.</param>
        public TrefoilKnot3D_MathCurve3(double epsilon)
        {  }

        /// <summary>Determine the amplitude of oscillation of the 
        /// parameterization in z-axis.</summary>
        double epsilon { get; } = 0.2;

        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                Sin(2 * t),
                Sin(t) * Cos(2 * t),
                epsilon * Cos(3 * t) );

        /// <inheritdoc/>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                2 * Cos(2 * t),
                Cos(t) * Cos(2 * t) - 2 * Sin(t) * Sin(2 * t),
                - 3 * epsilon * Sin(3 * t) );
        
        /// <inheritdoc/>
        public bool HasDerivative => true;

        /// <inheritdoc/>
        public double StartParameter { get; } = 0;

        /// <inheritdoc/>
        public double EndParameter { get; } = 2 * PI;

    }
}
