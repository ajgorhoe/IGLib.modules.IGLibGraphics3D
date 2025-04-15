using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Core
{

    /// <summary>Provides readonly access to elements of a fixed array. This is typically used
    /// to access array-like internal elements of other classes, supported by an internal 
    /// underlying field or variable  of type <see cref="ElementType[]"/>.
    /// <para>IMPORTANT: To use this class, the underlying array may not be reallocated during the
    /// lifetime of this class's object. If the underlying array may change, use the
    /// <see cref="ReadonlyArrayAccessorRef{ElementType}"/> instead.</para></summary>
    /// <typeparam name="ElementType">Type of elements of array to which access is provided
    /// by objects of this class.</typeparam>
    public class ReadonlyArrayAccessor<ElementType> : IReadonlyArrayAccessor<ElementType>
    {

        /// <summary>Initializes the class with a reference to the underlying array, which will
        /// be used to access elements.</summary>
        /// <param name="array"></param>
        public ReadonlyArrayAccessor(ElementType[] array)
        {
            _array = array;
            IsWritable = false;
            IsResizable = false;
        }

        /// <summary>The underlying array containing the elements. The referenced external array should
        /// not change for this class.</summary>
        protected readonly ElementType[] _array;

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException"></exception>
        public ElementType this[int index]
        {
            get {
                if (_array == null)
                {
                    throw new InvalidOperationException("The underlying array is not allocated.");
                }
                return _array[index];
            }
        }

        /// <inheritdoc/>
        public int Count => _array == null ? 0 : _array.Length;

        /// <inheritdoc/>
        public bool IsArrayNull => _array == null;

        /// <inheritdoc/>
        public bool IsWritable { get; init; }

        /// <inheritdoc/>
        public bool IsResizable { get; init; }

    }

    /// <summary>Provides read/write access to elements of a fixed array. This is typically used
    /// to access array-like internal elements of other classes, supported by an internal 
    /// underlying field or variable  of type <see cref="ElementType[]"/>.
    /// <para>IMPORTANT: To use this class, the underlying array may not be reallocated / resized
    /// during the lifetime of this class's object. If the underlying array may change, use the
    /// <see cref="ArrayAccessorRef{ElementType}"/> instead.</para></summary>
    /// <typeparam name="ElementType">Type of elements of array to which access is provided
    /// by objects of this class.</typeparam>
    public class ArrayAccessor<ElementType> : ReadonlyArrayAccessor<ElementType>,
        IArrayAccessor<ElementType>
    {

        /// <summary>Constructor, initializes the current array accessor with the underlying array
        /// object.</summary>
        /// <param name="array">The underlying array object that holds elements accessd by the current
        /// object.</param>
        public ArrayAccessor(ElementType[] array): base(array)
        {  }

        /// <inheritdoc/>
        public new ElementType this[int index]
        {
            get { return base[index]; }
            set
            {
                this[index] = value;
            }
        }

    }


}
