
using System;
using System.Linq;
using System.Reflection;
using IGLib.Core;


namespace IGLib.CoreExtended
{

    /// <summary>
    /// A type converter that extends <see cref="CollectionTypeConverter"/> to support implicit and explicit
    /// user-defined conversion operators using reflection.
    /// </summary>
    public class ReflectionTypeConverter : CollectionTypeConverter
    {
        /// <summary>
        /// Converts the given value to the specified target type, including support for user-defined
        /// implicit and explicit conversions via reflection.
        /// </summary>
        /// <param name="value">The input value to convert.</param>
        /// <param name="targetType">The desired target type.</param>
        /// <returns>The converted value.</returns>
        public override object ConvertToType(object value, Type targetType)
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

            // Try base conversion first (collections, primitives, etc.)
            try
            {
                return base.ConvertToType(value, targetType);
            }
            catch (InvalidOperationException)
            {
                // Continue to try user-defined conversion operators
            }

            // Look for implicit or explicit conversion operators on either side
            var methods = sourceType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                            m.ReturnType == actualTargetType &&
                            m.GetParameters().Length == 1 &&
                            m.GetParameters()[0].ParameterType.IsAssignableFrom(sourceType));

            foreach (var method in methods)
            {
                try
                {
                    return method.Invoke(null, new[] { value });
                }
                catch (TargetInvocationException tie)
                {
                    throw new InvalidOperationException(
                        $"Failed to invoke {method.Name} operator for conversion from {sourceType.FullName} to {actualTargetType.FullName}.",
                        tie.InnerException);
                }
            }

            // Also check for conversions defined on the target type
            var reverseMethods = actualTargetType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(m => (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                            m.ReturnType == actualTargetType &&
                            m.GetParameters().Length == 1 &&
                            m.GetParameters()[0].ParameterType.IsAssignableFrom(sourceType));

            foreach (var method in reverseMethods)
            {
                try
                {
                    return method.Invoke(null, new[] { value });
                }
                catch (TargetInvocationException tie)
                {
                    throw new InvalidOperationException(
                        $"Failed to invoke {method.Name} operator for conversion from {sourceType.FullName} to {actualTargetType.FullName}.",
                        tie.InnerException);
                }
            }

            throw new InvalidOperationException($"No conversion found from {sourceType.FullName} to {actualTargetType.FullName}.");
        }
    }

}