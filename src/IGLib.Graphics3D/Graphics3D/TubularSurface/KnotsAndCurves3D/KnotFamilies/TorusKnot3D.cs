using IG.Num;
using Microsoft.VisualBasic;
using static System.Math;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IGLib.Gr3D
{

    /// <summary>A torus knot is a special kind of knot that lies on the surface of a torus.
    /// <para>Each torus knot is specified by a pair of coprime integers p and q. A torus link arises 
    /// if p and q are not coprime (the number of components is gcd(p, q)). A torus knot is trivial 
    /// (equivalent to the unknot) if and only if either p or q is equal to 1 or −1. The simplest 
    /// nontrivial example is the (2,3)-torus knot, also known as the trefoil knot.</para>
    /// <para>The (p,q)-torus knot winds q times around a circle in the interior of the torus, and 
    /// p times around its axis of rotational symmetry.</para>
    /// <para>The direction in which the strands of the knot wrap around the torus:  right-handed
    /// screw for p * q > 0.</para>
    /// <para>Knot properties: A torus knot is trivial iff either p or q is equal to 1 or −1.
    /// Each nontrivial torus knot is prime and chiral. The(p, q) torus knot is equivalent to the
    /// (q, p) torus knot. The (p,−q) torus knot is the (mirror image) of the(p, q) torus knot.
    /// The (−p,−q) torus knot is equivalent to the(p, q) torus knot except for the reversed orientation.</para>
    /// <para>Basis for implementation: <see href="https://en.wikipedia.org/wiki/Torus_knot"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Torus_knot">
    /// Torus knot (Wkikipedia)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/solenoidtoric/solenoidtoric.shtml">
    /// Toric solenoid, knot and link (MathCurve)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class TorusKnot3D : ICurve3DParameterizationWithBounds
    {

        /// <summary>Empty constructor.</summary>
        /// <param name="p">Number of times the knot winds around torus' axis of rotational symmetry.</param>
        /// <param name="q">Number of times the knot winds around torus' circular cross-section.</param>
        public TorusKnot3D(int p, int q)
        {
            this.p = p;
            this.q = q;
        }

        /// <summary>Number of times the knot winds around torus' axis of rotational symmetry.</summary>
        protected double p { get; }

        /// <summary>Number of times the knot winds around torus' circular cross-section.</summary>
        protected double q { get; }

        /// <inheritdoc/>
        /// <remarks>Surface of the torus is given (in xylindrical coordinates) by
        /// (r - 2)^2 + z^2 = 1.</remarks>
        public vec3 Curve(double t) =>
            new vec3(
                (Cos(q * t) + 2) * Cos(p * t),
                (Cos(q * t) + 2) * Sin(p * t),
                Sin(q * t) );

        /// <inheritdoc/>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                -q * Cos(p * t) * Sin(q * t) - p * Sin(p * t) * (Cos(q * t) + 2),
                p * Cos(p * t) * (Cos(q * t) + 2) - q * Sin(p * t) * Sin(q * t),
                q * Cos(q * t) );
        
        /// <inheritdoc/>
        public bool HasDerivative => false;

        /// <inheritdoc/>
        public double StartParameter { get; } = 0;

        /// <inheritdoc/>
        public double EndParameter { get; } = 2 * PI;

    }
}
