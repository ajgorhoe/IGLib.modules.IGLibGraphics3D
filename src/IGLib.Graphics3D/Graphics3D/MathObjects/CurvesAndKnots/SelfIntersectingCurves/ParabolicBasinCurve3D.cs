
#nullable disable

using IG.Num;
using static System.Math;

namespace IGLib.Gr3D
{

    /// <summary>A curve wrapped around the paraboloid, which becomes a <see href="https://mathcurve.com/courbes2d.gb/rosace/rosace.shtml">
    /// rose</see> when projected to the XY plane.
    /// <para><seealso href="https://mathcurve.com/courbes3d.gb/vasque3d/vasque3d.shtml"/></para></summary>
    /// 
    /// <seealso cref="ICurve3DParameterizationWithBounds"/>
    public class ParabolicBasinCurve3D : ICurve3DParameterizationWithBounds
    {

        /// <summary>Constructor.</summary>
        /// <param name="n">The number of leaves of the rose curve. Must be greater than zero.
        /// Set it to more than 15 for interesting results.</param>
        /// <param name="a">Width of the paraboloid around which the curve wraps.</param>
        /// <param name="b">Height of the paraboloid atound which the curve is wrapped.</param>
        public ParabolicBasinCurve3D (int n, double a = 1.0, double b = 1.0)
        {
            this.n = n;
            this.a = a;
            this.b = b;
        }

        /// <summary>The number of leaves of the rose curve, must be greater than zero.</summary>
        public int n { get; }

        /// <summary>Width of the paraboloid around which the curve wraps.</summary>
        public double a { get; }

        /// <summary>Height of the paraboloid around which the curve wraps.</summary>
        public double b { get; }

        /// <summary>3D vector function of scalar argument, which gives a 3D representation of the knot.
        /// Properties <see cref="StartParameter"/> and <see cref="EndParameter"/> determine the starting
        /// and ending parameter that the curve closed.</summary>
        public vec3 Curve(double t) =>
            new vec3(
                a * Cos(t) * Cos(n * t),
                a * Sin(t) * Cos(n * t), 
                b * Cos(n * t) * Cos(n * t) );

        /// <summary>3D vector function of scalar argument, which represents the derivative of 
        /// <see cref="Curve"/> with respect to parameter. In each point on curve (for each parameter
        /// value), this defines a tangent on the curve (or speed vector), with its size representing 
        /// the velocity.</summary>
        public vec3 CurveDerivative(double t) =>
            new vec3(
                -a * n * Cos(t) * Sin(n * t) - a * Sin(t) * Cos(n * t),
                a * Cos(t) * Cos(n *  t) - a * n * Sin(t) * Sin(n * t),
                -2 * b * n * Cos(n * t) * Sin(n * t));

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
