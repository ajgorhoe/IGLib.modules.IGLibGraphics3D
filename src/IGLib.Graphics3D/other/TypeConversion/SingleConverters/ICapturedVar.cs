using System;

namespace IGLib.Core
{

    /// <summary>Interface for the <see cref="CapturedVar{DeclaredType}"/> and its derived types. See
    /// documetation for that type for more information.</summary>
    /// <typeparam name="DeclaredType"></typeparam>
    /// <seealso cref="CapturedVar{DeclaredType}">The implementing type that is of most practical importance.</seealso>
    public interface ICapturedVar<DeclaredType> : ICapturedVar
    {

        /// <summary>Value of the captured enntity encapsulated by this class (a variable, property, fiels, or
        /// value or expression whose type can be inferred).</summary>
        DeclaredType Value { get; init; }

    }

    /// <summary>Base interface for <see cref="CapturedVar{DeclaredType}"/>. This interface is provided such
    /// that in some scenarios, the generic type does not need to be provided.</summary>
    public interface ICapturedVar
    {

        /// <summary>Untyped value of the entity captured by the instance of this interface.</summary>
        object ValueObject { get; }

        Type Type { get; }

        string TypeName { get; }

        string TypeFullName { get; }

        string TypeString { get; }


        Type ValueType { get; }

        string ValueTypeName { get; }

        string ValueTypeFullName { get; }

        string ValueTypeString { get; }


        string ToString();

        string ToStringLong(string indentation = "  ");

    }


}
