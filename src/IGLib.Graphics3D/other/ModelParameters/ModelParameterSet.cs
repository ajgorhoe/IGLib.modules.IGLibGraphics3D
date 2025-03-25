using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IGLib.Core
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

        protected Dictionary<string, IModelParameter> ParametersDictionary { get; } =
            new Dictionary<string, IModelParameter>();

        protected List<string> ParameterNamesList { get; } = new List<string>();

        /// <summary>Number of parameters currently contained in this set.</summary>
        public int Ckount => ParametersDictionary.Count;

        public IReadOnlyList<IModelParameter> ParameterList => ParametersDictionary.Values.ToList();

        protected void AddParameter(string name, IModelParameter parameter)
        {
            if (ParametersDictionary.ContainsKey("name"))
            {
                throw new InvalidOperationException($"Parameter {name} is already contained in the set, you can only add a parameter once.");
                // Parameters[name] = parameter;
            } else
            {

            }

        }

    }
}
