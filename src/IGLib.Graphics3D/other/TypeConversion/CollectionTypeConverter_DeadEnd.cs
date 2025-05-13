using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;


namespace IGLib.Core
{


    /// <summary>
    /// Converts collection types to other collection types, including flattening 
    /// multi-dimensional arrays to one-dimensional generic collections.
    /// </summary>
    /// <remarks>
    /// Supports converting rectangular multi-dimensional arrays (e.g. int[,]) into 
    /// List{T}, IList{T}, or IEnumerable{T} by flattening the array. Each element 
    /// of the source array is converted to the target type T and added to a new List{T}. 
    /// List{T} is used as the concrete implementation for IList{T} and IEnumerable{T} results. 
    /// This converter does not reshape or convert one-dimensional collections into multi-dimensional arrays. 
    /// All used APIs are available in .NET Framework 4.8 and .NET Standard 2.0. 
    /// </remarks>
    [Obsolete("Dead end (heavy weight dependencies), will be removed.")]
    public class CollectionTypeConverter_DeadEnd : TypeConverter
    {
        // ... (other members)

        /// <inheritdoc />
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType != null && destinationType.IsGenericType)
            {
                Type genericDef = destinationType.GetGenericTypeDefinition();
                // If target is List<T>, IList<T>, or IEnumerable<T>, allow conversion (for appropriate source).
                if (genericDef == typeof(List<>) || genericDef == typeof(IList<>) || genericDef == typeof(IEnumerable<>))
                {
                    return true;
                }
            }
            // Fallback to base or other logic for other types
            return base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc />
        public override object? ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            // Handle multi-dimensional array to generic List/IList/IEnumerable conversion
            if (value is Array array
                && array.Rank > 1
                && destinationType.IsGenericType
                && (destinationType.GetGenericTypeDefinition() == typeof(List<>)
                    || destinationType.GetGenericTypeDefinition() == typeof(IList<>)
                    || destinationType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                // Determine the target element type T
                Type targetElementType = destinationType.GetGenericArguments()[0];
                // Determine the source element type (e.g. int for int[,])
                Type sourceElementType = value.GetType().GetElementType()!;

                // Create a List<T> to hold the converted elements
                Type listType = typeof(List<>).MakeGenericType(targetElementType);
                IList resultList = (IList)Activator.CreateInstance(listType)!;

                // Flatten the array and convert each element
                foreach (object? element in array)
                {
                    object? convertedElement = element;
                    if (convertedElement != null && !targetElementType.IsInstanceOfType(convertedElement))
                    {
                        // Convert element to the target type if necessary
                        TypeConverter elemConverter = TypeDescriptor.GetConverter(sourceElementType);
                        if (elemConverter != null && elemConverter.CanConvertTo(context, targetElementType))
                        {
                            convertedElement = elemConverter.ConvertTo(context, culture, element, targetElementType);
                        }
                        else
                        {
                            TypeConverter targetConverter = TypeDescriptor.GetConverter(targetElementType);
                            if (targetConverter != null && targetConverter.CanConvertFrom(context, sourceElementType))
                            {
                                convertedElement = targetConverter.ConvertFrom(context, culture, element);
                            }
                            else
                            {
                                // Fallback: use ChangeType for IConvertible types
                                convertedElement = System.Convert.ChangeType(element, targetElementType, culture);
                            }
                        }
                    }
                    resultList.Add(convertedElement);
                }

                // Return the List<T> (as List or as IList/IEnumerable as required by destinationType)
                return resultList;
            }

            // For other conversions, use the base implementation or other existing logic
            return base.ConvertTo(context, culture, value, destinationType);
        }

        // ... (possibly other ConvertFrom overrides, etc.)
    }



}
