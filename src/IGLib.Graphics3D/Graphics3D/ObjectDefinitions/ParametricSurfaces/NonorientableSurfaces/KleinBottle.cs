using IG.Num;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Parametric Klein bottle surface with mirror symmetry. 
    /// 
    /// <see cref="aa"/> and at the same time rotating it around the circle 
    /// by linearly growing rotationn angle, which reaches the specified number of half-twists <see cref="ff"/> 
    /// (which must be odd to generate non-orientable surface of a Klein bottle) after the angle on the larger circle
    /// reaches the full angle (360 degrees or 2*π).
    /// <para>No parameters.</para>
    /// <para>Basis for Implementation: Wolfram Research, <seealso href="https://mathworld.wolfram.com/KleinBottle.html"/></para>
    /// <para>At wikipedia: <seealso cref="https://en.wikipedia.org/wiki/Klein_bottle#The_figure_8_immersion"/></para>
    /// <para>At MathCurve: <seealso cref="https://mathcurve.com/surfaces.gb/klein/klein.shtml"/></para>
    /// <para>At Virtual Math Museum: <seealso cref="https://virtualmathmuseum.org/docs/Moebius_Strip.pdf"/></para>
    /// </summary>
    /// <seealso cref="MoebiusStrip"/>
    public class KleinBottle: IParametricSurfaceWithBounds
    {

        /// <summary>Constructor - Klein bottle generated from figure eight curve, with radius 
        /// <paramref name="aa"/> and number of half-twists <paramref name="ff"/>, which define 
        /// properties <see cref="aa"/> and <see cref="ff"/>.</summary>
        public KleinBottle(double aa = aaDefault, double ff = ffDefault)
        {
            this.aa = aa;
            this.ff = ff;
        }

        
        /// <summary>Default value of <see cref="aa"/>.</summary>
        public const double aaDefault = 1.0;

        /// <summary>Default value of <see cref="ff"/>.</summary>
        public const double ffDefault = 1.0;


        /// <summary>Radius of the circle around which the generating figure-eight curve is moved.</summary>
        public double aa { get; init; }


        /// <summary>The number of half-twists by which the generating figure-eight curve is rotated by 180 degrees 
        /// (π/2). before moving along the full circle (angle 2*π). Default is <see cref="ffDefault"/>.
        /// <para>Only for odd integer values of <see cref="ff"/> the represented surface is non-orientable Klein
        /// bottle (for odd values, we glue one surface of the curved stripes of annulus with the other, such that 
        /// traveling on any side by two full angles reaches all segments of the surface, which has a single side). 
        /// For <see cref="ff"/> = 0 we get an figure-eight torus. For even integer values (2, 4, 6, ...) the twisted
        /// surface remains orientable.</para>
        /// <para>This parameter is intentionally not an integer, such that we can use it as continuous
        /// parameter in evolution of the surface from figure-eight torus (<see cref="ff"/> = 0) to 1 half-twist, 
        /// 3 half-twists, etc., Kein bottles.</para></summary>
        public double ff { get; init; }



        /// <inheritdoc/>
        public vec3 Surface(double u, double v)
        {
            double sqrt2 = Sqrt(2.0);
            return new vec3(
                Cos(u) * (Cos(0.5 * u) * (sqrt2 + Cos(v)) + Sin(0.5 * u) * Sin(v) * Cos(v)),
                Sin(u) * (Cos(0.5 * u) * (sqrt2 + Cos(v)) + Sin(0.5 * u) * Sin(v) * Cos(v)),
                - Sin(0.5 * u) * (sqrt2 + Cos(v)) + Cos(0.5 * u) * Sin(v) * Cos(v) );
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative1(double u, double v)
        {
            return new vec3(
                0,
                0,
                0 );
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative2(double u, double v)
        {
            return new vec3(
                0,
                0,
                0 );
        }

        /// <inheritdoc/>
        public bool HasDerivative => false;

        /// <inheritdoc/>
        public double StartParameter1 { get; } = -PI;

        /// <inheritdoc/>
        public double EndParameter1 { get; } = PI;

        /// <inheritdoc/>
        public double StartParameter2 { get; init; } = - 0.5 * PI;


        /// <inheritdoc/>
        public double EndParameter2 { get; init; } = 0.5 * PI;

    }

}
