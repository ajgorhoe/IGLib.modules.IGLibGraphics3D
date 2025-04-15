using IG.Num;
using System.Text.Json.Serialization.Metadata;
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
        /// <param name="R">Defines <see cref="R"/>, the radius of the larger (in regular tori) circle, along which
        /// the smaller circle is translated. Default is <see cref="RDefault"/>.</param>
        /// <param name="r">Defines <see cref="r"/>, the adius of the smaller circle (the cross-section of the torus). 
        /// Default is <see cref="rDefault"/>.</param>
        public Torus(double R = RDefault, double r = rDefault) 
        {
            this.R = R;
            this.r = r;
        }

        public const double RDefault = 0.77;

        public const double rDefault = 0.25;

        /// <summary>Radius of the larger (in regular tori) generating circle, around which the coross-section
        /// circle is moved to generate the surface.
        /// Default value is <see cref="RDefault"/>.</summary>
        public double R { get; protected set; } = RDefault;


        /// <summary>Radius of the smaller circle (in regular tori), which defines the cross-section of torus
        /// tube and which is moved along the larger circle to define the torus surface. Default value is 
        /// <see cref="rDefault"/>.</summary>
        public double r { get; protected set; } = rDefault;

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
                -Sin(u) * (R + r * Cos(v)),
                Cos(u) * (R + r * Cos(v)),
                0);
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative2(double u, double v)
        {
            return new vec3(
                -r * Sin(v) * Cos(u),
                -r * Sin(v) * Sin(u),
                r * Cos(v));
        }

        /// <inheritdoc/>
        public bool HasDerivative => true;

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
