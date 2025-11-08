
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IGLib.Core;


namespace IGLib.CoreExtended
{



    /// <summary>
    /// A type converter that supports implicit/explicit conversion operators via reflection,
    /// including base class and interface fallback and optional caching.
    /// </summary>
    public class ReflectionTypeConverter : CollectionTypeConverter
    {
        private static readonly Dictionary<(Type Source, Type Target), Func<object, object>> _conversionCache = new();

        public virtual object ConvertToType(
            object value,
            Type targetType,
            bool allowSourceBaseConversions = true,
            bool allowInterfaceConversions = true)
        {
            if (value == null)
            {
                if (targetType.IsValueType && Nullable.GetUnderlyingType(targetType) == null)
                    throw new InvalidOperationException($"Cannot convert null to non-nullable type {targetType.FullName}.");
                return null;
            }

            var sourceType = value.GetType();
            var actualTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (actualTargetType.IsInstanceOfType(value))
                return value;

            var cacheKey = (Source: sourceType, Target: actualTargetType);
            if (_conversionCache.TryGetValue(cacheKey, out var cachedConverter))
            {
                if (cachedConverter == null)
                    throw new InvalidOperationException($"No conversion found from {sourceType} to {actualTargetType}.");
                return cachedConverter(value);
            }

            try
            {
                return base.ConvertToType(value, targetType);
            }
            catch (InvalidOperationException)
            {
                // Fallback to operator-based resolution
            }

            Func<object, object> converter = TryFindOperatorConverter(sourceType, actualTargetType, allowSourceBaseConversions, allowInterfaceConversions);

            if (converter == null)
            {
                _conversionCache[cacheKey] = null;
                throw new InvalidOperationException($"No conversion found from {sourceType.FullName} to {actualTargetType.FullName}.");
            }

            _conversionCache[cacheKey] = converter;
            return converter(value);
        }

        public override object ConvertToType(object value, Type targetType)
        {
            return ConvertToType(value, targetType, allowSourceBaseConversions: true, allowInterfaceConversions: true);
        }

        private Func<object, object> TryFindOperatorConverter(
            Type sourceType,
            Type targetType,
            bool allowSourceBaseConversions,
            bool allowInterfaceConversions)
        {
            // Try direct on source
            var op = FindOperatorMethod(sourceType, sourceType, targetType);
            if (op != null) return op;

            // Try direct on target
            op = FindOperatorMethod(targetType, sourceType, targetType);
            if (op != null) return op;

            // Try base types
            if (allowSourceBaseConversions)
            {
                Type current = sourceType.BaseType;
                while (current != null && current != typeof(object))
                {
                    op = FindOperatorMethod(current, current, targetType);
                    if (op != null) return op;

                    op = FindOperatorMethod(targetType, current, targetType);
                    if (op != null) return op;

                    current = current.BaseType;
                }
            }

            // Try interfaces
            if (allowInterfaceConversions)
            {
                foreach (var iface in sourceType.GetInterfaces())
                {
                    op = FindOperatorMethod(iface, iface, targetType);
                    if (op != null) return op;

                    op = FindOperatorMethod(targetType, iface, targetType);
                    if (op != null) return op;
                }
            }

            return null;
        }

        private Func<object, object> FindOperatorMethod(Type definingType, Type sourceType, Type targetType)
        {
            var methods = definingType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                            m.ReturnType == targetType &&
                            m.GetParameters().Length == 1 &&
                            m.GetParameters()[0].ParameterType.IsAssignableFrom(sourceType));

            foreach (var method in methods)
            {
                return value =>
                {
                    try
                    {
                        return method.Invoke(null, new[] { value });
                    }
                    catch (TargetInvocationException tie)
                    {
                        throw new InvalidOperationException(
                            $"Failed to invoke {method.Name} from {sourceType.FullName} to {targetType.FullName}.",
                            tie.InnerException);
                    }
                };
            }

            return null;
        }
    }



}