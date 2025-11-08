
#nullable disable

using IG.Num;

namespace IGLib.Gr2D
{

    /// <summary>Cartesion parameterization of a curve in 2D. Beside the <see cref="Curve"/> and 
    /// (optional) <see cref="CurveDerivative"/>, it also contains bounds on parameter, <see cref="StartParameter"/>
    /// and <see cref="EndParameter"/> as part of the class.
    /// <para><see cref="StartParameter"/> and <see cref="EndParameter"/> usually denote bounds on curve
    /// parameter that are used by default when the curve is tabulated, plotted, for generating meshes or for
    /// numerical calculations (i.e. lengths or urfaces or volumes where the curve is involved). Sometimes,
    /// these properties can also represent the range of parameter values for which the curve is defined,
    /// though rarely. More often, they represent some characteristic or natural range for the curve in 
    /// question, and sometimes theey are not used at all (the convention is to set both to 0 in such cases).
    /// For mathematical knots (slosed ones), these properties are usually set to such values that the
    /// represented curves exactly closes eginning with its end when parameter runs from 
    /// <see cref="StartParameter"/> and <see cref="EndParameter"/>.</para></summary>
    interface ICurveParameterization2D
    {

        /// <summary>2D vector function of scalar argument, which gives a 3D representation of the knot.
        /// Properties <see cref="StartParameter"/> and <see cref="EndParameter"/> determine the starting
        /// and ending parameter that the curve closed.</summary>
        vec3 Curve(double t);

        /// <summary>3D vector function of scalar argument, which represents the derivative of 
        /// <see cref="Curve"/> with respect to parameter. In each point on curve (for each parameter
        /// value), this defines a tangent on the curve (or speed vector), with its size representing 
        /// the velocity.
        /// <para>You can use <see href="https://www.derivative-calculator.net/">Derivative Calculator</see>
        /// to calculate or verify the derivatives.</para></summary>
        vec3 CurveDerivative(double t);

        /// <summary>Whether derivative is defined or not. If false then <see cref="CurveDerivative(double)"/>
        /// cannot be used to calculate tangents on the curve.</summary>
        bool HasDerivative { get; }

        /// <summary>Typical starting value of the parameter of the parametric curve that represents
        /// the knot. <see cref="StartParameter"/> and <see cref="EndParameter"/> should, for true
        /// knots, create a closed curve.</summary>
        double StartParameter { get; }

        /// <summary>Typical end value of the parameter of the parametric curve that represents
        /// the knot. <see cref="StartParameter"/> and <see cref="EndParameter"/> should, for true
        /// knots, create a closed curve.</summary>
        double EndParameter { get; }

    }

}
