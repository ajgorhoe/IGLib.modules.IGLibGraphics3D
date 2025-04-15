using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Core
{
    public interface IReadonlyArrayAccessor<ElementType>
    {


        int Count { get; }

        bool IsWritable { get; }

        bool IsResizable { get; }

        ElementType this[int index] { get; }

    }

}
