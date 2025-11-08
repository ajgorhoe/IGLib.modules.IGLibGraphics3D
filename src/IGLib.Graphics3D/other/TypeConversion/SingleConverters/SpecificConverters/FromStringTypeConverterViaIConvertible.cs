
#nullable disable

using System;
using System.Globalization;

namespace IGLib.Core
{

    /// <summary>
    /// Converts a string to a specific type using IConvertible and invariant culture.
    /// Only works with types that implement IConvertible.
    /// </summary>
    public class FromStringTypeConverterViaIConvertible : ISingleSourceTypeConverter<string>
    {
        public Type SourceType => typeof(string);

        public TargetType ConvertTyped<TargetType>(string source)
        {
            if (TryConvertTyped<TargetType>(source, out var result))
                return result;

            throw new InvalidCastException($"Cannot convert string to type {typeof(TargetType)} using IConvertible.");
        }

        public bool TryConvertTyped<TargetType>(string source, out TargetType target)
        {
            try
            {
                object result = System.Convert.ChangeType(source, typeof(TargetType), CultureInfo.InvariantCulture);
                if (result is TargetType t)
                {
                    target = t;
                    return true;
                }
            }
            catch { }

            target = default;
            return false;
        }

        public object Convert(object source, Type targetType)
        {
            if (TryConvert(source, out var result, targetType))
                return result;

            throw new InvalidCastException($"Cannot convert string to type {targetType} using IConvertible.");
        }

        public bool TryConvert(object source, out object target, Type targetType)
        {
            if (source is string str)
            {
                try
                {
                    target = System.Convert.ChangeType(str, targetType, CultureInfo.InvariantCulture);
                    return true;
                }
                catch { }
            }

            target = null;
            return false;
        }

    }

}
