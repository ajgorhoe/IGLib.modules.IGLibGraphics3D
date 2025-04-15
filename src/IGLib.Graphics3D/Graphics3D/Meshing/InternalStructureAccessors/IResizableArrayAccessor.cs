using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGLib.Core
{
    public interface IResizableArrayAccessor<ElementType>: IArrayAccessor<ElementType>
    {

        void Resize(int newCount, bool keepElements = false);

    }

}
