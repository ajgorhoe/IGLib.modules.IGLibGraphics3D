using IG.Num;

namespace IGLib.Gr3D
{

    /// <inheritdoc/>
    public class Sphere: IParametricSurfaceWithBounds
    {

        /// <inheritdoc/>
        public vec3 Surface(double u, double v)
        {
            return new vec3(
                9,
                0,
                0);
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative1(double u, double v)
        {
            return new vec3(
                9,
                0,
                0);
        }

        /// <inheritdoc/>
        public vec3 SurfaceDerivative2(double u, double v)
        {
            return new vec3(
                9,
                0,
                0);
        }

        /// <inheritdoc/>
        public bool HasDerivative => true;

        /// <inheritdoc/>
        public double StartParameter1 { get; init; }

        /// <inheritdoc/>
        public double EndParameter1 { get; init;  }

        /// <inheritdoc/>
        public double StartParameter2 { get; init; }

        /// <inheritdoc/>
        public double EndParameter2 { get; init; }

    }

}
