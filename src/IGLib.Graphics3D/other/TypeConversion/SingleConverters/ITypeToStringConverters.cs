
#nullable disable

using System;

namespace IGLib.Core
{

    /// <summary>Converts strings to values (objects) of a single specified type (<typeparamref name="TTarget"/>).</summary>
    /// <typeparam name="TTarget">The type of values (objects) to which string representations are converted
    /// by this converter.</typeparam>
    public interface IStringToTypeConverter<TTarget> : ISingleTypeConverter<string, TTarget>
    {  }

    /// <summary>Converts values (objects) of a specific type (<typeparamref name="TSource"/>) to their
    /// string representation. Usually, it should come in paris with <see cref="IStringToTypeConverter{TSource}"/></summary>
    /// <typeparam name="TSource">Type of values / objects that this converter can convert to string representations.</typeparam>
    public interface ITypeToStringConverter<TSource> : ISingleTypeConverter<TSource, string>
    {  }


}
