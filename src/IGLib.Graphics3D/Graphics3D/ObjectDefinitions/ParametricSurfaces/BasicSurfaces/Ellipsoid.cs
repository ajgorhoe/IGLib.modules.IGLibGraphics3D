using IG.Num;
using System.Reflection;
using System.Runtime.InteropServices;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Parametric ellipsoid surface, usual parameterization via longitude and latitude.
    /// <para>Parameters: <see cref="a"/>, <see cref="b"/>, <see cref="c"/></para>, 
    /// <para>Basis for Implementation: <seealso href="https://mathcurve.com/surfaces.gb/ellipsoid/ellipsoid.shtml"/></para>
    /// <para>Wikipedia: <seealso cref="https://en.wikipedia.org/wiki/Ellipsoid"/></para>
    /// </summary>
    public class Ellipsoid: IParametricSurfaceWithBounds
    {

        /// <summary>Constructor - ellipsoid with half-axes <paramref name="a"/>, <paramref name="b"/>
        /// and <paramref name="c"/>, which define ptoperties <see cref="a"/>, <see cref="b"/>
        /// and <see cref="c"/>.</summary>
        public Ellipsoid(double a = aDefault, double b = bDefault, double c = cDefault) 
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        
        /// <summary>Default value of <see cref="a"/>.</summary>
        public const double aDefault = 0.5;

        /// <summary>Default valur of <see cref="b"/>.</summary>
        public const double bDefault = 0.375;

        /// <summary>Default valur of <see cref="c"/>.</summary>
        public const double cDefault = 0.25;


        /// <summary>Half axis of the ellipsoid in the x direction. Default is <see cref="aDefault"/>.</summary>
        public double a { get; init; }

        /// <summary>Half axis of the ellipsoid in the y direction. Default is <see cref="bDefault"/>.</summary>
        public double b { get; init; }

        /// <summary>Half axis of the ellipsoid in the z direction. Default is <see cref="cDefault"/>.</summary>
        public double c { get; init; }


        /// <inheritdoc/>
        public vec3 Surface(double u, double v)
        {
            return new vec3(
                a * Cos(u) * Cos(v),
                b * Sin(u) * Cos(v),
                c * Sin(v));
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative1(double u, double v)
        {
            return new vec3(
                - a * Sin(u) * Cos(v),
                b * Cos(u) * Cos(v),
                0);
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative2(double u, double v)
        {
            return new vec3(
                -a * Cos(u) * Sin(v),
                -b * Sin(u) * Sin(v),
                c * Cos(v));
        }

        /// <inheritdoc/>
        public bool HasDerivative => true;

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
