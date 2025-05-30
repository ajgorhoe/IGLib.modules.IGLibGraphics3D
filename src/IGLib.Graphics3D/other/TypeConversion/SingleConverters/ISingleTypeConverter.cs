using System;

namespace IGLib.Core
{

    public interface IStringToTypeConverter<TargetType> : ISingleTypeConverter<string, TargetType>
    {  }

    public interface ITypeToStringConverter<SourceType> : ISingleTypeConverter<SourceType, string>
    {  }


    /// <summary>Converts values from a single specific source type (<typeparamref name="SourceType"/>) 
    /// to a single specific target type (<typeparamref name="TargetType"/>). Conversion methods are not typed and have parameters and
    /// return value of type object.
    /// <para>Use the generic <see cref="ISingleTypeConverter{SourceType, TargetType}"/> counterpart in order
    /// to include mathods with typed parameters.</para></summary>
    public interface ISingleTypeConverter<TSource, TTarget> : ISingleTypeConverter
    {

        /// <summary>Converts the parameter <paramref name="source"/> to the target type (<typeparamref name="TTarget"/>,
        /// or equivalently, <see cref="TargetType"/>), and returns the converted value.
        /// <para>Normally, throws an exception if the conversion cannot be performed.</para></summary>
        /// <param name="source">Object to be converted to the target type (<typeparamref name="TTarget"/>,
        /// or equivalently, <see cref="TargetType"/>).</param>
        /// <returns>The converted value, assignable to <see cref="TargetType"/> (or equivalently, <typeparamref name="TTarget"/>.)</returns>
        TTarget ConvertTyped(TSource source);

        /// <summary>Converts the parameter <paramref name="source"/> to the target type (<typeparamref name="TTarget"/>,
        /// or equivalently, <see cref="TargetType"/>), and stores the converted value (object) in <paramref name="target"/>.
        /// Returns true if conversion was successful, or false if it could not be performed.
        /// <para>Normally, it huld not throw exceptions because eventual failure to perform type conversion is 
        /// communicated to the caller via return value.</para></summary>
        /// <param name="source">The value (object) that is converted to the target type (<typeparamref name="TTarget"/>,
        /// or, equivalently, <see cref="TargetType"/>).</param>
        /// <param name="target">Reference where the converted value (object) is stored upon successful type conversion.</param>
        /// <returns>True if type conversion succeeded, false if not.</returns>
        bool TryConvertTyped(TSource source, out TTarget target);


        /// <summary>Source type, the type of values FROM which the current converter converts.
        /// Should correspond to <typeparamref name="TSource"/>.</summary>
        new Type SourceType { get; }

        /// <summary>Target type, the type of values TO which the current converter converts.
        /// Should correspond to <typeparamref name="TTarget"/>.</summary>
        new Type TargetType { get; }


    }

    /// <summary>Converts values from a single specific source type (<see cref="SourceType"/>) to a single 
    /// specific target type (<see cref="TargetType"/>). Conversion methods are not typed and have parameters 
    /// and return value of type object.
    /// <para>Use the generic <see cref="ISingleTypeConverter{SourceType, TargetType}"/> counterpart in order
    /// to include mathods with typed parameters.</para></summary>
    public interface ISingleTypeConverter
    {
    
        /// <summary>Source type, the type of values FROM which the current converter converts.</summary>
        Type SourceType { get; }
    
        /// <summary>Target type, the type of values TO which the current converter converts.</summary>
        Type TargetType { get; }
        
        /// <summary>Converts the value (object) <paramref name="source"/> of type <see cref="SourceType"/> to a value 
        /// (object) of type <see cref="TargetType"/> and returns it.
        /// <para>Throws an exception (normally, an <see cref="InvalidCastException"/> or <see cref="InvalidOperationException"/>)
        /// if the conversion cannot be performed (also in some cases <see cref="ArgumentException"/> or <see cref="ArgumentNullException"/>
        /// when the type of <paramref name="source"/> is incorrct or its value is null).</para></summary>
        /// <param name="source">Source value that is convertd to type <see cref="TargetType"/>. Its actual type must be 
        /// assignable to the type <see cref="SourceType"/>.</param>
        /// <returns>A value of type <see cref="TargetType"/>, convered from <paramref name="source"/>.</returns>
        object Convert(object source);


        /// <summary>Converts the value <paramref name="source"/> of type <see cref="SourceType"/> to a value of type 
        /// <see cref="TargetType"/> and stores the converted value (object) in <paramref name="target"/>.
        /// <para>Returns true if the conversion could be performed successfully, and false if not. It shoould normally 
        /// not throw exceptions, because success of conversion is communicated to the caller via returned value.</para></summary>
        /// <param name="source">Source value that is convertd to type <see cref="TargetType"/>. Its actual type must be 
        /// assignable to the type <see cref="SourceType"/>.</param>
        /// <param name="target">Object wheere the converted value is stored.</param>
        /// <returns>True if the conversion is successful, false if not.</returns>
        bool TryConvert(object source, out object target);

    }

}
