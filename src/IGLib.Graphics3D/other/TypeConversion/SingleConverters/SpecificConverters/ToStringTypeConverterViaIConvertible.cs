using System;
using System.Globalization;

namespace IGLib.Core
{

    /// <summary>
    /// Converts a value to its string representation using IConvertible and invariant culture.
    /// Only works with types that implement IConvertible.
    /// </summary>
    public class ToStringTypeConverterViaIConvertible : ISingleTargetTypeConverter<string>
    {   
        public Type TargetType => typeof(string);

        public string ConvertTyped<SourceType>(SourceType source)
        {
            if (TryConvertTyped(source, out string result))
                return result;

            throw new InvalidCastException($"Cannot convert value of type {typeof(SourceType)} to string using IConvertible.");
        }

        public bool TryConvertTyped<SourceType>(SourceType source, out string target)
        {
            if (source is IConvertible convertible)
            {
                try
                {
                    target = convertible.ToString(CultureInfo.InvariantCulture);
                    return true;
                }
                catch { }
            }

            target = null;
            return false;
        }

        public object Convert(object source)
        {
            if (TryConvert(source, out object result))
                return result;

            throw new InvalidCastException($"Cannot convert object of type {source?.GetType()} to string using IConvertible.");
        }

        public bool TryConvert(object source, out object target)
        {
            if (source is IConvertible convertible)
            {
                try
                {
                    target = convertible.ToString(CultureInfo.InvariantCulture);
                    return true;
                }
                catch { }
            }

            target = null;
            return false;
        }
    }

}
