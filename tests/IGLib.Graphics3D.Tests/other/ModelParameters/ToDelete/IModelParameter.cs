
#nullable disable

using System;

// namespace IGLib.Core.Experimental
namespace YourNamespace
{

    ///// <summary>Contains data for a single model parameter, similar as <see cref="IModelParameter"/>,
    ///// but with additional typed propertis to hold the strongly typed value and default value 
    ///// (properties <see cref="Value"/> and <see cref="DefaultValue"/>, respecrively.
    ///// <para>See also the <seealso cref="IModelParameter"/> base intrface (not typed) for more details.</para></summary>
    ///// 
    ///// <typeparam name="ValueType">Type of parameter's value.</typeparam>
    //public interface IModelParameterEXP<ValueType>
    //{

    //    /// <summary>Typed default value of the parameter. </summary>
    //    ValueType DefaultValue { get; }

    //    /// <summary>Typed value of the parameter.</summary>
    //    ValueType Value { get; set; }

    //    IModelParameterEXP<ValueType> UpdateValue(ValueType newValue);

    //    IModelParameterEXP<ValueType> UpdateDefaultValue(ValueType newValue);

    //}


    /// <summary>Contains data for a single model parameter: parameter name (identifier) <see cref="Name"/>, 
    /// <see cref="Title"/>, <see cref="Description"/>, default value (<see cref="DefaultValueObject"/>), 
    /// current value (<see cref="ValueObject"/>, and whether value is defined (<see cref="IsValueDefined"/>).
    /// <para>Value and default value are properties of type object. The generic Interface 
    /// <see cref="IModelParameterEXP{ParameterType}"/> is a typed variant that has property of the actual 
    /// parameter type.</para>
    /// <para>Dependent on use case, this class may serve to transfer parameter values accompanied with
    /// metadata, and in other cases it may just be used to store parameter's metadata.</para>
    /// <para>Objects of this type are usually stored in <see cref="IParameterSet"/> objects, which
    /// contain values and metadata on parameters of a specific parameterized model. Some examples of
    /// parameterized models: functions that have parameters additional to independent variables; 
    /// classes of curves in 2D and 3D or surfaces in 3D that have additional parameters used
    /// to modify the specific parameterization of such objects (e.g. ellipsoid in 3D may be
    /// parameterized with u and v being independent variables, and the actual surface can be adjusted
    /// (modified) by a set of other parameters such as half-aces of the ellipsoid, shift of the center
    /// from the origin of the coordinate system, and rotations)/</para></summary>
    public interface IModelParameter
    {

        /// <summary>Name of the parameter, as is used in the model.</summary>
        string Name { get; }

        /// <summary>Parameter type.</summary>
        Type Type { get; }


        /// <summary>Default value of the parameter, stored as object.
        /// <para>Default value can be used to initialize the model, before the parameter
        /// is set to <see cref="ValueObject"/> that specifies actual value of the parameter.</para>
        /// <para>In some use cases, this may not be set.</para></summary>
        object DefaultValueObject { get; }

        /// <summary>Whether default parameter vlaue is defined or not. This property has been added 
        /// to the  class such that for non-nullable parameter types it is possible to tell whether 
        /// the default parameter value has been set or not.
        /// <para>Should be initialized to <see cref="ModelParameter.InitialIsDefaultValueDefined"/></para></summary>
        bool IsDefaultValueDefined { get; }

        /// <summary>Current value of the parameter, stored as object.
        /// <para>In many use cases this class will just hold metadata of the parameter and the
        /// value will not be set</para></summary>
        object ValueObject { get; set; }

        /// <summary>Whether parameter vlaue is defined or not. This property has been added 
        /// to the  class such that for non-nullable parameter types it is possible to tell whether 
        /// the parameter value has been set or not.
        /// <para>Should be initialized to <see cref="ModelParameter.InitialIsValueDefined"/></para></summary>
        bool IsValueDefined { get; }


    }
}