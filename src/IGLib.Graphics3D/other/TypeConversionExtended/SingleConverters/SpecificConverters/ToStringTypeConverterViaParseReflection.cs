using System;
using System.Globalization;
using System.Reflection;

namespace IGLib.Core
{

    /// <summary>
    /// Converts a value to its string representation using invariant culture,
    /// only if it can be parsed back using TryParse or Parse methods.
    /// </summary>
    public class ToStringTypeConverterViaParseReflection : ISingleTargetTypeConverter<string>
    {
        /// <inheritdoc/>
        public Type TargetType => typeof(string);

        /// <inheritdoc/>
        public string ConvertTyped<SourceType>(SourceType source)
        {
            if (TryConvertTyped(source, out string result))
                return result;

            throw new InvalidCastException($"Cannot convert value of type {typeof(SourceType)} to string in a round-trip safe way.");
        }

        /// <inheritdoc/>
        public bool TryConvertTyped<SourceType>(SourceType source, out string target)
        {
            Type sourceType = typeof(SourceType);
            if (!IsRoundTripConvertible(sourceType))
            {
                target = null;
                return false;
            }

            try
            {
                target = System.Convert.ToString(source, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                target = null;
                return false;
            }
        }

        /// <inheritdoc/>
        public object Convert(object source)
        {
            if (TryConvert(source, out object result))
                return result;

            throw new InvalidCastException($"Cannot convert object of type {source?.GetType()} to string in a round-trip safe way.");
        }

        /// <inheritdoc/>
        public bool TryConvert(object source, out object target)
        {
            if (source == null)
            {
                target = null;
                return true;
            }

            Type sourceType = source.GetType();
            if (!IsRoundTripConvertible(sourceType))
            {
                target = null;
                return false;
            }

            try
            {
                target = System.Convert.ToString(source, CultureInfo.InvariantCulture);
                return true;
            }
            catch
            {
                target = null;
                return false;
            }
        }

        /// <summary>
        /// Determines whether a type supports round-trip conversion using TryParse or Parse with InvariantCulture.
        /// </summary>
        private bool IsRoundTripConvertible(Type type)
        {
            if (type.GetMethod("TryParse", new[] { typeof(string), typeof(IFormatProvider), type.MakeByRefType() }) != null)
                return true;

            if (type.GetMethod("TryParse", new[] { typeof(string), type.MakeByRefType() }) != null)
                return true;

            if (type.GetMethod("Parse", new[] { typeof(string), typeof(IFormatProvider) }) != null)
                return true;

            return false;
        }
    }
}