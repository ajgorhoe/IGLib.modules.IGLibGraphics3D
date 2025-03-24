using IG.Num;
using Microsoft.VisualBasic;
using static System.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IGLib.Gr3D
{

    /// <summary>A Lisajous knot is a knot whose projection on any of the three coordinate planes
    /// is a Lissajous curve.
    /// <para> knot cannot be self-intersecting, the three integers n1 , n2 , n3 must be pairwise
    /// relatively prime (coprime, GCD=1), and none of the quantities n1*fi2-n2*fi1, 
    /// n2*fi3-n3*fi2, n3*fi1-n1*fi3 may be an intgerr multiple of π.</para>
    /// <para>Basis for implementation: <see href="https://en.wikipedia.org/wiki/Lissajous_knot"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Lissajous_knot">
    /// Lissajous knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/lissajous3d/lissajous3d.shtml">
    /// 3D Lissajous curvee (MathCurve)</seealso></para>
    /// </summary>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Lissajous_curve">
    /// 2D Lissajous curve (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes2d.gb/lissajous/lissajous.shtml">
    /// 2D Lissajous curvee (MathCurve)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurveParameterization3D"/>
    public class LissajousKnot3D : ICurve3DParameterizationWithBounds
    {

        /// <summary>Constructor, initializes additional parameters that define the specific
        /// knot out of a family and phase shifts.</summary>
        public LissajousKnot3D(int n1 = 3, int n2 = 4, int n3 = 7,
            double fi1 = 0.1, double fi2 = 0.7)
        {
            this.n1 = n1;
            this.n2 = n2;
            this.n3 = n3;
            this.fi1 = fi1;
            this.fi2 = fi2;
            this.fi3 = fi3;
        }

        protected int n1 { get; }

        protected int n2 { get; }

        protected int n3 { get; }

        protected double fi1 { get; }

        protected double fi2 { get; }

        protected double fi3 { get; }

        /// <inheritdoc/>
        /// <remarks>Surface of the torus is given (in xylindrical coordinates) by
        /// (r - 2)^2 + z^2 = 1.</remarks>
        public virtual vec3 Curve(double t) =>
            new vec3(
                Cos(n1 * t + fi1),
                Cos(n2 * t + fi2),
                Cos(n3 * t + fi3));

        /// <inheritdoc/>
        public virtual vec3 CurveDerivative(double t) =>
            new vec3(
                - n1 * Sin(n1 * t + fi1),
                - n2 * Sin(n2 * t + fi2),
                - n3 * Sin(n3 * t + fi3) );
        
        /// <inheritdoc/>
        public virtual bool HasDerivative => true;

        /// <inheritdoc/>
        public virtual double StartParameter { get; } = 0;

        /// <inheritdoc/>
        public virtual double EndParameter { get; } = 2 * PI;

    }
}
