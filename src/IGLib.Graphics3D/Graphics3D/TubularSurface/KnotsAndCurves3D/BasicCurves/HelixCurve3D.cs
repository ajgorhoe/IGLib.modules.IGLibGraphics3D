using static System.Math;

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>A 3D parametric curve with bounds - a cylindrical helix.
    /// <para>The radius of the helix is a, and its shift (the distance between two consecutive coils) is 
    /// 2 * π * b (b is sometimes called reduced shift of the helix). The angle of the helix is constant
    /// and equal to atan(b/a).</para>
    /// <para>Additional parameters mentioned above: <see cref="a"/>, <see cref="b"/>, <see cref="righthanded"/></para>
    /// <para>Basis for implementation: <see href="https://en.wikipedia.org/wiki/Helix"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/helicecirculaire/helicecirculaire.shtml">
    /// Cylindrical Helix.</seealso></para></summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class HelixCurve3D : ICurve3DParameterizationWithBounds
    {

        /// <summary>Constructor.</summary>
        public HelixCurve3D(double a = 1.0, double b = 0, bool righthanded = true)
        {
            this.a = a;
            this.b = b;
            this.righthanded = righthanded;
        }

        /// <summary>Whether the helix orientation is right-handed (true) or left-handed (false).</summary>
        public bool righthanded { get; }

        /// <summary>1 if <see cref="righthanded"/>, -1 if not (for the left-handed helix curve).</summary>
        public double epsilon => righthanded ? 1 : -1;

        /// <summary>Radius of the helix.</summary>
        public double a { get; }

        /// <summary>Reduced shift of the helix. 2 * Pi * b is the height that is gained by each full turn
        /// of the helix.</summary>
        public double b { get; }

        /// <inheritdoc/>
        public vec3 Curve(double t) =>
            new vec3(
                a * Cos(t),
                epsilon * a * Sin(t), 
                b * t  );

        /// <inheritdoc/>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                - a * Sin(t),
                epsilon * a * Cos(t),
                b );

        public bool HasDerivative => true;

        /// <inheritdoc/>
        public double StartParameter { get; } = 0;

        /// <inheritdoc/>
        /// <remarks>By default, representation has two coils.</remarks>
        public double EndParameter { get; } = 2 * 2 * PI;

    }
}
