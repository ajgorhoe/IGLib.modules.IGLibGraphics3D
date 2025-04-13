using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Parametric torus surface.
    /// <para>Parameters: <see cref="R"/>, <see cref="r"/></para>
    /// <para>Basis for Implementation: <seealso href="https://mathcurve.com/surfaces.gb/tore/tore.shtml"/></para>
    /// <para>Wikipedia: <see cref="https://en.wikipedia.org/wiki/Torus"/></para>
    /// </summary>
    public class Torus: IParametricSurfaceWithBounds
    {

        /// <summary>Constructor - torus with larger radius <paramref name="R"/> and smaller radius <paramref name="r"/>, 
        /// centered at coordinate origin.</summary>
        /// <param name="R">Larger radius of the circle along which the smaller circle is translated.</param>
        /// <param name="r">Radius of the smaller circle (the cross-section).</param>
        /// 
        public Torus(double R, double r) 
        {
            this.R = R;
            this.r = r;
        }


        /// <summary>Larger radius of the torus, of the circle around which the coross-section circle is translated..</summary>
        public double R { get; init; }


        /// <summary>Smaller radius of the toruw, of the circle that defines the cross-section of torus tube.</summary>
        public double r { get; init; }

        /// <inheritdoc/>
        public vec3 Surface(double u, double v)
        {
            return new vec3(
                (R + r * Cos(v)) * Cos(u),
                (R + r * Cos(v)) * Sin(u),
                r * Sin(v));
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
                0);
        }

        /// <inheritdoc/>
        public bool HasDerivative => false;

        /// <inheritdoc/>
        public double StartParameter1 { get; } = 0;

        /// <inheritdoc/>
        public double EndParameter1 { get; } = 2.0 * PI;

        /// <inheritdoc/>
        public double StartParameter2 { get; init; } = 0;


        /// <inheritdoc/>
        public double EndParameter2 { get; init; } = 2.0 * PI;

    }

}
