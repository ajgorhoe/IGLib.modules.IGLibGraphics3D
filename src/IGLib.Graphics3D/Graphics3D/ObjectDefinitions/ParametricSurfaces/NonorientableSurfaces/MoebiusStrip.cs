using IG.Num;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Parametric Möbius strip, with configurable number of half-twists <see cref="ff"/> (which must 
    /// be odd to generate non-orientable surface).
    /// <para>Generated as ruled surface where a line segment of length 1 moves along the circle with radius 
    /// <see cref="aa"/> and rotates in such a way that after the whole circle is visited, the final rotation 
    /// is by <see cref="ff"/> half angles (180 degrees).</para>
    /// <para>Parameters: radius <see cref="aa"/>, number of half-twists <see cref="ff"/> (the strip surface is
    /// non-orientable for odd values 1, 3, 5, ...; 1 for the standard strip with one half-twist)</para>
    /// <para>Basis for Implementation: <seealso href="https://virtualmathmuseum.org/docs/Moebius_Strip.pdf"/></para>
    /// <para>At wikipedia: <seealso cref="https://en.wikipedia.org/wiki/M%C3%B6bius_strip"/></para>
    /// <para>At MathCurve: <seealso cref="https://mathcurve.com/surfaces.gb/mobius/mobius.shtml"/></para>
    /// </summary>
    public class MoebiusStrip: IParametricSurfaceWithBounds
    {

        /// <summary>Constructor - Möbius strip with radius <paramref name="aa"/> and numberr of half-twists
        /// <paramref name="ff"/>, which define the properties <see cref="aa"/> and <see cref="ff"/>.</summary>
        public MoebiusStrip(double aa = aaDefault, double ff = ffDefault)
        {
            this.aa = aa;
            this.ff = ff;
        }

        
        /// <summary>Default value of <see cref="aa"/>.</summary>
        public const double aaDefault = 1.0;

        /// <summary>Default value of <see cref="ff"/>.</summary>
        public const double ffDefault = 1.0;


        /// <summary>Radius of the circle around which the generating line is moved. Should be greater
        /// than 1.</summary>
        public double aa { get; init; }


        /// <summary>The number of half-twists by which the generating line is rotated by 180 degrees (π/2).
        /// Default is <see cref="ffDefault"/>.
        /// <para>Only for odd integer values of <see cref="ff"/> the represented surface is non-orientable (for odd
        /// values, we glue one surface of the annulus with the other, uch that traveling on any side by two
        /// full angles reaches all segments of the surface, which has a single side). 
        /// For <see cref="ff"/> = 0 we get an <see href="https://en.wikipedia.org/wiki/Annulus_(mathematics)">
        /// annulus</see>. For even integer values (2, 4, 6, ...) the twisted surface remains orientable.</para>
        /// <para>This parameter is intentionally not an integer, such that we can use it as continuous
        /// parameter in evolution of the strip from annulus (<see cref="ff"/> = 0) to 1 half-twist, 3 half-twists,
        /// etc.</para></summary>
        public double ff { get; init; }


        /// <summary>Half axis of the ellipsoid in the z direction. Default is <see cref="cDefault"/>.</summary>
        public double c { get; init; }


        /// <inheritdoc/>
        public vec3 Surface(double u, double v)
        {
            return new vec3(
                aa * (Cos(v) + u * Cos(ff * v / 2.0) * Cos(v)),
                aa * (Sin(v) + u * Cos(ff * v / 2.0) * Sin(v)),
                aa * u * Sin(ff * v / 2.0));
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative1(double u, double v)
        {
            return new vec3(
                0,
                0,
                0);
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
