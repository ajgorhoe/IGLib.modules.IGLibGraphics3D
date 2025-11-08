
#nullable disable

using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>Lissajous curves in 3D are trajectories of a point in space whose rectangular components
    /// have a sinusoidal motion. Projections on coordinate planes are classical 2D Lissajous curves, 
    /// which can be obtained with oscillators.
    /// <para>For n = 1 or n = m, we get a cylindrical sine wave.</para>
    /// <para>We get a closed curve if and only if n and m are rational.</para>
    /// <para>When the curve does not have double points, nor a cusp, it forms a knot in space, 
    /// called Lissajous knot, equivalent to a cubic billiard knot.</para>
    /// <para>Basis for implementation: <see href="https://mathcurve.com/courbes3d.gb/lissajous3d/lissajous3d.shtml"/></para>
    /// <para>See also:</para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/lissajous3d/lissajous3d.shtml">
    /// 3d Lissajous curve</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/lissajous3d/noeudlissajous.shtml">
    /// Lissajous Knot (Billard Knot)</seealso></para>
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/lissajous3d/noeudlissajous.shtml">
    /// Lissajous Knot (Wikipedia)</seealso></para>
    /// <para><seealso href="https://en.wikipedia.org/wiki/Lissajous_curve">
    /// Lissajous curve (2D; Wikipedia)</seealso></para>
    /// </summary>
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    class LissajousCurve3D : ICurve3DParameterizationWithBounds
    {


        public LissajousCurve3D(int m, int n, double fi = 0, double psi = 0,
            double a = 1.0, double b = 1.0, double c = 0.5)
        {
            this.m = m;
            this.n = n;
            this.fi = fi;
            this.psi = psi;
            this.a = a;
            this.b = b;
            this.c = c;
        }

        /// <summary>The number of oscillations in y direction per full revolution.</summary>
        public int n { get; }

        /// <summary>The number of oscillations in z direction per full revolution.</summary>
        public int m { get; }


        /// <summary>Phase shift 1 (y direction).</summary>
        public double fi { get; }

        /// <summary></summary>
        public double psi { get; }


        /// <summary>Width of the curve bounding box.</summary>
        public double a { get; }

        /// <summary>Depth of the curve bounding box.</summary>
        public double b { get; }

        /// <summary>Height of the curve bounding box.</summary>
        public double c { get; }

        /// <summary>3D vector function of scalar argument, which gives a 3D representation of the knot.
        /// Properties <see cref="StartParameter"/> and <see cref="EndParameter"/> determine the starting
        /// and ending parameter that the curve closed.</summary>
        public vec3 Curve(double t) =>
            new vec3(
                a * Sin(t),
                b * Sin(n * t + fi), 
                c * Sin(m * t + psi) );

        /// <summary>3D vector function of scalar argument, which represents the derivative of 
        /// <see cref="Curve"/> with respect to parameter. In each point on curve (for each parameter
        /// value), this defines a tangent on the curve (or speed vector), with its size representing 
        /// the velocity.</summary>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                a * Cos(t),
                b * n * Cos(n*t + fi),
                c * m * Cos(m * t + psi));

        public bool HasDerivative => true;

        /// <summary>Typical starting value of the parameter of the parametric curve that represents
        /// the knot. <see cref="StartParameter"/> and <see cref="EndParameter"/> should, for true
        /// knots, create a closed curve.</summary>
        public double StartParameter { get; } = 0;

        /// <summary>Typical end value of the parameter of the parametric curve that represents
        /// the knot. <see cref="StartParameter"/> and <see cref="EndParameter"/> should, for true
        /// knots, create a closed curve.</summary>
        public double EndParameter { get; } = 2 * PI;

    }
}
