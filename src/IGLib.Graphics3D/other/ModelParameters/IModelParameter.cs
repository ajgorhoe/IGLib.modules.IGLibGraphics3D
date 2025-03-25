using System;

namespace IGLib.Core
{

    /// <summary>Contains data on a single model parameter, similar as <see cref="IModelParameter"/>,
    /// but with additional typed propertis to hold the value and default value (<see cref="Value"/>
    /// and <see cref="DefaultValue"/>, respecrively, of the correct type.
    /// </summary>
    /// <typeparam name="ParameterType">Type of parameter's value.</typeparam>
    interface IModelParameter<ParameterType>
    {

        /// <summary>Typed default value of the parameter. </summary>
        ParameterType DefaultValue { get; }

        /// <summary>Typed value of the parameter.</summary>
        ParameterType Value { get; set; }
    }


    /// <summary>Contains data on a single model parameter: parameter name (identifier) <see cref="Name"/>, 
    /// <see cref="Title"/>, <see cref="Description"/>, default value (<see cref="DefaultValueObject"/>), 
    /// current value (<see cref="ValueObject"/>, and whether value is defined (<see cref="IsValueDefined"/>).
    /// <para>Value and default value are properties of type object. The generic Interface 
    /// <see cref="IModelParameter{ParameterType}"/> is a typed variant that has property of the actual 
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
    internal interface IModelParameter
    {

        /// <summary>Name of the parameter, as is used in the model.</summary>
        string Name { get; }

        /// <summary>Parameter type.</summary>
        Type Type { get; }

        /// <summary>Title when introducing the parameter, for example as title in 
        /// documentation, as label in user interfaces where parameter is displayed or 
        /// where it could be set, etc.</summary>
        string Title { get; }

        /// <summary>Description of the parameter, usually a short one, as it may be used in
        /// documentation describing the model or in user interfaces (e.g. as tooltip)</summary>
        string Description { get; }

        /// <summary>Default value of the parameter, stored as object.
        /// <para>Default value can be used to initialize the model, before the parameter
        /// is set to <see cref="ValueObject"/> that specifies actual value of the parameter.</para>
        /// <para>In some use cases, this may not be set.</para></summary>
        object DefaultValueObject { get; }

        /// <summary>Current value of the parameter, stored as object.
        /// <para>In many use cases this class will just hold metadata of the parameter and the
        /// value will not be set</para></summary>
        object ValueObject { get; }


        /// <summary>Whether parameter vlaue is defined or not. This property has been added to the 
        /// class, such that there is a possibility for non-nullable parameter types it is possible
        /// to tell whether the parameter value has been set.</summary>
        bool IsValueDefined { get; }

    }
}