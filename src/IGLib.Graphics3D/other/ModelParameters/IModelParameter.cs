using System;

namespace IGLib.Core
{

    /// <summary>Contains data for a single model parameter, similar as <see cref="IModelParameter"/>,
    /// but with additional typed propertis to hold the strongly typed value and default value 
    /// (properties <see cref="Value"/> and <see cref="DefaultValue"/>, respecrively.
    /// <para>See also the <seealso cref="IModelParameter"/> base intrface (not typed) for more details.</para></summary>
    /// 
    /// <typeparam name="ValueType">Type of parameter's value.</typeparam>
    public interface IModelParameter<ValueType>
    {

        /// <summary>Typed default value of the parameter. </summary>
        ValueType DefaultValue { get; }

        /// <summary>Typed value of the parameter.</summary>
        ValueType Value { get; set; }

        IModelParameter<ValueType> UpdateValue(ValueType newValue);

        IModelParameter<ValueType> UpdateDefaultValue(ValueType newValue);

    }


    /// <summary>Contains data for a single model parameter: parameter name (identifier) <see cref="Name"/>, 
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
    public interface IModelParameter
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

        /// <summary>If true then the current parameter is assumed to have the default value when
        /// the value itself (property <see cref="ValueObject"/>) haws not been defined (i.e.,
        /// <see cref="IsValueDefined"/> = false), but the default value is defined 
        /// (<see cref="IsDefaultValueDefined"/> = true).
        /// <para>Default value of this property in most implementations is
        /// <see cref="ModelParameter.DefaultIsDefaultWhenValueNotDefined"/>.</para></summary>
        bool IsDefaultWhenValueNotDefined { get; }

        /// <summary>Whether the current parameter is a constant, i.e., it cannot change its value 
        /// once it is set.
        /// <para>Correct behavior is that when this property is true, neither the value (<see cref="ValueObject"/>)
        /// nor the default value (<see cref="DefaultValueObject"/>) cannot be set if they are already defined (which
        /// is checked via properties <see cref="IsValueDefined"/> and <see cref="IsDefaultValueDefined"/>).</para>
        /// <para> The same is true for properties <see cref="ModelParameter{ValueType}.Value"/> and
        /// <see cref="ModelParameter{ValueType}.DefaultValue"/> in strongly typed paremeter objects (interface 
        /// <see cref="IModelParameter{ValueType}"/>).</para>
        /// <para>Default value of this property is <see cref="ModelParameter.DefaultIsConstant"/>.</para></summary>
        bool IsConstant { get; }

        /// <summary>Clears the value of the current parameter, making it undefined (<see cref="IsValueDefined"/> becomes false).</summary>
        /// <returns>The current parameter object, enabling method chaining (fluent API).</returns>
        IModelParameter ClearValue();

        /// <summary>Updates the value of the current parameter object, setting it to <paramref name="newValue"/>.
        /// <see cref="IsValueDefined"/> becomes true, even if it was false before the call.</summary>
        /// <returns>The current parameter object, enabling method chaining (fluent API).</returns>
        IModelParameter UpdateValue(object newValue);

        /// <summary>Clears the default value of the current parameter, making it undefined (<see cref="IsDefaultValueDefined"/> 
        /// becomes false).</summary>
        /// <returns>The current parameter object, enabling method chaining (fluent API).</returns>
        IModelParameter ClearDefaultValue();

        /// <summary>Updates the default value of the current parameter object, setting it to <paramref name="newDefaultValue"/>.
        /// <see cref="IsDefaultValueDefined"/> becomes true, even if it was false before the call.</summary>
        /// <returns>The current parameter object, enabling method chaining (fluent API).</returns>
        IModelParameter UpdateDefaultValue(object newDefaultValue);

        /// <summary>Updates the title of the current parameter (property <see cref="Title"/>).</summary>
        /// <param name="newTitle">The new value of the title; null is allowed.</param>
        /// <returns>The current parameter object, enabling method chaining (fluent API).</returns>
        IModelParameter UpdateTitle(string newTitle);

        /// <summary>Updates the description of the current parameter (property <see cref="Description"/>).</summary>
        /// <param name="newDescription">The new value of the description; null is allowed.</param>
        /// <returns>The current parameter object, enabling method chaining (fluent API).</returns>
        IModelParameter UpdateDescription(string newDescription);

    }
}