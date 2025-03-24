using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//#if !NET8_0_OR_GREATER
//using System.Collections.Specialized; // Fallback to non-generic OrderedDictionary
//#endif
namespace IG.Graphics3D.other.ModelParameters
{

    /// <summary>Contains a set of model parameters as <see cref="ModelParameter"/> objects.
    /// <para>It contains metadata such as name, title, and description of the set.</para>
    /// <para>Parameters can be referenced both by parameter name and parameter index. Therefore,
    /// parameters can be ordered.</para></summary>
    class ModelParameterSet
    {


//#if NET8_0_OR_GREATER
//        protected OrderedDictionary<string, ModelParameter> Parameters { get; } =
//            new OrderedDictionary<string, ModelParameter>();
//#else
//        protected System.Collections.OrderedDictionary Parameters { get; } =
//                    new Dictionary<string, ModelParameter>();
//#endif

        protected SortedDictionary<string, ModelParameter> Parameters { get; } =
            new SortedDictionary<string, ModelParameter>();

    }
}
