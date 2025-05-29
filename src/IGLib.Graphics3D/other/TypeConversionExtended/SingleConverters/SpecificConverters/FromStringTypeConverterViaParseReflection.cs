using System;
using System.Globalization;
using System.Reflection;

namespace IGLib.Core
{
    /// <summary>
    /// Converts from string to a specific type using TryParse or Parse methods via reflection.
    /// Uses invariant culture to ensure consistent behavior across environments.
    /// </summary>
    public class FromStringTypeConverterViaParseReflection : ISingleSourceTypeConverter<string>
    {
        /// <inheritdoc/>
        public Type SourceType => typeof(string);

        /// <inheritdoc/>
        public TargetType ConvertTyped<TargetType>(string source)
        {
            if (TryConvertTyped<TargetType>(source, out var result))
                return result;

            throw new InvalidCastException($"Cannot convert string to type {typeof(TargetType)}.");
        }

        /// <inheritdoc/>
        public bool TryConvertTyped<TargetType>(string source, out TargetType target)
        {
            Type targetType = typeof(TargetType);

            MethodInfo tryParseWithProvider = targetType.GetMethod("TryParse", new[] { typeof(string), typeof(IFormatProvider), targetType.MakeByRefType() });
            if (tryParseWithProvider != null)
            {
                object[] parameters = new object[] { source, CultureInfo.InvariantCulture, null };
                bool success = (bool)tryParseWithProvider.Invoke(null, parameters);
                if (success)
                {
                    target = (TargetType)parameters[2];
                    return true;
                }
            }

            MethodInfo tryParse = targetType.GetMethod("TryParse", new[] { typeof(string), targetType.MakeByRefType() });
            if (tryParse != null)
            {
                object[] parameters = new object[] { source, null };
                bool success = (bool)tryParse.Invoke(null, parameters);
                if (success)
                {
                    target = (TargetType)parameters[1];
                    return true;
                }
            }

            MethodInfo parseWithProvider = targetType.GetMethod("Parse", new[] { typeof(string), typeof(IFormatProvider) });
            if (parseWithProvider != null)
            {
                try
                {
                    object result = parseWithProvider.Invoke(null, new object[] { source, CultureInfo.InvariantCulture });
                    if (result is TargetType t)
                    {
                        target = t;
                        return true;
                    }
                }
                catch { }
            }

            target = default;
            return false;
        }

        /// <inheritdoc/>
        public object Convert(object source, Type targetType)
        {
            if (TryConvert(source, out var result, targetType))
                return result;

            throw new InvalidCastException($"Cannot convert object of type string to {targetType}.");
        }

        /// <inheritdoc/>
        public bool TryConvert(object source, out object target, Type targetType)
        {
            if (source is string str)
            {
                MethodInfo tryParseWithProvider = targetType.GetMethod("TryParse", new[] { typeof(string), typeof(IFormatProvider), targetType.MakeByRefType() });
                if (tryParseWithProvider != null)
                {
                    object[] parameters = new object[] { str, CultureInfo.InvariantCulture, null };
                    bool success = (bool)tryParseWithProvider.Invoke(null, parameters);
                    if (success)
                    {
                        target = parameters[2];
                        return true;
                    }
                }

                MethodInfo tryParse = targetType.GetMethod("TryParse", new[] { typeof(string), targetType.MakeByRefType() });
                if (tryParse != null)
                {
                    object[] parameters = new object[] { str, null };
                    bool success = (bool)tryParse.Invoke(null, parameters);
                    if (success)
                    {
                        target = parameters[1];
                        return true;
                    }
                }

                MethodInfo parseWithProvider = targetType.GetMethod("Parse", new[] { typeof(string), typeof(IFormatProvider) });
                if (parseWithProvider != null)
                {
                    try
                    {
                        target = parseWithProvider.Invoke(null, new object[] { str, CultureInfo.InvariantCulture });
                        return true;
                    }
                    catch { }
                }
            }

            target = null;
            return false;
        }
    }

}