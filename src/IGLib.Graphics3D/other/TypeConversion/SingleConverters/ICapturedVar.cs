
#nullable disable

using System;
using IGLib.Core.CollectionExtensions;

namespace IGLib.Core
{

    /// <summary>Interface for the <see cref="CapturedVar{DeclaredType}"/> and its derived types. See
    /// documetation for that type for more information.</summary>
    /// <typeparam name="DeclaredType"></typeparam>
    /// <seealso cref="CapturedVar{DeclaredType}">The implementing type that is of most practical importance.</seealso>
    public interface ICapturedVar<DeclaredType> : ICapturedVar
    {

        /// <summary>Value of the captured entity encapsulated by this class (a variable, property, field, or
        /// value or expression whose type can be inferred by the compiler).</summary>
        DeclaredType Value { get; init; }

    }

    /// <summary>Base interface for <see cref="CapturedVar{DeclaredType}"/>. This interface is provided such
    /// that in some scenarios, the generic type does not need to be provided (according to the interface 
    /// segragation principle).</summary>
    public interface ICapturedVar
    {

        /// <summary>Untyped value of the entity captured by the instance of the class, typed as <see cref="object"/>.</summary>
        object ValueObject { get; }

        /// <summary>The declared type of the captured entity (variable, property, field, or expression / value of a known type).</summary>
        Type Type { get; }

        /// <summary>Short name of the declared type of the captured entity, the <see cref="Type"/>, produced
        /// by Type.Name.</summary>
        string TypeName { get; }

        /// <summary>Full name of the declared type of the captured entity, the <see cref="Type"/>, produced
        /// by <see cref="Type.FullName"/>.</summary>
        string TypeFullName { get; }

        /// <summary>Readable name of the declared type of the captured entity, the <see cref="Type"/>, produced
        /// by <see cref="Type.ToString"/>().</summary>
        string TypeString { get; }


        /// <summary>The actual type of the captured entity's value.</summary>
        Type ValueType { get; }

        /// <summary>Short name of the actual type of the captured entity's value, the <see cref="ValueType"/>, 
        /// produced by Type.Name.</summary>
        string ValueTypeName { get; }

        /// <summary>Full name of the actual type of the captured entity's value, the <see cref="ValueType"/>, 
        /// produced by <see cref="Type.FullName"/>.</summary>
        string ValueTypeFullName { get; }

        /// <summary>Readable name of the actual type of the captured entity's value, the <see cref="ValueType"/>, 
        /// produced by <see cref="Type.ToString"/>().</summary>
        string ValueTypeString { get; }

        /// <summary>Returns a short representation of the current object, containing the captured entity's
        /// declared type, actual type of its value, and readable representation of its value.</summary>
        string ToString();

        /// <summary>Returns a detailed representation of the current object, including all the properties,
        /// which includes short and long forms of the declared type of the captured entity and actual type
        /// of its value, and the captured value itself.</summary>
        /// <param name="indentation">String usef for indentation of the produced string representatino. If
        /// not specified then the default indentation string is used.</param>
        /// <returns></returns>
        string ToStringLong(string indentation = 
            CollectionExtensions.CollectionExtensions.DefaultIndentationString);

    }


}
