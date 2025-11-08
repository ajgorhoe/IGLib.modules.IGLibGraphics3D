
#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Core
{

    /// <summary>A generic delegate that returns a reference to an object of the
    /// specific type.
    /// <para>Similar to <see cref="Func{TResult}"/>, but instead of a value (object)
    /// of type T it returns a reference to a variable or field of this type.</para>
    /// <para>Returned value can be used to set a variable of type <typeparamref name="TResult"/>
    /// or of type "ref <typeparamref name="TResult"/>", or as return of a method with such 
    /// reference return type.</para></summary>
    /// <typeparam name="TResult">Type of object whose reference is returned by the delegate.</typeparam>
    /// <returns>A reference to an object of type T, through which the object can be accessed
    /// or set.</returns>
    public delegate ref TResult FuncRef<TResult>();

}
