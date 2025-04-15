using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Core
{
    public interface IArrayAccessor<ElementType>: IReadonlyArrayAccessor<ElementType>
    {

        new ElementType this[int index] { get; set; }

    }

}
