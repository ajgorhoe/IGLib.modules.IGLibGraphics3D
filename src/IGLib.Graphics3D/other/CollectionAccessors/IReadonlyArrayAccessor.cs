
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Core
{

    /// <summary>Provides read only access to elements of an underlying onedimensional array-like
    /// structure.</summary>
    /// <typeparam name="ElementType">Type of array elements that are accessed via objects of this
    /// class.</typeparam>
    public interface IReadonlyArrayAccessor<ElementType>
    {

        /// <summary>Returns element of the underlying array ad position <paramref name="index"/>.</summary>
        /// <param name="index">Index of the array element to be returned.</param>
        /// <returns>The element specified by index <paramref name="index"/>.</returns>
        ElementType this[int index] { get; }

        /// <summary>Number of elements of the underlying array.</summary>
        int Count { get; }

        /// <summary>Whether the underlying array-like structure is null.</summary>
        bool IsArrayNull { get; }

        /// <summary>Whether write access to elements is provided by the current element.
        /// <para>This will normally be determined by the type of the implementing class, but
        /// the property makes it possible to query the information programatically.</para></summary>
        bool IsWritable { get; }

        /// <summary>Whether the underlying array structure can be reallocated (resized), i.e.,
        /// whether a reference to the underlying array can change.
        /// <para>This will normally be determined by the type of the implementing class, but
        /// the property makes it possible to query the information programatically.</para></summary>
        bool IsResizable { get; }

    }

}
