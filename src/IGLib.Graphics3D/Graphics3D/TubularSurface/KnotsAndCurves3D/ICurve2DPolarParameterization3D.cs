using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>Cylindrical parameterization of a curve in 3D. Beside the <see cref="CurveCylindrical"/> and 
    /// (optional) <see cref="CurveDerivativeCylindrical"/>, it also contains bounds on parameter, 
    /// <see cref="StartParameterCylindrical"/> and <see cref="EndParameterCylindrical"/> as part of the class.
    /// </summary>
    interface ICurve2DPolarParameterization
    {

        /// <summary>Planar curve (in XY plane) in polar form (of the form r = r(φ)).</summary>
        double CurvePolar(double phi);

        /// <summary>Derivative of the planar curve (in XY plane) of form r = r(φ) with respect to polar angle.</summary>
        double CurveDerivativePolar(double phi);

        /// <summary>Whether the derivative <see cref="CurveDerivativePolar(double)"/> is defined or not. If 
        /// false then <see cref="CurveDerivativePolar(double)"/> cannot be used.</summary>
        bool HasDerivativePolar { get; }

        /// <summary>Typical starting value of the parameter φ of the parametric curve that represents
        /// the curve. Interval from <see cref="StartParameter"/> to <see cref="EndParameter"/> should
        /// produce a meaningful / representative curve section.</summary>
        double StartParameter { get; }

        /// <summary>Typical end value of the parameter φ of the parametric curve that represents
        /// the curve. Interval from <see cref="StartParameter"/> to <see cref="EndParameter"/> should
        /// produce a meaningful / representative curve section.</summary>
        double EndParameter { get; }

    }

}
