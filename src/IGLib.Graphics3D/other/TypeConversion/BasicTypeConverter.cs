using System;

namespace IGLib.Core
{
    /// <summary>Provides basic type conversion functionality using IConvertible and nullable handling.
    /// <para>To include array-like conversion, use the <see cref="CollectionTypeConverter"/> class.</para>
    /// </summary>
    public class BasicTypeConverter : ITypeConverter
    {

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public virtual object ConvertToType(object value, Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            try
            {
                if (value == null)
                {
                    if (IsNullableType(targetType))
                        return null;
                    else
                        throw new InvalidOperationException($"Cannot assign null to non-nullable type {TypeName(targetType)}.");
                }

                Type actualTargetType = Nullable.GetUnderlyingType(targetType) ?? targetType;

                if (actualTargetType.IsInstanceOfType(value))
                {
                    return value;
                }

                if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(actualTargetType))
                {
                    return Convert.ChangeType(value, actualTargetType);
                }
                throw new InvalidOperationException($"Cannot convert value of type {TypeName(value.GetType())} to type {TypeName(targetType)}.");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to convert value of type {TypeName(value?.GetType())} to type {TypeName(targetType)}.", ex);
            }
        }

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public T ConvertToType<T>(object value)
        {
            return (T)ConvertToType(value, typeof(T));
        }

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">When this method returns, contains the converted value.</param>
        public void ConvertToType<T>(object value, out T result)
        {
            result = ConvertToType<T>(value);
        }

        /// <summary>
        /// Determines whether the specified type is nullable.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>true if the type is nullable; otherwise, false.</returns>
        public static bool IsNullableType(Type type)
        {
            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>Default value of <see cref="OutputFullTypeNames"/>.</summary>
        public const bool DefaultOutputFullTypeNames = false;

        /// <summary>Whether to take full type names in exception messages and elsewhere.
        /// <para>Default is <see cref="DefaultOutputFullTypeNames"/>.</para></summary>
        public bool OutputFullTypeNames { get; set; } = DefaultOutputFullTypeNames;

        public const string NullTypeString = "[[null type]]";

        /// <summary>Returns name of the specified type <paramref name="type"/>, dependent on the property
        /// <see cref="OutputFullTypeNames"/>: if it is true then the full type name is returned (obtained 
        /// by <see cref="Type.FullName"/>), otherwise the short name is returned (obtained by 
        /// <see cref="Type.Name"/>).</summary>
        /// <param name="type">Type whose name is to be returned. If null then <see cref="NullTypeString"/>
        /// is returned.</param>
        /// <returns>The name of the type <paramref name="type"/>: full name if <see cref="OutputFullTypeNames"/>
        /// is true, short name if it is false, or <see cref="NullTypeString"/> if <paramref name="type"/> is null.</returns>
        protected string TypeName(Type type)
        {
            if (type == null)
            {
                return NullTypeString;
            }
            if (OutputFullTypeNames)
            {
                return type.FullName;
            }
            return type.Name;
        }

    }
}
