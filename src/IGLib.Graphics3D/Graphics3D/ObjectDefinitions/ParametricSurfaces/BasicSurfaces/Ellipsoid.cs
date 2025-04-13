using IG.Num;
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

        /// <summary>Constructor - ellipsoid with galf-axes <paramref name="a"/>, <paramref name="b"/>
        /// and <paramref name="c"/>.</summary>
        public Ellipsoid(double a, double b, double c) 
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }


        /// <summary>Half axis of the ellipsoid in the x direction.</summary>
        public double a { get; init; }

        /// <summary>Half axis of the ellipsoid in the y direction.</summary>
        public double b { get; init; }

        /// <summary>Half axis of the ellipsoid in the z direction.</summary>
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
        public double StartParameter2 { get; init; } = -PI / 2.0;


        /// <inheritdoc/>
        public double EndParameter2 { get; init; } = PI / 2.0;

    }

}
