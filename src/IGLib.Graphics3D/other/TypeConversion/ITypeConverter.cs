using System;

namespace IGLib.Core
{
    /// <summary>Defines methods for converting objects to a specified type.
    /// <para>Implemented by hierarchy of type converters, including <see cref="BasicTypeConverter"/>
    /// and <see cref="CollectionTypeConverter"/>. One reason for inheritance hierarchy of converters
    /// is to have layered hierarchy with increasing capability but also increasing dependencies (e.g.,
    /// one policy is to keep code that requires advanced reflection (it needs System.Reflection namespace)
    /// out of lower layers of libraries such that it does not interfere with trimming, which is needed
    /// in many deployment scenarios such as Ahead of Time compilation, WebAssembly deployments like in
    /// Blazor client profiles, self-contained deployments to embedded and other resource-limited scenarios).</para>
    /// </summary>
    public interface ITypeConverter
    {
        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="targetType">The type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        object ConvertToType(object value, Type targetType);

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        T ConvertToType<T>(object value);

        /// <summary>
        /// Converts the specified value to the specified target type.
        /// </summary>
        /// <typeparam name="T">The type to convert the value to.</typeparam>
        /// <param name="value">The value to convert.</param>
        /// <param name="result">When this method returns, contains the converted value.</param>
        void ConvertToType<T>(object value, out T result);
    }
}
