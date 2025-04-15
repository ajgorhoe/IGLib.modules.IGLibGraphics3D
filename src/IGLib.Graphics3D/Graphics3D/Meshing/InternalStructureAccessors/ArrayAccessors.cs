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

        /// <summary>Th eunderlying array containing the elements. The referenced external array should
        /// not change for this class.</summary>
        protected readonly ElementType[] _array;

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

        public int Count => _array == null ? 0 : _array.Length;

        public bool IsArrayNull => _array == null;

        public bool IsWritable { get; init; }

        public bool IsResizable { get; init; }

    }

    public class ArrayAccessor<ElementType> : ReadonlyArrayAccessor<ElementType>,
        IArrayAccessor<ElementType>
    {

        ArrayAccessor(ElementType[] array): base(array)
        {  }

        public ElementType this[int index]
        {
            get { return base[index]; }
            set
            {
                this[index] = value;
            }
        }

    }


}
