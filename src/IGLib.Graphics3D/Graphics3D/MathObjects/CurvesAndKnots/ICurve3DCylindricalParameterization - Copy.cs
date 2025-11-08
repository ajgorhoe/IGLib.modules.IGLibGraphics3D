
#nullable disable

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>Cylindrical parameterization of a curve in 3D. Beside the <see cref="CurveCylindrical"/> and 
    /// (optional) <see cref="CurveDerivativeCylindrical"/>, it also contains bounds on parameter, 
    /// <see cref="StartParameterCylindrical"/> and <see cref="EndParameterCylindrical"/> as part of the class.
    /// </summary>
    interface ICurve3DCylindricalParameterization
    {

        /// <summary>3D vector function of scalar argument, which gives a 3D representation of the curve.
        /// Properties <see cref="StartParameterCylindrical"/> and <see cref="EndParameterCylindrical"/> determine the starting
        /// and ending parameter that the curve closed.</summary>
        (double rho, double phi, double z) CurveCylindrical(double t);

        /// <summary>3D vector function of scalar argument, which represents the derivative of 
        /// <see cref="CurveCylindrical"/> with respect to parameter. In each point on curve (for each parameter
        /// value), this defines a tangent on the curve (or speed vector), with its size representing 
        /// the velocity.
        /// <para>You can use <see href="https://www.derivative-calculator.net/">Derivative Calculator</see>
        /// to calculate or verify the derivatives.</para></summary>
        (double rhoDerivative, double phiDerivative, double zDerivative) CurveDerivativeCylindrical(double t);

        /// <summary>Whether derivative is defined or not. If false then <see cref="CurveDerivativeCylindrical(double)"/>
        /// cannot be used to calculate tangents on the curve.</summary>
        bool HasDerivativeCylindrical { get; }

        /// <summary>Typical starting value of the parameter of the parametric curve that represents
        /// the curve. Interval from <see cref="StartParameter"/> to <see cref="EndParameter"/> should, 
        /// for mathematical knots, produce a closed curve.</summary>
        double StartParameter { get; }

        /// <summary>Typical end value of the parameter of the parametric curve that represents
        /// the curve.  Interval from <see cref="StartParameter"/> to <see cref="EndParameter"/> should, 
        /// for mathematical knots, produce a closed curve.</summary>
        double EndParameter { get; }

    }

}
