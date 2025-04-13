using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Parametric sphere surface, usual parameterization via longitude and latitude.
    /// <para>Parameter: <see cref="R"/></para>
    /// <para>Basis for Implementation: <seealso href="https://mathcurve.com/surfaces.gb/sphere/sphere.shtml"/></para>
    /// <para>Wikipedia: <seealso cref="https://en.wikipedia.org/wiki/Sphere"/></para>
    /// </summary>
    public class Sphere: IParametricSurfaceWithBounds
    {

        /// <summary>Constructor - sphere with radius R, centered at coordinate origin.</summary>
        /// <param name="R">Radius of the sphere.</param>
        public Sphere(double R) 
        {
            this.R = R;
        }


        /// <summary>Radius of the sphere.</summary>
        public double R { get; init; }

        /// <inheritdoc/>
        public vec3 Surface(double u, double v)
        {
            return new vec3(
                R * Cos(u) * Cos(v),
                R * Sin(u) * Cos(v),
                R * Sin(v));
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative1(double u, double v)
        {
            return new vec3(
                - R * Sin(u) * Cos(v),
                R * Cos(u) * Cos(v),
                0);
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative2(double u, double v)
        {
            return new vec3(
                -R * Cos(u) * Sin(v),
                -R * Sin(u) * Sin(v),
                R * Cos(v));
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
