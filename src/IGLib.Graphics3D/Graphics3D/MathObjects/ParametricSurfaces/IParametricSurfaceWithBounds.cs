
#nullable disable

using IG.Num;

namespace IGLib.Gr3D
{

    /// <summary>Parametric surface in 3D. Beside the <see cref="Surface"/> and (optional) <see cref="SurfaceDerivative1(double, double)"/>
    /// and <see cref="SurfaceDerivative2(double, double)"/> it also contains typical bounds on parameters,
    /// <see cref="StartParameter1"/>, <see cref="EndParameter1"/>,  <see cref="StartParameter2"/> and <see cref="EndParameter2"/>.
    /// These usually denote bounds on the surface parameters that are used by default when the urface is tabulated, 
    /// plotted, for meshes or for numerical calculations
    /// Sometimes, these properties can also represent the range of parameter values for which the surface is defined,
    /// though rarely. More often, they represent some characteristic or natural range for the surface in 
    /// question, and sometimes theey are not used at all (the convention is to set them to 0 in such cases).
    /// For mathematical knots (slosed ones), these properties are usually set to such values that the represented 
    /// surfaces exactly closes when parameter runs across the specified range.</summary>
    public interface IParametricSurfaceWithBounds
    {

        /// <summary>Defines the parametric surface in 3D space.</summary>
        /// <param name="u">Th first parameter of the surface parameterization.</param>
        /// <param name="v">The second parameter of the surface parameterization.</param>
        vec3 Surface(double u, double v);

        /// <summary>Defines the derivative of <see cref="Surface"/> with respect to the first parameter.
        /// In each point on the surface (for each pair of parameter values), this defines the tangent on the 
        /// surface in the directio of the first coordinate curve.
        /// <para>You can use <see href="https://www.derivative-calculator.net/">Derivative Calculator</see>
        /// to calculate or verify the derivatives.</para></summary>
        vec3 SurfaceDerivative1(double u, double v);

        /// <summary>Defines the derivative of <see cref="Surface"/> with respect to the second parameter.
        /// In each point on the surface (for each pair of parameter values), this defines the tangent on the 
        /// surface in the direction of  the second coordinate curve.
        /// <para>You can use <see href="https://www.derivative-calculator.net/">Derivative Calculator</see>
        /// to calculate or verify the derivatives.</para></summary>
        vec3 SurfaceDerivative2(double u, double v);

        /// <summary>Whether derivatives are specified or not. If false then <see cref="SurfaceDerivative1(double, double)"/>
        /// cand  <see cref="SurfaceDerivative2(double, double)"/> cannot be used to calculate tangents on surface.</summary>
        bool HasDerivative { get; }

        /// <summary>Typical starting value of the first parameter of the parametric surface.</summary>
        double StartParameter1 { get; }

        /// <summary>Typical end value of the second parameter of the parametric surface.</summary>
        double EndParameter1 { get; }

        /// <summary>Typical starting value of the first parameter of the parametric surface.</summary>
        double StartParameter2 { get; }

        /// <summary>Typical end value of the second parameter of the parametric surface.</summary>
        double EndParameter2 { get; }

    }

}
