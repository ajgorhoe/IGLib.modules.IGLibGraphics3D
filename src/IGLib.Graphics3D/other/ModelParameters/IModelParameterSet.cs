using System.Collections.Generic;

namespace IGLib.Core
{

    /// <summary>Contains infomrmation on modle parameters in a specific parameter set. This
    /// usually represents all parameters that are used in the certain parameterized model,
    /// or a related group of model parameters.</summary>
    public interface IModelParameterSet<ModelParameterType>
        where ModelParameterType : IModelParameter
    {


        /// <summary>Title when introducing the parameter set, for example as title in 
        /// documentation, as label in user interfaces where parameter set is displayed or 
        /// where it could be edited, etc.</summary>
        string Title { get; }
       
        /// <summary>Description of the parameter set, usually a short one, as it may be used in
        /// documentation describing the model or in user interfaces (e.g. as tooltip).</summary>
        string Description { get; }

        /// <summary>Gets a readonly list of model parameter objects, ordered in the same order
        /// in which parameters were added to the set during parameterization. This enables 
        /// preservation of sequential order when this is importatnt.</summary>
        IReadOnlyList<ModelParameterType> ParameterList { get; }

        /// <summary>Number of parameters contained in the set.</summary>
        int Count { get; }

        /// <summary>Index operator, gets the specific model parameter by its sequential order
        /// (counting from 0) in which parameters were added to the parameter set.</summary>
        /// <param name="whichParameter">Index of the parameter by the order in whiich parameters were added to the set.</param>
        /// <returns>The parameter that was added at place <paramref name="whichParameter"/></returns>
        ModelParameterType this[int whichParameter] { get; }

        /// <summary>Index operator, gets the specific parameter by its name.</summary>
        /// <param name="parameterName">Name of the parameter to be returned.</param>
        /// <returns>The parameter object corresponding to the <paramref name="parameterName"/>,
        /// containing information on that parameter.</returns>
        ModelParameterType this[string parameterName] { get; }

    }
}