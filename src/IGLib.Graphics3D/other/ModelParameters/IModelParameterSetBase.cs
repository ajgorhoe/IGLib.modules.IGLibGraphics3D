using System;
using System.Collections.Generic;

namespace IGLib.Core
{

    /// <summary>A set of model parameters of the specified type, where parameters can be added,
    /// removed or changed after initialization of the set.
    /// <para>This can represents all parameters that are used in the certain parameterized model,
    /// or a related group of parameters.</para></summary>
    /// <typeparam name="ModelParameterType">Specifies the type of parameter objects contained in the 
    /// parameter set (need to implement the <see cref="IModelParameter"/> interface).</typeparam>

    public interface IModelParameterSetBase<ModelParameterType>: IModelParameterSetBaseFixed<ModelParameterType>
        where ModelParameterType : IModelParameter
    {

        /// <summary>Adds the specified parameter objects to the currrent parameter set at the specified keys.
        /// <para>Exception of type <see cref="InvalidOperationException"/> is thrown if any of the keys 
        /// already exists. Use <see cref="AddOrReplace(ValueTuple{string, ModelParameterType}[])"/> to allow
        /// replaclement of parameters at existing keys and avoid exceptions.</para></summary>
        /// <param name="keysAndParameters">Zero or more tuples containing a key at which the parameter object 
        /// is added, and the parameter object itself.</param>
        void Add(params (string Key, ModelParameterType Parameter)[] keysAndParameters);


        /// <summary>Adds the specified parameter objects to the currrent parameter set at the specified keys.
        /// If any of the keys is already contained in the set then the corresponding parameter object
        /// replaces the current parameter object at that key.</summary>
        /// <param name="keysAndParameters">Zero or more tuples containing a key at which the parameter object 
        /// is added, and the parameter object itself.</param>
        void AddOrReplace(params (string Key, ModelParameterType Parameter)[] keysAndParameters);

        /// <summary>Adds the specified parameter objects to the currrent parameter set at keys that
        /// equal to parameter names (the <see cref="IModelParameter.Name"/> property).
        /// <para>Exception of type <see cref="InvalidOperationException"/> is thrown if any of the keys 
        /// already exists. Use <see cref="AddOrReplace(ModelParameterType[])"/> to allow replaclement 
        /// of parameters at existing keys and avoid exceptions.</para></summary>
        /// <param name="parameters">Zero or more parameter objects to be added.</param>
        void Add(params ModelParameterType[] parameters);

        /// <summary>Adds the specified parameter objects to the currrent parameter set at keys that
        /// equal parameter names (the <see cref="IModelParameter.Name"/> property).
        /// If any of the keys is already contained in the set then the corresponding parameter object
        /// replaces the current parameter object at that key.</summary>
        /// <param name="parameters">Zero or more parameter objects to be added.</param>
        void AddOrReplace(params ModelParameterType[] parameters);

        /// <summary>Removes parameter objects at the specified keys. If any of the specified keys is
        /// not contained in the parameter set, it is ignored and nothing happens for that key.</summary>
        /// <param name="keys">Keys for which parameter objects are removed from the current parameter set.</param>
        void Remove(params string[] keys);

        /// <summary>Index operator, gets or sets the specific parameter by its name.</summary>
        /// <param name="parameterName">Access key of the parameter. This can equal parameter's
        /// <see cref="ModelParameterType.Name"/> property, but this is not necessary.</param>
        new ModelParameterType this[string parameterName] { get; set; }


        /// <summary>Index operator, gets or sets the specific parameter by its sequential number,
        /// which defines the order by which parameter was added.</summary>
        /// <param name="whichParameter">Sequential number of the parameter, according to the order
        /// in which parameters were added.
        /// <para>Warning:</para>
        /// <para></para></param>
        new ModelParameterType this[int whichParameter] { get; set; }

    }

    /// <summary>A fixed set of model parameters of the specified type, where parameters cannot be added,
    /// removed or changed after initialization of the set. 
    /// <para>This can represents all parameters that are used in the certain parameterized model,
    /// or a related group of parameters.</para></summary>
    /// <typeparam name="ModelParameterType">Specifies the type of parameter objects contained in the 
    /// parameter set (need to implement the <see cref="IModelParameter"/> interface).</typeparam>
    public interface IModelParameterSetBaseFixed<ModelParameterType>
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

        /// <summary>Returns true if the current parameter set contains the specified key.</summary>
        /// <param name="key">Tke key that is queried.</param>
        bool ContainsKey(string key);

        /// <summary>Index operator, gets the specific parameter by its name.</summary>
        /// <param name="parameterName">Name of the parameter to be returned.</param>
        /// <returns>The parameter object corresponding to the <paramref name="parameterName"/>,
        /// containing information on that parameter.</returns>
        ModelParameterType this[string parameterName] { get; }

        /// <summary>Index operator, gets the specific model parameter by its sequential order
        /// (counting from 0) in which parameters were added to the parameter set.</summary>
        /// <param name="whichParameter">Index of the parameter by the order in whiich parameters were added to the set.</param>
        /// <returns>The parameter that was added at place <paramref name="whichParameter"/></returns>
        ModelParameterType this[int whichParameter] { get; }

        /// <summary>Whether or not parameters can be added after the parameter set has ben created.</summary>
        bool CanAddParameters { get; }

    }
}